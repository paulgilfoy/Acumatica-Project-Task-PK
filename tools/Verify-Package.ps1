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

.EXAMPLE
  .\tools\Build-Package.ps1; .\tools\Verify-Package.ps1
  .\tools\Verify-Package.ps1 -Zip build\ProjectTask-live.zip
#>
param(
    [string]$Zip,
    [string]$ProjectName = 'ProjectTask',
    [int]$CompanyID = 2,
    [string]$SitePath = 'C:\Program Files\Acumatica ERP\PK9'
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
function CanonXml([string]$t) {
    $d = New-Object Xml.XmlDocument; $d.PreserveWhitespace = $false; $d.LoadXml($t.TrimStart([char]0xFEFF)); $d.DocumentElement.OuterXml
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
$live = @{}
foreach ($i in & (Join-Path $PSScriptRoot 'Get-LiveProject.ps1') -ProjectName $ProjectName -CompanyID $CompanyID -SitePath $SitePath -NoWrite) {
    $live[$i.Key] = $i
}

# ---- Compare ----
$rows = @(); $fail = 0; $warn = 0
foreach ($k in (@($pkg.Keys) + @($live.Keys) | Sort-Object -Unique)) {
    $p = $pkg[$k]; $l = $live[$k]; $status = 'OK'; $note = ''
    if (-not $p) { $status = 'MISSING IN ZIP'; $note = "live item ($($l.SrcRel)) not in the package" }
    elseif (-not $l) { $status = 'NOT LIVE'; $note = 'in the package, not in the live project' }
    else {
        $diffAttr = @($l.Attr.Keys | Where-Object { "$($l.Attr[$_])" -cne "$($p.Attr[$_])" } | ForEach-Object { "$_ live='$($l.Attr[$_])' zip='$($p.Attr[$_])'" })
        if ($diffAttr.Count) { $status = 'DIFF'; $note = $diffAttr -join '; ' }
        elseif ($null -ne $l.Xml -or $null -ne $p.Xml) {
            if ((CanonXml $l.Xml) -cne (CanonXml $p.Xml)) { $status = 'DIFF'; $note = 'XML content differs' }
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
    $rows += [pscustomobject]@{ Status = $status; Item = $k; Note = $note.Trim('; ') }
}

# Runtime copies of code items
$rt = Join-Path $SitePath 'App_RuntimeCode'
foreach ($p in $pkg.Values | Where-Object Type -eq 'Graph') {
    $cls = $p.Attr.ClassName
    $file = @("$rt\$ProjectName.$cls.cs", "$rt\$cls.cs") | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
    if (-not $file) { $warn++; $rows += [pscustomobject]@{ Status = 'WARN'; Item = "App_RuntimeCode|$cls"; Note = 'no runtime copy (not published?)' }; continue }
    if ((Norm ([IO.File]::ReadAllText($file))) -cne (Norm $p.Body)) {
        $warn++; $rows += [pscustomobject]@{ Status = 'WARN'; Item = "App_RuntimeCode|$cls"; Note = "differs from $(Split-Path $file -Leaf) (published code is different)" }
    } else { $rows += [pscustomobject]@{ Status = 'OK'; Item = "App_RuntimeCode|$cls"; Note = "= $(Split-Path $file -Leaf)" } }
}

$rows | Sort-Object @{ e = { $_.Status -eq 'OK' } }, Item | Format-Table -AutoSize -Wrap | Out-String -Width 220 | Write-Host
Write-Host ("Zip: {0} items; live {1}: {2} items (CompanyID {3}). Problems: {4}; runtime-copy warnings: {5}." -f `
    $pkg.Count, $ProjectName, $live.Count, $CompanyID, $fail, $warn)
if ($fail -eq 0) { Write-Host 'PACKAGE MATCHES LIVE' -ForegroundColor Green; exit 0 }
else { Write-Host 'PACKAGE DIFFERS FROM LIVE' -ForegroundColor Red; exit 1 }
