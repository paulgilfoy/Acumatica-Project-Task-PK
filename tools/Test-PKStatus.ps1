<#
.SYNOPSIS
  Test for the T-25 "PK Status" (one status per project = the status of the project's own department). Ends with
  "PK STATUS TEST PASS" (exit 0) or "PK STATUS TEST FAIL" (exit 1). Read-only: no database writes, nothing published.

.DESCRIPTION
  [1] Static checks:
      - src\Code\PKDepartmentStatus.cs has no "using PX..." (stays standalone, compiled below).
      - The department -> status field mapping of PKDepartmentStatus.StatusFieldByDepartment equals
        (a) the [PKProjectStatusList("<dept>")] attribute on that field in src\Code\Contract.cs and
        (b) the CASE list in src\Sql\PKProjectStatusBackfill.sql (the SQL copy cannot call C#).
      - Contract.cs declares UsrPKStatusID with [PKDepartmentStatus(...)] and UsrPKStatus as PXDBString(N) with N equal to
        PKProjectStatus.Description (PXDBString in PKProjectStatus.cs and nvarchar in Sql\PKProjectStatus.sql);
        Tables\Contract.xml declares both columns (string MaxLength N); both fields are Enabled = false.
      - PKDepartmentStatusAttribute and PKProjectStatusMaint use PKDepartmentStatus (one mapping, no copies).
  [2] Compiles src\Code\PKDepartmentStatus.cs with a console harness (PK9\Bin\roslyn\csc.exe, no Acumatica references) and
      RUNS the real Resolve() over a table of cases (departments in any case/with spaces, ADMIN/TECH/blank/null, empty status,
      unknown status ID, status of another department set, inactive status text). Harness + exe in %TEMP%\PKStatusTest\.
  [3] ScriptDom parse of the backfill script and of the dynamic UPDATE inside it (parse only, nothing runs).
  [4] Read-only preview on PK9 (..\Acumatica-PK-TimeClock\tools\Query-PK9.ps1, SELECT only): the FROM/JOIN/WHERE of the
      backfill turned into SELECTs: rows the backfill would write, and the resulting PK Status per department.
      Before the publish the two new columns do not exist yet, so they are read as NULL. Skipped with -SkipDatabase.

.EXAMPLE
  .\tools\Test-PKStatus.ps1
  .\tools\Test-PKStatus.ps1 -SkipDatabase
#>
param(
    [string]$SitePath = 'C:\Program Files\Acumatica ERP\PK9',
    [int]$CompanyID = 2,
    [switch]$SkipDatabase,
    [string]$ScriptDomDll = 'C:\Program Files\Microsoft SQL Server Management Studio 22\Release\Common7\IDE\Extensions\Application\Microsoft.SqlServer.TransactSql.ScriptDom.dll'
)
$ErrorActionPreference = 'Stop'
$repo      = Split-Path $PSScriptRoot -Parent
$code      = Join-Path $repo 'src\Code'
$helper    = Join-Path $code 'PKDepartmentStatus.cs'
$sqlFile   = Join-Path $repo 'src\Sql\PKProjectStatusBackfill.sql'
$seedFile  = Join-Path $repo 'src\Sql\PKProjectStatus.sql'
$tableFile = Join-Path $repo 'src\Tables\Contract.xml'
$query     = Join-Path (Split-Path $repo -Parent) 'Acumatica-PK-TimeClock\tools\Query-PK9.ps1'
$csc       = Join-Path $SitePath 'Bin\roslyn\csc.exe'
$fw        = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319'
$work      = Join-Path $env:TEMP 'PKStatusTest'
New-Item -ItemType Directory -Force $work | Out-Null
$script:fail = 0
function Ok([string]$m)   { Write-Host "  ok    $m" }
function Fail([string]$m) { $script:fail++; Write-Host "  FAIL  $m" -ForegroundColor Red }
function Read-Text([string]$p) { [IO.File]::ReadAllText($p).TrimStart([char]0xFEFF) }

$helperText   = Read-Text $helper
$contractText = Read-Text (Join-Path $code 'Contract.cs')
$sqlText      = Read-Text $sqlFile

# ---- [1] Static checks ------------------------------------------------------------------------------------------------
Write-Host '[1] Static checks'
if ($helperText -match '(?m)^\s*using\s+PX') { Fail 'PKDepartmentStatus.cs references PX namespaces (must stay standalone)' } else { Ok 'PKDepartmentStatus.cs has no Acumatica usings' }

$map = [ordered]@{}
foreach ($m in [regex]::Matches($helperText, '\{\s*"(?<d>[^"]+)"\s*,\s*"(?<f>[^"]+)"\s*\}')) { $map[$m.Groups['d'].Value] = $m.Groups['f'].Value }
if ($map.Count -ne 4) { Fail "expected 4 department entries in StatusFieldByDepartment, found $($map.Count)" } else { Ok "C# mapping: $(($map.GetEnumerator() | ForEach-Object { "$($_.Key)->$($_.Value)" }) -join ', ')" }

foreach ($d in $map.Keys) {
    $f = $map[$d]
    $a = [regex]::Match($contractText, "(?s)\[PKProjectStatusList\(\s*""(?<d>[^""]*)""\s*\)\][^\r\n]*\s*(?:\[[^\]]*\]\s*(?://[^\r\n]*)?\s*)*public\s+virtual\s+int\?\s+$f\b")
    if (-not $a.Success) { Fail "Contract.cs: no [PKProjectStatusList(...)] on $f" }
    elseif ($a.Groups['d'].Value -cne $d) { Fail "Contract.cs: $f has PKProjectStatusList(`"$($a.Groups['d'].Value)`"), C# mapping says $d" }
    else { Ok "Contract.cs: $f carries PKProjectStatusList(`"$d`")" }
}

$sqlMap = [ordered]@{}
foreach ($m in [regex]::Matches($sqlText, "WHEN\s+''(?<d>[^']+)''\s+THEN\s+c\.(?<f>\w+)")) { $sqlMap[$m.Groups['d'].Value] = $m.Groups['f'].Value }
$same = ($sqlMap.Count -eq $map.Count) -and -not @($map.Keys | Where-Object { $sqlMap[$_] -cne $map[$_] }).Count
if ($same) { Ok 'Sql\PKProjectStatusBackfill.sql CASE list = C# mapping' } else { Fail "backfill CASE list differs: $(($sqlMap.GetEnumerator() | ForEach-Object { "$($_.Key)->$($_.Value)" }) -join ', ')" }

$descLen = [regex]::Match((Read-Text (Join-Path $code 'PKProjectStatus.cs')), '(?s)\[PXDBString\((?<n>\d+)[^\]]*\]\s*(?:\[[^\]]*\]\s*)*public\s+virtual\s+string\s+Description\b')
$seedLen = [regex]::Match((Read-Text $seedFile), '\[Description\]\s*\[nvarchar\]\((?<n>\d+)\)')
$statLen = [regex]::Match($contractText, '(?s)\[PXDBString\((?<n>\d+)\s*,\s*IsUnicode\s*=\s*true\)\][^\r\n]*\s*(?:\[[^\]]*\]\s*)*public\s+virtual\s+string\s+UsrPKStatus\b')
[xml]$tbl = Read-Text $tableFile
$colText = $tbl.Table.Column | Where-Object ColumnName -eq 'UsrPKStatus'
$colID   = $tbl.Table.Column | Where-Object ColumnName -eq 'UsrPKStatusID'
if (-not $descLen.Success -or -not $seedLen.Success) { Fail 'cannot read the PKProjectStatus.Description length' }
elseif (-not $statLen.Success) { Fail 'Contract.cs: UsrPKStatus is not a PXDBString(N, IsUnicode = true)' }
elseif ($statLen.Groups['n'].Value -ne $descLen.Groups['n'].Value -or $seedLen.Groups['n'].Value -ne $descLen.Groups['n'].Value) {
    Fail "lengths differ: UsrPKStatus $($statLen.Groups['n'].Value), PKProjectStatus.Description DAC $($descLen.Groups['n'].Value) / SQL $($seedLen.Groups['n'].Value)" }
else { Ok "UsrPKStatus length $($statLen.Groups['n'].Value) = PKProjectStatus.Description" }
if (-not $colText -or $colText.ColumnType -ne 'string' -or $colText.MaxLength -ne $descLen.Groups['n'].Value -or $colText.IsNewColumn -ne 'True') { Fail 'Tables\Contract.xml: UsrPKStatus column missing or not string(N), IsNewColumn' } else { Ok "Tables\Contract.xml: UsrPKStatus string($($colText.MaxLength))" }
if (-not $colID -or $colID.ColumnType -ne 'int' -or $colID.IsNewColumn -ne 'True') { Fail 'Tables\Contract.xml: UsrPKStatusID column missing or not int' } else { Ok 'Tables\Contract.xml: UsrPKStatusID int' }
if ($contractText -notmatch '(?s)\[PKDepartmentStatus\(typeof\(usrPGDepartment\),\s*typeof\(usrPKStatus\)\)\]\s*\[PXUIField\([^\]]*Enabled\s*=\s*false[^\]]*\)\]\s*public\s+virtual\s+int\?\s+UsrPKStatusID\b') { Fail 'Contract.cs: UsrPKStatusID lacks [PKDepartmentStatus(typeof(usrPGDepartment), typeof(usrPKStatus))] + [PXUIField(... Enabled = false)]' } else { Ok 'Contract.cs: UsrPKStatusID computed by PKDepartmentStatus, read-only' }
if ($contractText -notmatch '(?s)\[PXUIField\([^\]]*Enabled\s*=\s*false[^\]]*\)\]\s*public\s+virtual\s+string\s+UsrPKStatus\b') { Fail 'Contract.cs: UsrPKStatus is not Enabled = false' } else { Ok 'Contract.cs: UsrPKStatus read-only' }
foreach ($pair in @(@('PKDepartmentStatusAttribute.cs', 'PKDepartmentStatus\.Resolve\('), @('PKProjectStatusMaint.cs', 'PKDepartmentStatus\.StatusFieldOf\('))) {
    if ((Read-Text (Join-Path $code $pair[0])) -match $pair[1]) { Ok "$($pair[0]) uses $($pair[1] -replace '\\','')" } else { Fail "$($pair[0]) does not call $($pair[1] -replace '\\','')" }
}

# ---- [2] Compile and run the real mapping ------------------------------------------------------------------------------
# name | department ('-' = null) | TAX | ACCT | AUDIT | 401K status ('-' = null) | expected ID | expected text ('-' = null)
$cases = @(
    @('TAX project',                              'TAX',    '14', '-', '-', '-',  '14', 'Complete (Tax)'),
    @('ACCT project',                             'ACCT',   '-', '2', '-', '-',   '2',  'Initial Review (Acctg)'),
    @('AUDIT project',                            'AUDIT',  '-', '-', '11', '-',  '11', 'Complete (Audit)'),
    @('401K project',                             '401K',   '-', '-', '-', '20',  '20', 'Complete (401k)'),
    @('Lower-case department',                    'tax',    '3', '-', '-', '-',   '3',  'Information Requested (from Client)'),
    @('Department with spaces',                   ' ACCT ', '-', '7', '-', '-',   '7',  'Complete (Acct)'),
    @('401k lower-case k',                        '401k',   '-', '-', '-', '5',   '5',  'Initial RECS and Dis'),
    @('Status of ANOTHER department ignored',     'AUDIT',  '1', '1', '2', '-',   '2',  'In Preparation (Audit)'),
    @('Own status empty, others set -> empty',    'TAX',    '-', '7', '11', '20', '-',  '-'),
    @('ADMIN department -> empty',                'ADMIN',  '14', '7', '11', '20', '-', '-'),
    @('TECH department -> empty',                 'TECH',   '14', '-', '-', '-',  '-',  '-'),
    @('No department -> empty',                   '-',      '14', '7', '11', '20', '-', '-'),
    @('Blank department -> empty',                '   ',    '14', '-', '-', '-',  '-',  '-'),
    @('Unknown status ID keeps the ID, no text',  'TAX',    '99', '-', '-', '-',  '99', '-'),
    @('Inactive status still gets its text',      'ACCT',   '-', '9', '-', '-',   '9',  'Retired Acct Status'),
    @('Status 0 (no such status) keeps the ID',   'ACCT',   '-', '0', '-', '-',   '0',  '-')
)
$casesFile = Join-Path $work 'cases.tsv'
($cases | ForEach-Object { $_ -join "`t" }) | Set-Content -Path $casesFile -Encoding utf8

$harness = @'
using System;
using System.Collections.Generic;
using System.IO;
using ProjectTask;

internal static class PKStatusHarness
{
    // Stand-in for the cached PKProjectStatus list (PKProjectStatusListAttribute.GetStatuses returns active AND inactive rows).
    private static readonly Dictionary<string, string> Texts = new Dictionary<string, string>
    {
        { "TAX|14", "Complete (Tax)" }, { "TAX|3", "Information Requested (from Client)" },
        { "ACCT|2", "Initial Review (Acctg)" }, { "ACCT|7", "Complete (Acct)" }, { "ACCT|9", "Retired Acct Status" },
        { "AUDIT|11", "Complete (Audit)" }, { "AUDIT|2", "In Preparation (Audit)" },
        { "401K|20", "Complete (401k)" }, { "401K|5", "Initial RECS and Dis" },
    };
    private static int? I(string s) { s = s.Trim(); return s == "-" ? (int?)null : int.Parse(s); }
    private static string S(string s) { return s == "-" ? null : s; }
    private static string Show(object o) { return o == null ? "-" : o.ToString(); }

    private static int Main(string[] args)
    {
        Console.WriteLine("Mapping: " + string.Join(", ", new List<string>(Map())));
        int failed = 0, n = 0;
        foreach (string line in File.ReadAllLines(args[0]))
        {
            string l = line.TrimStart('\uFEFF');
            if (l.Trim().Length == 0) continue;
            string[] c = l.Split('\t');
            n++;
            var row = new Dictionary<string, int?>
            {
                { "UsrPGTaxStatus", I(c[2]) }, { "UsrPGAcctStatus", I(c[3]) }, { "UsrPGAuditStatus", I(c[4]) }, { "Usr401kStatus", I(c[5]) },
            };
            var describedWith = new List<string>();
            int? id; string text; string error = null;
            try
            {
                PKDepartmentStatus.Resolve(S(c[1]), f => row[f], (d, s) =>
                {
                    describedWith.Add(d);
                    string t; return Texts.TryGetValue(d + "|" + s, out t) ? t : null;
                }, out id, out text);
            }
            catch (Exception ex) { id = null; text = null; error = ex.GetType().Name + ": " + ex.Message; }
            bool ok = error == null && Show(id) == c[6] && Show(text) == c[7];
            // describe must get the canonical department key (as stored in PKProjectStatus), never the raw row value
            foreach (string d in describedWith) if (!PKDepartmentStatus.StatusFieldByDepartment.ContainsKey(d) || d != PKDepartmentStatus.CanonicalDepartment(d)) { ok = false; error = "describe got '" + d + "'"; }
            if (!ok) failed++;
            Console.WriteLine(string.Format("{0,-4} {1,-3} {2,-42} dept={3,-8} -> id={4,-3} text={5}{6}",
                ok ? "PASS" : "FAIL", n, c[0], "'" + c[1] + "'", Show(id), Show(text),
                ok ? "" : "   (expected id=" + c[6] + " text=" + c[7] + (error != null ? "; " + error : "") + ")"));
        }
        Console.WriteLine(string.Format("Cases: {0}, passed {1}, failed {2}", n, n - failed, failed));
        return failed == 0 ? 0 : 1;
    }

    private static IEnumerable<string> Map()
    {
        foreach (var kv in PKDepartmentStatus.StatusFieldByDepartment) yield return kv.Key + "->" + kv.Value;
    }
}
'@
$harnessFile = Join-Path $work 'PKStatusHarness.cs'
Set-Content -Path $harnessFile -Value $harness -Encoding utf8
$exe = Join-Path $work 'PKStatusTest.exe'
if (Test-Path $exe) { Remove-Item $exe -Force -Confirm:$false }
Write-Host "`n[2] Compiling $helper + harness ..."
& $csc /nologo /target:exe /langversion:latest /nostdlib- "/r:$fw\System.dll" "/r:$fw\System.Core.dll" "/out:$exe" $helper $harnessFile
if ($LASTEXITCODE -ne 0 -or -not (Test-Path $exe)) { Fail 'harness compile failed' }
else {
    Write-Host "Running $($cases.Count) case(s)"
    & $exe $casesFile
    if ($LASTEXITCODE -ne 0) { $script:fail++ }
}

# ---- [3] T-SQL parse --------------------------------------------------------------------------------------------------
Write-Host "`n[3] T-SQL syntax (ScriptDom TSql160Parser, parse only)"
$inner = [regex]::Match($sqlText, "(?s)EXEC\s+sp_executesql\s+N'(?<q>(?:[^']|'')*)'")
if (-not $inner.Success) { Fail 'no EXEC sp_executesql N''...'' block found' }
$innerSql = $inner.Groups['q'].Value -replace "''", "'"
if (Test-Path $ScriptDomDll) {
    Add-Type -Path $ScriptDomDll
    foreach ($pair in @(@('whole script', $sqlText), @('dynamic UPDATE', $innerSql))) {
        $parser = New-Object Microsoft.SqlServer.TransactSql.ScriptDom.TSql160Parser($true)
        $errs = $null
        [void]$parser.Parse((New-Object IO.StringReader($pair[1])), [ref]$errs)
        if ($errs.Count) { foreach ($e in $errs) { Fail "$($pair[0]) line $($e.Line) col $($e.Column): $($e.Message)" } } else { Ok "$($pair[0]) parses without errors" }
    }
} else { Write-Host "  skip  ScriptDom DLL not found: $ScriptDomDll" -ForegroundColor Yellow }

# ---- [4] Read-only preview on PK9 -------------------------------------------------------------------------------------
if ($SkipDatabase) { Write-Host "`n[4] skipped (-SkipDatabase)" }
elseif (-not (Test-Path $query)) { Write-Host "`n[4] skip: $query not found" -ForegroundColor Yellow }
else {
    Write-Host "`n[4] Read-only preview on PK9 (SELECTs built from the backfill's FROM/JOIN/WHERE; nothing is written)"
    $from = [regex]::Match($innerSql, '(?s)(?<f>FROM\s+dbo\.Contract\s+c.*)$')
    if (-not $from.Success) { Fail 'cannot find FROM dbo.Contract c in the dynamic UPDATE' }
    else {
        $fromSql = $from.Groups['f'].Value.Trim()
        $cols = @(& $query "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Contract' AND COLUMN_NAME IN ('UsrPKStatusID','UsrPKStatus')" 6>$null | Out-String)
        $published = ($cols -join '') -match 'UsrPKStatusID' -and ($cols -join '') -match 'UsrPKStatus\b'
        if (-not $published) {
            Write-Host '  info  columns UsrPKStatusID/UsrPKStatus do not exist yet (not published): read as NULL'
            $fromSql = $fromSql -replace 'c\.UsrPKStatusID', 'CAST(NULL AS int)' -replace 'c\.UsrPKStatus\b', 'CAST(NULL AS nvarchar(60))'
        } else { Write-Host '  info  columns exist (published): rows to write = drift (expected 0 after the backfill ran)' }
        $whereAll = $fromSql -replace '(?s)WHERE\s+c\.CompanyID\s*>\s*0.*$', "WHERE c.CompanyID = $CompanyID"
        $toWrite  = $fromSql -replace 'WHERE\s+c\.CompanyID\s*>\s*0', "WHERE c.CompanyID = $CompanyID"
        Write-Host "  Rows the backfill would write (CompanyID $CompanyID):"
        & $query "SELECT COUNT(*) AS RowsToWrite $toWrite" | Out-String -Width 200 | Write-Host
        Write-Host "  Resulting PK Status per department (CompanyID $CompanyID, all Contract rows incl. templates):"
        & $query @"
SELECT ISNULL(UPPER(LTRIM(RTRIM(c.UsrPGDepartment))), '(none)') AS Dept,
       COUNT(*) AS Rows,
       SUM(CASE WHEN v.PKStatusID IS NOT NULL THEN 1 ELSE 0 END) AS WithStatus,
       SUM(CASE WHEN v.PKStatusID IS NULL THEN 1 ELSE 0 END) AS StaysNull,
       SUM(CASE WHEN v.PKStatusID IS NOT NULL AND s.Description IS NULL THEN 1 ELSE 0 END) AS IdWithoutText,
       SUM(CASE WHEN s.Active = 0 THEN 1 ELSE 0 END) AS InactiveStatus
$whereAll
GROUP BY ISNULL(UPPER(LTRIM(RTRIM(c.UsrPGDepartment))), '(none)')
ORDER BY Dept
"@ | Out-String -Width 200 | Write-Host
    }
}

if ($script:fail -eq 0) { Write-Host 'PK STATUS TEST PASS' -ForegroundColor Green; exit 0 }
Write-Host "PK STATUS TEST FAIL ($script:fail)" -ForegroundColor Red
exit 1
