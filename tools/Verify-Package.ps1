<#
.SYNOPSIS
  READ-ONLY: checks that a package zip (default build\ProjectTask.zip) has exactly the items of the customization
  project "ProjectTask" on PK9 (CompanyID 2) with the same content, and that the published runtime copies match.
  Prints "PACKAGE MATCHES LIVE" (exit 0) or "PACKAGE DIFFERS FROM LIVE" (exit 1).

.DESCRIPTION
  Live side: tools\Get-LiveProject.ps1 -NoWrite (CustObject rows; file bodies from the PK9 site copies, because
  PK9 keeps uploaded files in external blob storage - see that script).
  Per item (key = CustObject Type + Name, the same identity the TimeClock fingerprint uses):
    Graph          ClassName, FileType, IsNew + Source body
    Sql            TableName + script body
    Table, SiteMapNode, Automation, Page   XML compared after parsing (whitespace between elements ignored)
    File, PerTenantFile                    AppRelativePath (+ScreenId) + zip entry body vs live body; size vs UploadFileRevision.Size (KB)
  Text bodies are compared after removing a BOM, normalizing CRLF/CR to LF and trimming trailing whitespace at the end.
  Acumatica stores LF (XML parsing normalizes line endings), so line endings never count as drift.

  Also compares each code item with its compiled copy in PK9\App_RuntimeCode (<Project>.<Class>.cs, else <Class>.cs)
  and reports differences as WARN (runtime copies change only on publish).

  Exact SHA-256 of CustObject.Content cannot be predicted before import (Acumatica re-serializes the XML),
  so "matches" means: same item set, same significant attributes, same normalized bodies.

  Page items (Classic page customizations): by default the root <Page pageSource="..."> attribute is left out of the
  comparison on both sides. It is a deflate+base64 snapshot of the page Acumatica customized, and Acumatica re-generates
  it on every import, so it never equals the source after a publish although the customization is the same. Everything
  else in the Page item (path, ControlId, every child element and attribute, incl. OriginalIndex) is still compared
  exactly. An item whose only difference was pageSource shows as OK with a note, and a summary line lists them.
  -StrictPageSource compares pageSource too (the old behaviour).

  -ExtraProject (merges, e.g. T-10 "Contact"): the live side becomes the UNION of ProjectTask and the named projects,
  i.e. "importing this zip and unpublishing those projects loses nothing". An item present in two live projects is a
  problem (DUPLICATE LIVE). A live DAC item (Data Class editor, <DAC type="X"> with <Field> children) has no
  package counterpart of the same type, because the merge carries it as a Code item; it counts as OK when a zip
  Graph item declares PXCacheExtension<X> with exactly the DAC's fields (same type, same attribute lines), a zip
  Table item has a column for every StorageName="AddColumn" field, and (WARN otherwise) the code body equals the
  file Acumatica generated from the DAC item, App_RuntimeCode\<X with . -> _>_extensions.cs. That Graph item then
  shows as OK "replaces DAC|X" instead of NOT LIVE.

.EXAMPLE
  .\tools\Build-Package.ps1; .\tools\Verify-Package.ps1
  .\tools\Verify-Package.ps1 -Zip build\ProjectTask-live.zip
  .\tools\Verify-Package.ps1 -ExtraProject Contact     # T-10: zip == live ProjectTask + live Contact
  .\tools\Verify-Package.ps1 -StrictPageSource        # also compare the re-generated Classic pageSource blobs
