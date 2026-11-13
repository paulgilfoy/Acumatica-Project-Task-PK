<#
.SYNOPSIS
  Type-checks the repo's Modern UI TypeScript (src/FrontendSources/screens/**/*.ts) against PK9's real
  client-controls typings and base screens. NO emit, writes nothing to PK9 (temp tsconfig in %TEMP%).

.DESCRIPTION
  Uses PK9's own Node.js (AcumaticaTools) and TypeScript (FrontendSources\screen\node_modules\typescript).
  Catches wrong imports, unknown decorators/properties, wrong base-screen class names, etc.
  Does NOT validate HTML (selectors, tags) - those only fail at publish (log: PK9\App_Data\logs).
  Prints "TS CHECK OK" on success; exits 1 on errors.

.EXAMPLE
  .\tools\Test-ModernUI.ps1
#>
param(
    [string]$SitePath = 'C:\Program Files\Acumatica ERP\PK9',
    [string]$NodeExe = 'C:\Program Files\AcumaticaTools\NodeJS\node-v22.11.0-win-x64\node.exe'
)
$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
$uiRoot = Join-Path $repo 'src\FrontendSources\screens'
$screen = Join-Path $SitePath 'FrontendSources\screen'
$tsc = Join-Path $screen 'node_modules\typescript\bin\tsc'
foreach ($p in $NodeExe, $tsc, $uiRoot) { if (-not (Test-Path $p)) { throw "Not found: $p" } }

$files = @(Get-ChildItem $uiRoot -Recurse -Filter *.ts | ForEach-Object { $_.FullName -replace '\\', '/' })
$cfg = [ordered]@{
    compilerOptions = [ordered]@{
        noEmit = $true; strictPropertyInitialization = $false; module = 'esnext'; target = 'es6'
        lib = @('es2017', 'dom', 'DOM.Iterable'); moduleResolution = 'node'
        experimentalDecorators = $true; emitDecoratorMetadata = $true; esModuleInterop = $true
        skipLibCheck = $true; allowJs = $true; resolveJsonModule = $true; types = @()
        baseUrl = ($screen -replace '\\', '/')    # so "src/screens/..." imports resolve to PK9 base screens
        paths = [ordered]@{
            'client-controls'   = @('../node_modules/client-controls')
            'client-controls/*' = @('../node_modules/client-controls/*')
        }
    }
    files = $files
}
$cfgPath = Join-Path $env:TEMP 'PKTimeClock_tsconfig.json'
$cfg | ConvertTo-Json -Depth 6 | Set-Content $cfgPath -Encoding UTF8

Write-Host "Type-checking $($files.Count) file(s) with $(& $NodeExe $tsc --version) ..."
$out = & $NodeExe $tsc -p $cfgPath 2>&1
if ($LASTEXITCODE -eq 0) { Write-Host 'TS CHECK OK' -ForegroundColor Green }
else { $out | ForEach-Object { Write-Host $_ -ForegroundColor Red }; exit 1 }
