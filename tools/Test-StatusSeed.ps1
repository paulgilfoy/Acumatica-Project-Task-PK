<#
.SYNOPSIS
  READ-ONLY seed-diff test for src\Sql\PKProjectStatus.sql (T-21). Prints "SEED TEST PASS" (exit 0) or "SEED TEST FAIL" (exit 1).

.DESCRIPTION
  Parses the seed VALUES rows (N'DEPT', ID, N'Text') and compares them with:
    0. (T-22) src\Code\Contract.cs: each of the 4 fields carries [PKProjectStatusList("<dept>")] with the department
       the seed uses (or, before T-22, still its PXIntList).
    1. the four PXIntList arrays of ContractExt (Usr401kStatus=401K, UsrPGTaxStatus=TAX, UsrPGAcctStatus=ACCT,
       UsrPGAuditStatus=AUDIT): the frozen pre-T-22 lists. Source: -ContractFile, else src\Code\Contract.cs while it
       still has them, else the published copy PK9\App_RuntimeCode\Contract.cs (until the bundle B publish), else the
       git history of src\Code\Contract.cs (last commit with the lists). Expected: IDENTICAL
       (same ints, same text byte for byte, incl. the 401K 17 double space, D3).
    2. the live CSAttributeDetail rows of STATUS401K/STATUSTAX/STATUSACCT/STATUSAUDI (PK9, -CompanyID, read-only SQL via
       ..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1). Expected: equal except the APPROVED deviations
       D1 (ACCT 02, AUDIT 01, 401K 05: PXIntList text wins) and D2 (ACCT 00 "Not Started" not seeded).
       After the first PKPM2001 save of a department its CSAttributeDetail rows equal the table (sync), so a D1 row
       that already matches is reported as "D1 already synced" and still passes.
  Also: ScriptDom syntax parse of the whole script (no execution; SSMS 22 DLL), a read-only bind check of the seed SELECT
  (companies x VALUES, without the INSERT and without the NOT EXISTS on the not-yet-created table), and, if the table
  PKProjectStatus already exists (after publish), a comparison of its rows with the seed (54 rows per seeded company).
  Never writes to the database and never runs the script.

.EXAMPLE
  .\tools\Test-StatusSeed.ps1
  .\tools\Test-StatusSeed.ps1 -ContractFile 'C:\Program Files\Acumatica ERP\PK9\App_RuntimeCode\Contract.cs'
#>
param(
    [string]$SeedFile,
    [string]$ContractFile,
    [int]$CompanyID = 2,
    [string]$SitePath = 'C:\Program Files\Acumatica ERP\PK9',
    [string]$ScriptDomDll = 'C:\Program Files\Microsoft SQL Server Management Studio 22\Release\Common7\IDE\Extensions\Application\Microsoft.SqlServer.TransactSql.ScriptDom.dll'
)
$ErrorActionPreference = 'Stop'
$repo  = Split-Path $PSScriptRoot -Parent
$query = Join-Path (Split-Path $repo -Parent) 'Acumatica-PK-TimeClock\tools\Query-PK9.ps1'
if (-not $SeedFile) { $SeedFile = Join-Path $repo 'src\Sql\PKProjectStatus.sql' }
$fail = 0
function Fail([string]$msg) { $script:fail++; Write-Host "  FAIL  $msg" -ForegroundColor Red }
function Ok([string]$msg)   { Write-Host "  ok    $msg" }
function Note([string]$msg) { Write-Host "  dev   $msg" -ForegroundColor Yellow }
function Show([string]$s)   { if ($null -eq $s) { '<none>' } else { '"' + $s + '"' } }
function Invoke-ReadOnly([string]$sql) {
    $csv = Join-Path $env:TEMP ("PKStatusSeed_{0}.csv" -f [guid]::NewGuid().ToString('N'))
    try {
        & $query $sql -OutCsv $csv 6>$null | Out-Null
        if (Test-Path $csv) { @(Import-Csv $csv) } else { @() }
    } finally { Remove-Item $csv -Force -ErrorAction SilentlyContinue -Confirm:$false }
}

