<#
.SYNOPSIS
  Compile-checks THIS repo's src\Code\*.cs (ProjectTask) against the PK9 site assemblies (Acumatica 26.200,
  .NET Framework 4.8) with the Roslyn compiler that ships with Acumatica. Prints "COMPILE OK" / "COMPILE FAILED".

.DESCRIPTION
  Adapted from ..\Acumatica-PK-TimeClock\tools\Test-Compile.ps1. That script always compiles the TimeClock repo's
  own src\Code (it resolves the repo from its own folder), so it does NOT check changes made here; use this one.

  Safe: read-only against PK9; the throwaway DLL and .rsp go to %TEMP%. Does NOT publish anything.
  -IncludeRuntimeCode adds every published customization file in PK9\App_RuntimeCode (other projects, including
  PKTimeClock, which reads our UsrPG* fields) EXCEPT the published copies of our own code items:
  for src\Code\X.cs it leaves out App_RuntimeCode\ProjectTask.X.cs if that exists, else App_RuntimeCode\X.cs
  (the PMTask item is published as ProjectTask.PMTask.cs; App_RuntimeCode\PMTask.cs belongs to Velixo).

.EXAMPLE
  .\tools\Test-Compile.ps1 -IncludeRuntimeCode
  .\tools\Test-Compile.ps1 -IncludeRuntimeCode -ExcludeRuntimeFile PX_Objects_CR_Contact_extensions
      # T-10 cutover check: compile as if the Contact project were already unpublished.
#>
param(
    [string]$SitePath = 'C:\Program Files\Acumatica ERP\PK9',
    [string]$ProjectName = 'ProjectTask',
    [switch]$IncludeRuntimeCode,
    # App_RuntimeCode file base names to leave out (e.g. files of a project that is about to be unpublished).
    [string[]]$ExcludeRuntimeFile = @()
)
$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
$bin  = Join-Path $SitePath 'Bin'
$rt   = Join-Path $SitePath 'App_RuntimeCode'
$csc  = Join-Path $bin 'roslyn\csc.exe'
$fw   = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319'

$sources = @(Get-ChildItem (Join-Path $repo 'src\Code') -Filter *.cs -Recurse | ForEach-Object FullName)
$excluded = @()
if ($IncludeRuntimeCode) {
    $ownRuntime = foreach ($s in $sources) {
        $b = [IO.Path]::GetFileNameWithoutExtension($s)
        if (Test-Path (Join-Path $rt "$ProjectName.$b.cs")) { "$ProjectName.$b" } else { $b }
    }
    foreach ($f in Get-ChildItem $rt -Filter *.cs) {
        if ($ownRuntime -contains $f.BaseName -or $ExcludeRuntimeFile -contains $f.BaseName) { $excluded += $f.Name }
        else { $sources += $f.FullName }
    }
}

$refs = Get-ChildItem $bin -Filter *.dll | Where-Object {
    try { [void][Reflection.AssemblyName]::GetAssemblyName($_.FullName); $true } catch { $false }
} | ForEach-Object { "/r:`"$($_.FullName)`"" }
$fwRefs = 'netstandard.dll','System.dll','System.Core.dll','System.Data.dll','System.Xml.dll','System.Web.dll','System.ComponentModel.DataAnnotations.dll','System.Runtime.Serialization.dll' |
    ForEach-Object { "/r:`"$fw\$_`"" }
$facades = Get-ChildItem "$fw\..\..\..\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\Facades" -Filter *.dll -ErrorAction SilentlyContinue |
    ForEach-Object { "/r:`"$($_.FullName)`"" }

$out = Join-Path $env:TEMP 'ProjectTask_CompileCheck.dll'
$rsp = Join-Path $env:TEMP 'ProjectTask_CompileCheck.rsp'
@('/nologo', '/target:library', '/nostdlib-', '/langversion:latest', '/nowarn:1701,1702,0618,1591',
  "/out:`"$out`"") + $fwRefs + $facades + $refs + ($sources | ForEach-Object { "`"$_`"" }) |
    Set-Content -Path $rsp -Encoding ascii

if ($excluded) { Write-Host "Left out of App_RuntimeCode: $($excluded -join ', ')" }
Write-Host "Compiling $($sources.Count) file(s) against $bin ..."
& $csc "@$rsp"
if ($LASTEXITCODE -eq 0) { Write-Host 'COMPILE OK' -ForegroundColor Green } else { Write-Host 'COMPILE FAILED' -ForegroundColor Red }
exit $LASTEXITCODE
