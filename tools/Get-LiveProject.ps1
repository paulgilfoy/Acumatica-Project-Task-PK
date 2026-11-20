<#
.SYNOPSIS
  READ-ONLY: reads the customization project "ProjectTask" (CompanyID 2) from the PK9 database and writes it as a
  src-shaped tree (default build\live\src) plus build\live\items.csv. Never writes to the database or to src\.

.DESCRIPTION
  Items come from CustObject (joined to CustProject by ProjID). Item -> path mapping (same as tools\Build-Package.ps1):
    Graph         Code#<X>                       -> Code\<X>.cs              (CDATA "Source" body)
    Sql           Sql#<X>                        -> Sql\<X>.sql              (CDATA "CustomScript" body)
    Table         <X>                            -> Tables\<X>.xml           (verbatim item XML)
    SiteMapNode   SiteMapNode#<NodeID>           -> SiteMap\<ScreenID>.xml   (verbatim item XML)
    Automation    Automation#<SCREEN>            -> Automation\<SCREEN>.xml  (verbatim item XML)
    Page          ~/pages/<mod>/<screen>.aspx    -> PageCustomizations\<SCREEN>.xml (verbatim item XML, Classic page edit)
    File          File#Pages\PK\X.aspx           -> Pages\PK\X.aspx          (file body, see below)
    PerTenantFile PerTenantFile#screens\...      -> FrontendSources\screens\... (file body, see below)

  File bodies: File / PerTenantFile items store only a FileID. On PK9 the bodies are NOT in the database:
  UploadFileRevision.Data is empty and BlobHandler points at external (Azure) blob storage (BlobStorageConfig).
  This tool does not access blob storage. It reads the published copy on the PK9 site instead
  (File -> <site>\<AppRelativePath>; PerTenantFile -> <site>\FrontendSources\screen\src\customizationScreens\<tenant>\<AppRelativePath>)
  and records BodySource = 'site' (or 'db' when the revision has data). UploadFileRevision.Size (KB, rounded up)
  is kept in SizeKB as a sanity check.

  Text is written UTF-8 (no BOM) with the database's line endings (LF); site copies are copied byte for byte.
  Compare with line-ending normalization (tools\Verify-Package.ps1 does).

  Output objects (-PassThru / -NoWrite; used by tools\Verify-Package.ps1):
    Type, Name, Key ("Type|Name"), SrcRel (path under src\), Attr (significant attributes), Body (text),
    Xml (item XML for Table/SiteMapNode/Automation/Page), Bytes, SizeKB, BodySource.

  Re-sync of src\ (T-90): pass -OutDir <repo>\src ONLY when the lead/user asks; the default never touches src\.

.EXAMPLE
  .\tools\Get-LiveProject.ps1                       # -> build\live\src + build\live\items.csv
  .\tools\Get-LiveProject.ps1 -NoWrite | ft Type, Name, SrcRel, BodySource
#>
param(
    [string]$ProjectName = 'ProjectTask',
    [int]$CompanyID = 2,
    [string]$OutDir,
    [string]$SitePath = 'C:\Program Files\Acumatica ERP\PK9',
    [string]$TenantFolder = 'Company',   # login name of CompanyID 2 (customizationScreens\<tenant>)
    [switch]$NoWrite,                    # only return the item objects
    [switch]$PassThru
)
$ErrorActionPreference = 'Stop'
$repo  = Split-Path $PSScriptRoot -Parent
$query = Join-Path (Split-Path $repo -Parent) 'Acumatica-PK-TimeClock\tools\Query-PK9.ps1'
if (-not (Test-Path $query)) { throw "Read-only query tool not found: $query" }
if (-not $OutDir) { $OutDir = Join-Path $repo 'build\live' }
if ($ProjectName -notmatch '^[\w.\-]+$') { throw "Invalid project name: $ProjectName" }

function Invoke-ReadOnlyQuery([string]$sql) {
    $tmp = Join-Path $env:TEMP ("pt-live-{0}-{1}.csv" -f $PID, [guid]::NewGuid().ToString('N'))
    try {
        & $query -Sql $sql -OutCsv $tmp 6>$null | Out-Null   # long text: -OutCsv + Import-Csv (TimeClock learning)
        if (Test-Path $tmp) { @(Import-Csv $tmp) } else { @() }
    } finally { if (Test-Path $tmp) { Remove-Item $tmp -Force -Confirm:$false } }
}

$objects = Invoke-ReadOnlyQuery @"
SELECT o.Type, o.Name, o.Content
FROM CustObject o
JOIN CustProject p ON p.CompanyID = o.CompanyID AND p.ProjID = o.ProjectID
WHERE p.Name = N'$ProjectName' AND o.CompanyID = $CompanyID
ORDER BY o.Type, o.Name
"@
if (-not $objects.Count) { throw "No items found for project '$ProjectName' (CompanyID $CompanyID)." }

$files = Invoke-ReadOnlyQuery @"
SELECT o.Name, r.Size,
  CAST('' AS XML).value('xs:base64Binary(sql:column("r.Data"))', 'varchar(max)') AS DataB64
