<#
.SYNOPSIS
  Builds the Acumatica customization package (zip with project.xml) for the "ProjectTask" project from src\.
  Output: build\ProjectTask.zip (git-ignored). Writes nothing outside build\. Does NOT import or publish.

.DESCRIPTION
  Modeled on ..\Acumatica-PK-TimeClock\tools\Build-Package.ps1 (format verified there by reflecting
  PX.Web.Customization; see its scratchpad "Customization packaging"). Mapping (src -> project.xml item / zip entry):
    Code\*.cs                  -> <Graph ClassName="<file name>" FileType="<see $CodeFileType>"> with CDATA Source
    Sql\*.sql                  -> <Sql TableName="<file name>" CustomScript> with CDATA      (must be idempotent)
    Tables\*.xml               -> <Table> elements, verbatim (Usr columns on existing tables)
    SiteMap\*.xml              -> <SiteMapNode> elements, verbatim
    Automation\*.xml           -> <Automation screen="..."> elements, verbatim   (workflow field/condition edits)
    PageCustomizations\*.xml   -> <Page path="~/pages/..."> elements, verbatim  (Classic screen-editor edits)
    Pages\**                   -> <File AppRelativePath="Pages\..."/> + zip entry at that path (ASPX shells)
    FrontendSources\screens\** -> <PerTenantFile AppRelativePath="screens\..." ScreenId="..."/> + zip entry (Modern UI)
  Automation\ and PageCustomizations\ are optional folders (live items exported by tools\Get-LiveProject.ps1).

  Line endings: Acumatica parses project.xml as XML, so CRLF inside CDATA is stored as LF in CustObject.Content;
  package line endings do not change what is stored. Verify with tools\Verify-Package.ps1.

  Importing a zip into an existing project REPLACES all its items: anything not in src\ is removed on import.
  Run ..\Acumatica-PK-TimeClock\tools\Test-Compile.ps1 / tools\Test-Compile.ps1 and tools\Test-ModernUI.ps1 first.

.EXAMPLE
  .\tools\Build-Package.ps1
  .\tools\Build-Package.ps1 -SrcDir build\live\src -OutFile build\ProjectTask-live.zip   # rebuild the live project