$Departments = [ordered]@{ '401K' = 'Usr401kStatus'; 'TAX' = 'UsrPGTaxStatus'; 'ACCT' = 'UsrPGAcctStatus'; 'AUDIT' = 'UsrPGAuditStatus' }
$Attributes  = @{ '401K' = 'STATUS401K'; 'TAX' = 'STATUSTAX'; 'ACCT' = 'STATUSACCT'; 'AUDIT' = 'STATUSAUDI' }
# Approved deviations from CSAttributeDetail (docs/status-design.md D1, D2).
$D1 = @{ 'ACCT|2' = 'D1'; 'AUDIT|1' = 'D1 (case)'; '401K|5' = 'D1 (case)' }
$D2 = @{ 'ACCT|0' = 'D2 (ACCT 0 "Not Started" not seeded)' }

# ---- 1. Parse the seed ----
Write-Host "Seed: $SeedFile"
$sqlText = [IO.File]::ReadAllText($SeedFile)
$rowRe = "\(\s*N'(?<d>[^']*)'\s*,\s*(?<id>-?\d+)\s*,\s*N'(?<t>(?:[^']|'')*)'\s*\)"
$seed = [ordered]@{}
foreach ($m in [regex]::Matches($sqlText, $rowRe)) {
    $k = "$($m.Groups['d'].Value)|$([int]$m.Groups['id'].Value)"
    if ($seed.Contains($k)) { Fail "duplicate seed row $k" }
    $seed[$k] = $m.Groups['t'].Value -replace "''", "'"
}
$counts = ($Departments.Keys | ForEach-Object { $d = $_; "$d $(@($seed.Keys | Where-Object { $_ -like "$d|*" }).Count)" }) -join ', '
Write-Host "Seed rows: $($seed.Count) ($counts)"
if ($seed.Count -ne 54) { Fail "expected 54 seed rows, found $($seed.Count)" } else { Ok '54 seed rows' }
foreach ($k in $seed.Keys) {
    if ($Departments.Keys -notcontains $k.Split('|')[0]) { Fail "seed row $k has an unknown department" }
    if ([int]$k.Split('|')[1] -le 0) { Fail "seed row $k has StatusID <= 0" }
    if ($seed[$k].Length -gt 60) { Fail "seed row $k text longer than 60" }
}
foreach ($d in $Departments.Keys) {   # case-insensitive uniqueness (the screen rule)
    $dups = @($seed.Keys | Where-Object { $_ -like "$d|*" } | ForEach-Object { $seed[$_].Trim().ToLowerInvariant() } | Group-Object | Where-Object Count -gt 1)
    foreach ($g in $dups) { Fail "department $d has duplicate description '$($g.Name)'" }
}