FROM CustObject o
JOIN CustProject p ON p.CompanyID = o.CompanyID AND p.ProjID = o.ProjectID
JOIN UploadFile f ON f.CompanyID = o.CompanyID AND f.FileID = CAST(o.Content AS xml).value('(/*/@FileID)[1]', 'uniqueidentifier')
JOIN UploadFileRevision r ON r.CompanyID = f.CompanyID AND r.FileID = f.FileID AND r.FileRevisionID = f.LastRevisionID
WHERE p.Name = N'$ProjectName' AND o.CompanyID = $CompanyID AND o.Type IN ('File', 'PerTenantFile')
"@
$fileInfo = @{}
foreach ($f in $files) { $fileInfo[$f.Name] = $f }

function Get-CData([xml]$doc, [string]$name) {
    $n = $doc.DocumentElement.SelectSingleNode("CDATA[@name='$name']")
    if (-not $n) { throw "Item has no CDATA '$name': $($doc.DocumentElement.Name)" }
    $n.InnerText
}

$items = foreach ($o in $objects) {
    [xml]$doc = $o.Content
    $root = $doc.DocumentElement
    $attr = @{}; $body = $null; $xmlText = $null; $bytes = $null; $sizeKB = $null; $bodySource = $null
    switch ($o.Type) {
        'Graph' {
            $rel = "Code\$($root.ClassName).cs"
            $attr = @{ ClassName = $root.ClassName; FileType = $root.FileType; IsNew = $root.IsNew }
            $body = Get-CData $doc 'Source'; $bodySource = 'db'
        }
        'Sql' {
            $rel = "Sql\$($root.TableName).sql"
            $attr = @{ TableName = $root.TableName }
            $body = Get-CData $doc 'CustomScript'; $bodySource = 'db'
        }
        'Table'       { $rel = "Tables\$($root.TableName).xml"; $xmlText = $o.Content }
        'SiteMapNode' { $rel = "SiteMap\$($root.SelectSingleNode('data-set/data/SiteMap/row').ScreenID).xml"; $xmlText = $o.Content }
        'Automation'  { $rel = "Automation\$($root.screen).xml"; $xmlText = $o.Content }
        'Page'        {
            $rel = 'PageCustomizations\' + [IO.Path]::GetFileNameWithoutExtension($root.path).ToUpperInvariant() + '.xml'
            $xmlText = $o.Content
        }
        { $_ -in 'File', 'PerTenantFile' } {
            $app = $root.AppRelativePath
            $attr = @{ AppRelativePath = $app }
            if ($o.Type -eq 'File') { $rel = $app; $copyPath = Join-Path $SitePath $app }
            else {
                $rel = "FrontendSources\$app"; $attr.ScreenId = $root.ScreenId
                $copyPath = Join-Path $SitePath "FrontendSources\screen\src\customizationScreens\$TenantFolder\$app"
            }
            $fi = $fileInfo[$o.Name]
            if ($fi) { $sizeKB = [int]$fi.Size }
            if ($fi -and $fi.DataB64) {
                $bytes = [Convert]::FromBase64String($fi.DataB64); $bodySource = 'db'
            } elseif (Test-Path -LiteralPath $copyPath) {
                $bytes = [IO.File]::ReadAllBytes($copyPath); $bodySource = 'site'
            } else { $bodySource = 'missing' }
            if ($null -ne $bytes) { $body = [Text.Encoding]::UTF8.GetString($bytes) }
        }
        default { $rel = "Other\$($o.Type)\$(($o.Name -replace '[\\/:*?""<>|#~]', '_')).xml"; $xmlText = $o.Content }
    }
    [pscustomobject]@{
        Type = $o.Type; Name = $o.Name; Key = "$($o.Type)|$($o.Name)"; SrcRel = $rel
        Attr = $attr; Body = $body; Xml = $xmlText; Bytes = $bytes; SizeKB = $sizeKB; BodySource = $bodySource
    }
}

if (-not $NoWrite) {
    $srcOut = Join-Path $OutDir 'src'
    $repoSrc = [IO.Path]::GetFullPath((Join-Path $repo 'src')).TrimEnd('\')
    if ([IO.Path]::GetFullPath($OutDir).TrimEnd('\') -eq $repoSrc) {
        Write-Host 'Writing INTO the repo src\ (re-sync). Only do this when the lead/user asked for it.' -ForegroundColor Yellow
        $srcOut = $repoSrc
    } elseif (Test-Path $srcOut) { Remove-Item $srcOut -Recurse -Force -Confirm:$false }
    $utf8 = New-Object Text.UTF8Encoding($false)
    foreach ($i in $items) {
        $path = Join-Path $srcOut $i.SrcRel
        New-Item -ItemType Directory -Force (Split-Path $path -Parent) | Out-Null
        if ($null -ne $i.Bytes) { [IO.File]::WriteAllBytes($path, $i.Bytes) }
        elseif ($null -ne $i.Body) { [IO.File]::WriteAllText($path, $i.Body, $utf8) }
        elseif ($null -ne $i.Xml) { [IO.File]::WriteAllText($path, $i.Xml, $utf8) }
        else { Write-Host "No body for $($i.Key) - not written" -ForegroundColor Yellow }
    }
    if ($srcOut -ne $repoSrc) {
        $items | Select-Object Type, Name, SrcRel, BodySource, SizeKB |
            Export-Csv (Join-Path $OutDir 'items.csv') -NoTypeInformation -Encoding UTF8
    }
    Write-Host "Live '$ProjectName' (CompanyID $CompanyID): $($items.Count) item(s) -> $srcOut" -ForegroundColor Green
}
if ($PassThru -or $NoWrite) { $items }