#>
param(
    [string]$ProjectName = 'ProjectTask',
    [string]$Description = 'Adding various Data fields to the Project and Task screens',
    [string]$ProductVersion = '26.200',
    [string]$SrcDir,
    [string]$OutFile,
    # Customization-editor item kind per code file (live values). Files not listed get "NewFile".
    [hashtable]$CodeFileType = @{
        PMDocGatheringMaint  = 'NewGraph'; PMSampleTestingMaint = 'NewGraph'
        PMDocumentGathering  = 'NewDac';   PMSampleTesting      = 'NewDac'
    },
    [switch]$SkipModernUI
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression, System.IO.Compression.FileSystem
$repo = Split-Path $PSScriptRoot -Parent
$src  = if ($SrcDir) { (Resolve-Path $SrcDir).Path } else { Join-Path $repo 'src' }
if (-not $OutFile) { $OutFile = Join-Path $repo "build\$ProjectName.zip" }
$OutFile = [IO.Path]::GetFullPath($(if ([IO.Path]::IsPathRooted($OutFile)) { $OutFile } else { Join-Path (Get-Location) $OutFile }))
New-Item -ItemType Directory -Force (Split-Path $OutFile -Parent) | Out-Null

function CData([string]$text, [string]$what) {
    if ($text.Contains(']]>')) { throw "$what contains ']]>' which cannot be embedded in CDATA." }
    "<![CDATA[$text]]>"
}
function Read-Text([string]$path) { [IO.File]::ReadAllText($path).TrimStart([char]0xFEFF) }
function Add-Verbatim($sb, [string]$path, [string]$expectedRoot) {
    $text = (Read-Text $path).Trim()
    [xml]$doc = $text   # must be well-formed
    if ($doc.DocumentElement.Name -ne $expectedRoot) { throw "$path : root element <$($doc.DocumentElement.Name)>, expected <$expectedRoot>." }
    [void]$sb.AppendLine(($text -split "`r?`n" | ForEach-Object { "    $_" }) -join "`r`n")
}
function Get-Files([string]$sub, [string]$filter) {
    $dir = Join-Path $src $sub
    if (Test-Path $dir) { Get-ChildItem $dir -Filter $filter -File | Sort-Object Name }
}

$sb = New-Object System.Text.StringBuilder
[void]$sb.AppendLine("<Customization level=`"`" description=`"$([Security.SecurityElement]::Escape($Description))`" product-version=`"$ProductVersion`">")
$zipFiles = @()
$count = [ordered]@{}

foreach ($f in Get-Files 'Code' '*.cs') {
    $type = if ($CodeFileType.ContainsKey($f.BaseName)) { $CodeFileType[$f.BaseName] } else { 'NewFile' }
    [void]$sb.AppendLine("    <Graph ClassName=`"$($f.BaseName)`" Source=`"#CDATA`" IsNew=`"True`" FileType=`"$type`">")
    [void]$sb.AppendLine("        <CDATA name=`"Source`">$(CData (Read-Text $f.FullName) $f.Name)</CDATA>")
    [void]$sb.AppendLine('    </Graph>')
    $count.Code++
}
foreach ($f in Get-Files 'Sql' '*.sql') {
    [void]$sb.AppendLine("    <Sql TableName=`"$($f.BaseName)`" CustomScript=`"#CDATA`">")
    [void]$sb.AppendLine("        <CDATA name=`"CustomScript`">$(CData (Read-Text $f.FullName) $f.Name)</CDATA>")
    [void]$sb.AppendLine('    </Sql>')
    $count.Sql++
}
foreach ($f in Get-Files 'Tables' '*.xml')             { Add-Verbatim $sb $f.FullName 'Table';       $count.Table++ }
foreach ($f in Get-Files 'SiteMap' '*.xml')            { Add-Verbatim $sb $f.FullName 'SiteMapNode'; $count.SiteMapNode++ }
foreach ($f in Get-Files 'Automation' '*.xml')         { Add-Verbatim $sb $f.FullName 'Automation';  $count.Automation++ }
foreach ($f in Get-Files 'PageCustomizations' '*.xml') { Add-Verbatim $sb $f.FullName 'Page';        $count.Page++ }

# Site files (ASPX shells): src\Pages\PK\X.aspx -> AppRelativePath Pages\PK\X.aspx
$pagesRoot = Join-Path $src 'Pages'
if (Test-Path $pagesRoot) {
    Get-ChildItem $pagesRoot -Recurse -File | Sort-Object FullName | ForEach-Object {
        $rel = 'Pages\' + $_.FullName.Substring($pagesRoot.Length + 1)
        [void]$sb.AppendLine("    <File AppRelativePath=`"$rel`" />")
        $zipFiles += [pscustomobject]@{ Rel = $rel; Full = $_.FullName }
        $count.File++
    }
}

# Modern UI: src\FrontendSources\screens\PM\PM301000\extensions\x.ts -> AppRelativePath screens\PM\PM301000\extensions\x.ts
$uiRoot = Join-Path $src 'FrontendSources'
if (-not $SkipModernUI -and (Test-Path $uiRoot)) {
    Get-ChildItem $uiRoot -Recurse -File | Sort-Object FullName | ForEach-Object {
        $rel = $_.FullName.Substring($uiRoot.Length + 1)
        $parts = $rel -split '\\'
        if ($parts[0] -ne 'screens' -or $parts.Count -lt 4) { throw "Unexpected Modern UI path: $rel" }
        [void]$sb.AppendLine("    <PerTenantFile AppRelativePath=`"$rel`" ScreenId=`"$($parts[2])`" />")
        $zipFiles += [pscustomobject]@{ Rel = $rel; Full = $_.FullName }
        $count.PerTenantFile++
    }
}
[void]$sb.AppendLine('</Customization>')
[xml]$null = $sb.ToString()   # validate

if (Test-Path $OutFile) { Remove-Item $OutFile -Force -Confirm:$false }
$zip = [IO.Compression.ZipFile]::Open($OutFile, 'Create')
try {
    $entry = $zip.CreateEntry('project.xml')
    $w = New-Object IO.StreamWriter($entry.Open(), (New-Object Text.UTF8Encoding($false)))
    $w.Write($sb.ToString()); $w.Close()
    foreach ($f in $zipFiles) { [void][IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, $f.Full, $f.Rel) }
} finally { $zip.Dispose() }

$summary = ($count.GetEnumerator() | ForEach-Object { "$($_.Key) $($_.Value)" }) -join ', '
$total = ($count.Values | Measure-Object -Sum).Sum
Write-Host "Built $OutFile  ($total items: $summary)" -ForegroundColor Green
if (-not $count.Automation -or -not $count.Page) {
    Write-Host 'Note: the source has no Automation\ and/or PageCustomizations\ items. Live ProjectTask had both on 2026-11-20;' -ForegroundColor Yellow
    Write-Host '      importing a zip without them REMOVES them from the project. Check with tools\Verify-Package.ps1.' -ForegroundColor Yellow
}
if ($SkipModernUI) { Write-Host 'Modern UI files NOT included (-SkipModernUI).' -ForegroundColor Yellow }