#>
param(
    [string]$Zip,
    [string]$ProjectName = 'ProjectTask',
    [string[]]$ExtraProject = @(),
    [int]$CompanyID = 2,
    [string]$SitePath = 'C:\Program Files\Acumatica ERP\PK9',
    [switch]$StrictPageSource   # Page items: compare the pageSource attribute too (default: ignored, see .DESCRIPTION)
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression, System.IO.Compression.FileSystem
$repo = Split-Path $PSScriptRoot -Parent
if (-not $Zip) { $Zip = Join-Path $repo "build\$ProjectName.zip" }
if (-not (Test-Path $Zip)) { throw "Zip not found: $Zip (run tools\Build-Package.ps1)" }
$Zip = (Resolve-Path $Zip).Path

function Norm([string]$t) {
    if ($null -eq $t) { return $null }
    ($t.TrimStart([char]0xFEFF) -replace "`r`n", "`n" -replace "`r", "`n").TrimEnd()
}
function CanonXml([string]$t, [switch]$NoPageSource) {
    $d = New-Object Xml.XmlDocument; $d.PreserveWhitespace = $false; $d.LoadXml($t.TrimStart([char]0xFEFF))
    # Only the root <Page> element's pageSource attribute; nothing else is dropped.
    if ($NoPageSource -and $d.DocumentElement.LocalName -eq 'Page') { $d.DocumentElement.RemoveAttribute('pageSource') }
    $d.DocumentElement.OuterXml
}
function Get-ZipText($entry) {
    $sr = New-Object IO.StreamReader($entry.Open(), [Text.Encoding]::UTF8)
    try { $sr.ReadToEnd() } finally { $sr.Dispose() }
}

# ---- Package side ----
$pkg = @{}
$z = [IO.Compression.ZipFile]::OpenRead($Zip)
try {
    $entries = @{}
    foreach ($e in $z.Entries) { $entries[($e.FullName -replace '/', '\')] = $e }
    if (-not $entries.ContainsKey('project.xml')) { throw 'project.xml missing from zip' }
    [xml]$proj = Get-ZipText $entries['project.xml']
    foreach ($n in $proj.DocumentElement.ChildNodes) {
        if ($n.NodeType -ne 'Element') { continue }
        $attr = @{}; $body = $null; $xml = $null; $len = $null
        switch ($n.LocalName) {
            'Graph' { $name = "Code#$($n.ClassName)"; $attr = @{ ClassName = $n.ClassName; FileType = $n.FileType; IsNew = $n.IsNew }
                      $body = $n.SelectSingleNode("CDATA[@name='Source']").InnerText }
            'Sql'   { $name = "Sql#$($n.TableName)"; $attr = @{ TableName = $n.TableName }
                      $body = $n.SelectSingleNode("CDATA[@name='CustomScript']").InnerText }
            'Table'       { $name = $n.TableName; $xml = $n.OuterXml }
            'SiteMapNode' { $name = "SiteMapNode#$($n.SelectSingleNode('data-set/data/SiteMap/row').NodeID)"; $xml = $n.OuterXml }
            'Automation'  { $name = "Automation#$($n.screen)"; $xml = $n.OuterXml }
            'Page'        { $name = $n.path.ToLowerInvariant(); $xml = $n.OuterXml }
            { $_ -in 'File', 'PerTenantFile' } {
                $app = $n.AppRelativePath; $name = "$($n.LocalName)#$app"
                $attr = @{ AppRelativePath = $app }; if ($n.LocalName -eq 'PerTenantFile') { $attr.ScreenId = $n.ScreenId }
                if ($entries.ContainsKey($app)) { $body = Get-ZipText $entries[$app]; $len = $entries[$app].Length }
                else { $body = $null }
            }
            default { $name = "?$($n.LocalName)"; $xml = $n.OuterXml }
        }
        $key = "$($n.LocalName)|$name"
        if ($pkg.ContainsKey($key)) { throw "Duplicate item in project.xml: $key" }
        $pkg[$key] = [pscustomobject]@{ Key = $key; Type = $n.LocalName; Attr = $attr; Body = $body; Xml = $xml; Length = $len }
    }
} finally { $z.Dispose() }

# ---- Live side ----
$live = @{}; $rows = @(); $fail = 0; $warn = 0
$projects = @($ProjectName) + @($ExtraProject | Where-Object { $_ -and $_ -ne $ProjectName })
foreach ($liveProj in $projects) {
    foreach ($i in & (Join-Path $PSScriptRoot 'Get-LiveProject.ps1') -ProjectName $liveProj -CompanyID $CompanyID -SitePath $SitePath -NoWrite) {
        $i | Add-Member -NotePropertyName Project -NotePropertyValue $liveProj -Force
        if ($live.ContainsKey($i.Key)) {
            $fail++; $rows += [pscustomobject]@{ Status = 'DUPLICATE LIVE'; Item = $i.Key; Note = "in live $($live[$i.Key].Project) and $liveProj" }
            continue
        }
        $live[$i.Key] = $i
    }
}
$rt = Join-Path $SitePath 'App_RuntimeCode'

# ---- Live DAC items (Data Class editor) carried as Code items ----
$dacResult = @{}   # live DAC key -> row
$replaces  = @{}   # zip Graph key -> live DAC key
foreach ($l in @($live.Values | Where-Object { $_.Type -eq 'DAC' -and -not $pkg.ContainsKey($_.Key) })) {
    [xml]$d = $l.Xml
    $type = $d.DocumentElement.type; $short = $type.Split('.')[-1]
    $extRe = "PXCacheExtension<\s*(global::)?($([regex]::Escape($type))|$([regex]::Escape($short)))\s*>"
    $cands = @($pkg.Values | Where-Object { $_.Type -eq 'Graph' -and $_.Body -match $extRe })
    if ($cands.Count -ne 1) {
        $dacResult[$l.Key] = [pscustomobject]@{ Status = 'MISSING IN ZIP'; Item = $l.Key; Note = "$($l.Project): $($cands.Count) zip code items extend $type (need exactly 1)" }
        continue
    }
    $c = $cands[0]; $body = Norm $c.Body; $problems = @(); $names = @()
    foreach ($f in $d.DocumentElement.SelectNodes('Field')) {
        $fn = $f.FieldName; $names += $fn
        $want = @((($f.SelectSingleNode("CDATA[@name='TextAttributes']")).InnerText -split "`n") | ForEach-Object { $_.Trim() } | Where-Object { $_ })
        $m = [regex]::Match($body, "(?s)#region\s+$fn\s*\n(.*?)#endregion")
        if (-not $m.Success) { $problems += "$fn not declared"; continue }
        $region = $m.Groups[1].Value
        $have = @(($region -split "`n" | ForEach-Object { $_.Trim() }) | Where-Object { $_ -like '`[*' })
        if (($have -join '|') -cne ($want -join '|')) { $problems += "$fn attributes differ" }
        if ($region -notmatch "public\s+virtual\s+$([regex]::Escape($f.TypeName))\??\s+$fn\s*\{") { $problems += "$fn not '$($f.TypeName)'" }
        if ($f.StorageName -eq 'AddColumn') {
            $tbl = $pkg["Table|$($f.MapDbTable)"]
            if (-not $tbl -or -not ([xml]$tbl.Xml).SelectSingleNode("Table/Column[@ColumnName='$fn']")) { $problems += "no zip Table column $($f.MapDbTable).$fn" }
        }
    }
    $extra = @([regex]::Matches($body, 'public\s+virtual\s+[\w.?<>]+\s+(\w+)\s*\{') | ForEach-Object { $_.Groups[1].Value } | Where-Object { $names -notcontains $_ })
    if ($extra.Count) { $problems += "extra fields in code: $($extra -join ', ')" }
    $gen = Join-Path $rt (($type -replace '\.', '_') + '_extensions.cs')
    if ($problems.Count) { $st = 'DIFF'; $nt = "vs $($c.Key): $($problems -join '; ')" }
    else { $st = 'OK'; $nt = "$($l.Project) DAC item carried as $($c.Key): $($names.Count) fields, same types and attributes, Table columns present" }
    $dacResult[$l.Key] = [pscustomobject]@{ Status = $st; Item = $l.Key; Note = $nt }
    $replaces[$c.Key] = [pscustomobject]@{ DacKey = $l.Key; Generated = $gen; Project = $l.Project }
}

# ---- Compare ----
$pageSourceIgnored = @()
foreach ($k in (@($pkg.Keys) + @($live.Keys) | Sort-Object -Unique)) {
    $p = $pkg[$k]; $l = $live[$k]; $status = 'OK'; $note = ''
    if ($dacResult.ContainsKey($k)) { $r = $dacResult[$k]; if ($r.Status -ne 'OK') { $fail++ }; $rows += $r; continue }
    if (-not $p) { $status = 'MISSING IN ZIP'; $note = "live item ($($l.SrcRel)) not in the package" }
    elseif (-not $l -and $replaces.ContainsKey($k)) { $note = "replaces live $($replaces[$k].DacKey) ($($replaces[$k].Project))" }
    elseif (-not $l) { $status = 'NOT LIVE'; $note = 'in the package, not in the live project' }
    else {
        $diffAttr = @($l.Attr.Keys | Where-Object { "$($l.Attr[$_])" -cne "$($p.Attr[$_])" } | ForEach-Object { "$_ live='$($l.Attr[$_])' zip='$($p.Attr[$_])'" })
        if ($diffAttr.Count) { $status = 'DIFF'; $note = $diffAttr -join '; ' }
        elseif ($null -ne $l.Xml -or $null -ne $p.Xml) {
            if ((CanonXml $l.Xml) -cne (CanonXml $p.Xml)) {
                if ($p.Type -eq 'Page' -and $l.Type -eq 'Page' -and -not $StrictPageSource) {
                    if ((CanonXml $l.Xml -NoPageSource) -cne (CanonXml $p.Xml -NoPageSource)) {
                        $status = 'DIFF'; $note = 'XML content differs (also besides pageSource)'
                    } else {
                        $note = 'pageSource differs (re-generated on import; ignored, -StrictPageSource compares it)'
                        $pageSourceIgnored += $k
                    }
                } else { $status = 'DIFF'; $note = 'XML content differs' }
            }
        } else {
            if ($null -eq $p.Body) { $status = 'DIFF'; $note = 'zip entry missing' }
            elseif ($null -eq $l.Body) { $status = 'UNVERIFIED'; $note = "no live body ($($l.BodySource))" }
            elseif ((Norm $l.Body) -cne (Norm $p.Body)) { $status = 'DIFF'; $note = "body differs (live source: $($l.BodySource))" }
            elseif ($l.BodySource -eq 'site') { $note = 'body vs PK9 site copy' }
            if ($status -eq 'OK' -and $null -ne $l.SizeKB -and $null -ne $p.Length) {
                $kb = [Math]::Ceiling($p.Length / 1024)
                if ($kb -ne $l.SizeKB) { $note += "; size $kb KB vs UploadFileRevision.Size $($l.SizeKB) KB" }
                else { $note += "; size $kb KB = DB" }
            }
        }
    }
    if ($status -ne 'OK') { $fail++ }
    $from = if ($l -and $l.Project -ne $ProjectName) { "[$($l.Project)] " } else { '' }
    $rows += [pscustomobject]@{ Status = $status; Item = $k; Note = $from + $note.Trim('; ') }
}

# Runtime copies of code items (a Code item that replaces a live DAC item: the file generated from that DAC item)
foreach ($p in $pkg.Values | Where-Object Type -eq 'Graph') {
    $cls = $p.Attr.ClassName
    $cand = @("$rt\$ProjectName.$cls.cs", "$rt\$cls.cs")
    if ($replaces.ContainsKey($p.Key)) { $cand += $replaces[$p.Key].Generated }
    $file = $cand | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
    if (-not $file) { $warn++; $rows += [pscustomobject]@{ Status = 'WARN'; Item = "App_RuntimeCode|$cls"; Note = 'no runtime copy (not published?)' }; continue }
    if ((Norm ([IO.File]::ReadAllText($file))) -cne (Norm $p.Body)) {
        $warn++; $rows += [pscustomobject]@{ Status = 'WARN'; Item = "App_RuntimeCode|$cls"; Note = "differs from $(Split-Path $file -Leaf) (published code is different)" }
    } else { $rows += [pscustomobject]@{ Status = 'OK'; Item = "App_RuntimeCode|$cls"; Note = "= $(Split-Path $file -Leaf)" } }
}

$rows | Sort-Object @{ e = { $_.Status -eq 'OK' } }, Item | Format-Table -AutoSize -Wrap | Out-String -Width 220 | Write-Host
Write-Host ("Zip: {0} items; live {1}: {2} items (CompanyID {3}). Problems: {4}; runtime-copy warnings: {5}." -f `
    $pkg.Count, ($projects -join ' + '), $live.Count, $CompanyID, $fail, $warn)
if ($pageSourceIgnored.Count) {
    Write-Host ("Note: pageSource differed but was ignored on {0} Page item(s) (Acumatica re-generates it on import; -StrictPageSource to compare): {1}" -f `
        $pageSourceIgnored.Count, (($pageSourceIgnored | ForEach-Object { $_ -replace '^Page\|', '' }) -join ', ')) -ForegroundColor Yellow
}
if ($fail -eq 0) { Write-Host 'PACKAGE MATCHES LIVE' -ForegroundColor Green; exit 0 }
else { Write-Host 'PACKAGE DIFFERS FROM LIVE' -ForegroundColor Red; exit 1 }