# ---- 2. Seed vs PXIntList (Contract.cs) ----
function Get-IntLists([string]$path, [string]$text) {
    if ([string]::IsNullOrEmpty($text)) { $text = [IO.File]::ReadAllText($path) }   # [string] turns $null into ''
    $lists = @{}
    foreach ($d in $Departments.Keys) {
        $f = $Departments[$d]
        $m = [regex]::Match($text, "(?s)\[PXIntList\(\s*new\s+int\[\]\s*\{(?<ids>[^}]*)\}\s*,\s*new\s+string\[\]\s*\{(?<txt>[^}]*)\}\s*\)\]\s*(?:\[[^\]]*\]\s*)*public\s+virtual\s+int\?\s+$f\b")
        if (-not $m.Success) { return $null }
        $ids = @($m.Groups['ids'].Value -split ',' | ForEach-Object { [int]$_.Trim() })
        $txt = @([regex]::Matches($m.Groups['txt'].Value, '"((?:[^"\\]|\\.)*)"') | ForEach-Object { $_.Groups[1].Value })
        if ($ids.Count -ne $txt.Count) { throw "$path : $f has $($ids.Count) ints but $($txt.Count) labels" }
        $lists[$d] = for ($i = 0; $i -lt $ids.Count; $i++) { [pscustomobject]@{ ID = $ids[$i]; Text = $txt[$i] } }
    }
    $lists
}
# [0] (T-22) src\Code\Contract.cs must carry [PKProjectStatusList("<dept>")] on each field, with a department the seed uses
#     (a misspelt department would load an empty list: numbers on PM301000, no validation).
$srcContract = Join-Path $repo 'src\Code\Contract.cs'
$srcText = [IO.File]::ReadAllText($srcContract)
$srcHasLists = [bool](Get-IntLists $srcContract $srcText)
Write-Host "`n[0] List attribute on the 4 fields in src\Code\Contract.cs"
foreach ($d in $Departments.Keys) {
    $f = $Departments[$d]
    $a =[regex]::Match($srcText, "(?s)\[PKProjectStatusList\(\s*""(?<d>[^""]*)""\s*\)\][^\r\n]*\s*(?:\[[^\]]*\]\s*(?://[^\r\n]*)?\s*)*public\s+virtual\s+int\?\s+$f\b")
    if ($a.Success) {
        if ($a.Groups['d'].Value -cne $d) { Fail "$f has PKProjectStatusList(`"$($a.Groups['d'].Value)`"), expected `"$d`"" }
        elseif (-not @($seed.Keys | Where-Object { $_ -like "$d|*" }).Count) { Fail "$f department `"$d`" has no seed rows" }
        else { Ok "$f -> PKProjectStatusList(`"$d`") ($(@($seed.Keys | Where-Object { $_ -like "$d|*" }).Count) seed rows)" }
    }
    elseif ($srcHasLists) { Write-Host "  info  $f still uses the PXIntList (before T-22)" }
    else { Fail "$f has neither PKProjectStatusList(`"$d`") nor a PXIntList" }
}

# [1] baseline = the old PXIntLists. Sources in order: -ContractFile; src (before T-22); the published PK9 copy (until the
#     bundle B publish); git history of src\Code\Contract.cs (last commit that still had the lists; always available).
$baseText = $null
if (-not $ContractFile -and -not $srcHasLists) {
    Write-Host "`nsrc\Code\Contract.cs has no PXIntList for the 4 fields (T-22 swapped them); looking for the frozen pre-T-22 lists." -ForegroundColor Yellow
    $rt = Join-Path $SitePath 'App_RuntimeCode\Contract.cs'
    if ((Test-Path $rt) -and (Get-IntLists $rt)) { $ContractFile = $rt }
    else {
        $git = @(Get-Command git -ErrorAction SilentlyContinue | ForEach-Object Source) + @(Get-ChildItem "$env:LOCALAPPDATA\GitHubDesktop\app-*\resources\app\git\cmd\git.exe" -ErrorAction SilentlyContinue | Sort-Object FullName -Descending | ForEach-Object FullName) | Select-Object -First 1
        if ($git) {
            foreach ($h in @(& $git -C $repo log --format=%H -- src/Code/Contract.cs)) {
                $t = (& $git -C $repo show "${h}:src/Code/Contract.cs") -join "`n"
                if ($t -and (Get-IntLists "git $h" $t)) { $ContractFile = "git $($h.Substring(0,7)):src/Code/Contract.cs"; $baseText = $t; break }
            }
        }
    }
}
elseif (-not $ContractFile) { $ContractFile = $srcContract }
Write-Host "`n[1] Seed vs PXIntList in $(if ($ContractFile) { $ContractFile } else { '<no source found>' }) (expected: identical)"
$lists = if ($baseText) { Get-IntLists $ContractFile $baseText } elseif ($ContractFile) { Get-IntLists $ContractFile } else { $null }
if (-not $lists) { Fail "no PXIntList for all 4 fields in $(if ($ContractFile) { $ContractFile } else { 'src, the PK9 runtime copy or git history' })" }
else {
    $exact = 0
    foreach ($d in $Departments.Keys) {
        foreach ($e in $lists[$d]) {
            $k = "$d|$($e.ID)"
            if (-not $seed.Contains($k)) { Fail "$k `"$($e.Text)`" is in the PXIntList but not in the seed" }
            elseif ($seed[$k] -cne $e.Text) { Fail "$k text differs: seed $(Show $seed[$k]) vs PXIntList $(Show $e.Text)" }
            else { $exact++ }
        }
        $extra = @($seed.Keys | Where-Object { $_ -like "$d|*" } | Where-Object { $id = [int]$_.Split('|')[1]; -not ($lists[$d] | Where-Object ID -eq $id) })
        foreach ($k in $extra) { Fail "$k is in the seed but not in the PXIntList" }
    }
    Ok "$exact of $($seed.Count) seed rows equal the PXIntList exactly (int and text, case and spaces)"
}

# ---- 3. Seed vs live CSAttributeDetail ----
Write-Host "`n[2] Seed vs live CSAttributeDetail (CompanyID $CompanyID; expected: equal except D1/D2)"
$attrRows = @(Invoke-ReadOnly @"
SELECT AttributeID, ValueID, Description FROM CSAttributeDetail
WHERE CompanyID = $CompanyID AND AttributeID IN ('STATUS401K','STATUSTAX','STATUSACCT','STATUSAUDI')
ORDER BY AttributeID, ValueID
"@)
$byAttr = @{}; foreach ($a in $Attributes.Keys) { $byAttr[$Attributes[$a]] = $a }
$live = @{}
foreach ($r in $attrRows) { $live["$($byAttr[$r.AttributeID])|$([int]$r.ValueID)"] = [pscustomobject]@{ ValueID = $r.ValueID; Text = $r.Description } }
Write-Host "Live CSAttributeDetail rows: $($attrRows.Count)"
# Rows the user added or renamed on PKPM2001 are synced into CSAttributeDetail: accept them when the table holds the same text.
$tblText = @{}
if ([int](@(Invoke-ReadOnly "SELECT COUNT(*) AS N FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PKProjectStatus'")[0].N) -gt 0) {
    foreach ($t in @(Invoke-ReadOnly "SELECT DepartmentID, StatusID, Description FROM PKProjectStatus WHERE CompanyID = $CompanyID")) {
        $tblText["$("$($t.DepartmentID)".Trim())|$([int]$t.StatusID)"] = $t.Description
    }
}
$same = 0; $devs = @()
foreach ($k in $seed.Keys) {
    $l = $live[$k]
    if (-not $l) { Fail "$k is seeded but has no CSAttributeDetail row (the first PKPM2001 save would insert it; not expected today)"; continue }
    if ($l.ValueID -ne ('{0:00}' -f [int]$k.Split('|')[1])) { Fail "$k ValueID '$($l.ValueID)' is not the 2-digit ID" }
    if ($l.Text -ceq $seed[$k]) {
        $same++
        if ($D1.ContainsKey($k)) { Note "$k $($D1[$k]) already synced: CSAttributeDetail = seed $(Show $seed[$k])" }
    }
    elseif ($D1.ContainsKey($k)) { $devs += "$k $($D1[$k]): seed $(Show $seed[$k]) vs CSAttributeDetail $(Show $l.Text)" }
    elseif ($tblText.ContainsKey($k) -and $tblText[$k] -ceq $l.Text) { Note "$k renamed on PKPM2001 and synced: seed $(Show $seed[$k]) vs table/CSAttributeDetail $(Show $l.Text)" }
    else { Fail "$k unexpected difference: seed $(Show $seed[$k]) vs CSAttributeDetail $(Show $l.Text)" }
}
foreach ($k in $live.Keys | Sort-Object) {
    if ($seed.Contains($k)) { continue }
    if ($D2.ContainsKey($k)) { $devs += "$k $($D2[$k]): CSAttributeDetail $(Show $live[$k].Text) stays, the sync never deletes it" }
    elseif ($tblText.ContainsKey($k) -and $tblText[$k] -ceq $live[$k].Text) { Note "$k added on PKPM2001 and synced: $(Show $live[$k].Text)" }
    else { Fail "$k exists in CSAttributeDetail ($(Show $live[$k].Text)) but not in the seed" }
}
Ok "$same of $($seed.Count) seed rows equal CSAttributeDetail exactly"
foreach ($x in $devs) { Note "intended deviation $x" }

# ---- 4. ScriptDom syntax parse (no execution) ----
Write-Host "`n[3] T-SQL syntax (ScriptDom TSql160Parser, parse only)"
if (Test-Path $ScriptDomDll) {
    Add-Type -Path $ScriptDomDll
    $parser = New-Object Microsoft.SqlServer.TransactSql.ScriptDom.TSql160Parser($true)
    $errs = $null
    $sr = New-Object IO.StringReader($sqlText)
    [void]$parser.Parse($sr, [ref]$errs)
    if ($errs.Count) { foreach ($e in $errs) { Fail "line $($e.Line) col $($e.Column): $($e.Message)" } }
    else { Ok 'script parses without errors' }
} else { Write-Host "  skip  ScriptDom DLL not found: $ScriptDomDll" -ForegroundColor Yellow }

# ---- 5. Read-only bind check of the seed SELECT ----
Write-Host "`n[4] Bind check: companies x VALUES (read-only SELECT, no INSERT)"
$m = [regex]::Match($sqlText, '(?s)FROM\s+\(SELECT DISTINCT.*?\)\s+v\(\[DepartmentID\],\s*\[StatusID\],\s*\[Description\]\)')
if (-not $m.Success) { Fail 'could not find the companies x VALUES block in the seed' }
else {
    $bind = @(Invoke-ReadOnly ("SELECT c.CompanyID, COUNT(*) AS SeedRows, MAX(LEN(v.Description)) AS MaxLen " + $m.Value + " GROUP BY c.CompanyID ORDER BY c.CompanyID"))
    if (-not $bind.Count) { Fail 'the company query returns no company (no STATUSTAX attribute with CompanyID > 0)' }
    foreach ($b in $bind) {
        if ([int]$b.SeedRows -ne 54) { Fail "company $($b.CompanyID): $($b.SeedRows) rows" } else { Ok "company $($b.CompanyID) would get 54 rows (max text length $($b.MaxLen))" }
    }
}

# ---- 6. After publish: the table vs the seed ----
Write-Host "`n[5] Table PKProjectStatus on PK9"
$exists = @(Invoke-ReadOnly "SELECT COUNT(*) AS N FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PKProjectStatus'")
if ([int]$exists[0].N -eq 0) { Write-Host '  skip  table does not exist yet (expected before the bundle B publish)' }
else {
    $tbl = @(Invoke-ReadOnly "SELECT DepartmentID, StatusID, Description, SortOrder, Active, CreatedByScreenID FROM PKProjectStatus WHERE CompanyID = $CompanyID")
    Write-Host "  rows: $($tbl.Count) ($((($tbl | Group-Object DepartmentID | ForEach-Object { "$($_.Name) $($_.Count)" }) -join ', ')))"
    foreach ($k in $seed.Keys) {
        $d, $id = $k.Split('|')
        $r = $tbl | Where-Object { $_.DepartmentID -eq $d -and [int]$_.StatusID -eq [int]$id }
        if (-not $r) { Fail "$k missing from the table" }
        elseif ($r.Description -cne $seed[$k] -and $r.CreatedByScreenID.Trim() -eq 'PKPM2001') { Note "$k renamed on PKPM2001 since the seed: $(Show $r.Description)" }
    }
    Ok 'table checked against the seed (renames on PKPM2001 are reported, not failed)'
}

Write-Host ''
if ($fail -eq 0) { Write-Host 'SEED TEST PASS' -ForegroundColor Green; exit 0 }
else { Write-Host "SEED TEST FAIL ($fail problem(s))" -ForegroundColor Red; exit 1 }
