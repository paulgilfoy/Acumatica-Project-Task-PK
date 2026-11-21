<#
.SYNOPSIS
  Unit test for the template due-date rule (T-32): compiles src\Code\PKTemplateDueDate.cs together with a small console
  harness, RUNS it over a table of cases and prints PASS/FAIL per case. Ends with "DATE TEST PASS" (exit 0) or
  "DATE TEST FAIL" (exit 1).

.DESCRIPTION
  PKTemplateDueDate.cs has no Acumatica references, so it compiles on its own with the Roslyn compiler that ships with
  Acumatica (PK9\Bin\roslyn\csc.exe, as tools\Test-Compile.ps1 uses). The harness and the .exe are written to
  %TEMP%\PKTemplateDates\ (throwaway). Read-only: no database, no site files, nothing published.

  Also checks (static): PKTemplateDueDate.cs has no "using PX..." (stays standalone), PKProjectTemplateExt.cs calls
  PKTemplateDueDate.GetDueDate, and PKTemplateDueDate.ActiveRule equals -ExpectRule (the rule the user chose; change the
  default here when the rule is switched, see docs\template-copy.md).

  Case columns: Rule (RollForward | AsIs), Template (task's UsrPGDueDate), Start (project StartDate), Fallback (business
  date passed by PKProjectTemplateExt), Expected. Dates yyyy-MM-dd (optional HH:mm), '-' = null.

.EXAMPLE
  .\tools\Test-TemplateDates.ps1
#>
param(
    [string]$SitePath = 'C:\Program Files\Acumatica ERP\PK9',
    [ValidateSet('RollForward', 'AsIs')][string]$ExpectRule = 'AsIs'
)
$ErrorActionPreference = 'Stop'
$repo    = Split-Path $PSScriptRoot -Parent
$dueFile = Join-Path $repo 'src\Code\PKTemplateDueDate.cs'
$extFile = Join-Path $repo 'src\Code\PKProjectTemplateExt.cs'
$csc     = Join-Path $SitePath 'Bin\roslyn\csc.exe'
$fw      = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319'
$work    = Join-Path $env:TEMP 'PKTemplateDates'
New-Item -ItemType Directory -Force $work | Out-Null
$fail = 0

# ---- Cases --------------------------------------------------------------------------------------------------------
# Realistic data (docs/template-flow.md section 6): task 011065 is always due 3/15, 011040 4/15, 02TXFRA 5/15.
$cases = @(
    # name                                                      rule           template      start              fallback      expected
    @('Template later in the year than the start',              'RollForward', '2025-04-15', '2026-01-10',      '2026-01-10', '2026-04-15'),
    @('Template earlier in the year than the start -> next yr', 'RollForward', '2025-04-15', '2026-11-20',      '2026-11-20', '2027-04-15'),
    @('Template month/day equal to the start (same day)',       'RollForward', '2025-04-15', '2026-04-15',      '2026-04-15', '2026-04-15'),
    @('Start one day after the template month/day',             'RollForward', '2025-04-15', '2026-04-16',      '2026-04-16', '2027-04-15'),
    @('Template year older than the start year',                'RollForward', '2021-05-15', '2026-02-01',      '2026-02-01', '2026-05-15'),
    @('Template year later than the start year',                'RollForward', '2030-05-15', '2026-02-01',      '2026-02-01', '2026-05-15'),
    @('Start in a different year (2027), template 2025',        'RollForward', '2025-03-15', '2027-01-04',      '2026-11-20', '2027-03-15'),
    @('Feb 29 template into a non-leap year -> Feb 28',         'RollForward', '2024-02-29', '2026-01-05',      '2026-01-05', '2026-02-28'),
    @('Feb 29 template into a leap year -> Feb 29',             'RollForward', '2024-02-29', '2027-12-01',      '2027-12-01', '2028-02-29'),
    @('Feb 29 template, start Mar 1 -> next Feb 28',            'RollForward', '2024-02-29', '2026-03-01',      '2026-03-01', '2027-02-28'),
    @('Feb 29 template, start = Feb 28 non-leap -> same day',   'RollForward', '2024-02-29', '2027-02-28',      '2027-02-28', '2027-02-28'),
    @('Feb 28 template, start Feb 29 (leap) -> next year',      'RollForward', '2025-02-28', '2028-02-29',      '2028-02-29', '2029-02-28'),
    @('Dec 31 template, start Dec 31',                          'RollForward', '2025-12-31', '2026-12-31',      '2026-12-31', '2026-12-31'),
    @('Jan 1 template, start Dec 31 -> Jan 1 next year',        'RollForward', '2025-01-01', '2026-12-31',      '2026-12-31', '2027-01-01'),
    @('Start with a time of day: time dropped',                 'RollForward', '2025-04-15', '2026-04-15 13:45','2026-04-15', '2026-04-15'),
    @('Template with a time of day: time dropped',              'RollForward', '2025-04-15 08:30','2026-01-10', '2026-01-10', '2026-04-15'),
    @('Null start date -> fallback (business date) used',       'RollForward', '2025-03-15', '-',               '2026-11-20', '2027-03-15'),
    @('Null start date, fallback before 3/15 -> this year',     'RollForward', '2025-03-15', '-',               '2026-01-02', '2026-03-15'),
    @('Null start AND null fallback -> template date as is',    'RollForward', '2025-03-15', '-',               '-',          '2025-03-15'),
    @('Null template date -> null (nothing copied)',            'RollForward', '-',          '2026-11-20',      '2026-11-20', '-'),
    @('Null template date, null start -> null',                 'RollForward', '-',          '-',               '-',          '-'),
    @('Start set: start wins over the fallback (Add Common Task on an existing project)',
                                                                'RollForward', '2025-03-15', '2026-02-01',      '2026-11-20', '2026-03-15'),
    @('Add Common Task, project started 2024-01-05 -> 2024 date', 'RollForward', '2025-03-15', '2024-01-05', '2026-11-20', '2024-03-15'),
    @('1/1/1900 placeholder template -> next Jan 1',           'RollForward', '1900-01-01', '2026-11-20',      '2026-11-20', '2027-01-01'),
    # realistic: project created today (business date 2026-11-20 = StartDate set by DefaultFromTemplate)
    @('011065 3/15, project created 2026-11-20',                'RollForward', '2025-03-15', '2026-11-20',      '2026-11-20', '2027-03-15'),
    @('011040 4/15, project created 2026-11-20',                'RollForward', '2025-04-15', '2026-11-20',      '2026-11-20', '2027-04-15'),
    @('02TXFRA 5/15, project created 2026-11-20',               'RollForward', '2025-05-15', '2026-11-20',      '2026-11-20', '2027-05-15'),
    # realistic: tax-season project opened in January
    @('011065 3/15, project created 2026-01-05',                'RollForward', '2025-03-15', '2026-01-05',      '2026-01-05', '2026-03-15'),
    @('011040 4/15, project created 2026-01-05',                'RollForward', '2025-04-15', '2026-01-05',      '2026-01-05', '2026-04-15'),
    @('02TXFRA 5/15, project created 2026-01-05',               'RollForward', '2025-05-15', '2026-01-05',      '2026-01-05', '2026-05-15'),
    # realistic: opened 2026-03-20, after the 3/15 deadline (known limitation: rolls to next year; user corrects by hand)
    @('011065 3/15, project created 2026-03-20 (late)',         'RollForward', '2025-03-15', '2026-03-20',      '2026-03-20', '2027-03-15'),
    @('011040 4/15, project created 2026-03-20',                'RollForward', '2025-04-15', '2026-03-20',      '2026-03-20', '2026-04-15'),
    @('02TXFRA 5/15, project created 2026-03-20',               'RollForward', '2025-05-15', '2026-03-20',      '2026-03-20', '2026-05-15'),
    # option A (as is), kept switchable
    @('AsIs: template date copied unchanged',                   'AsIs',        '2025-04-15', '2026-11-20',      '2026-11-20', '2025-04-15'),
    @('AsIs: null template -> null',                            'AsIs',        '-',          '2026-11-20',      '2026-11-20', '-'),
    @('AsIs: Feb 29 kept',                                      'AsIs',        '2024-02-29', '2026-11-20',      '2026-11-20', '2024-02-29')
)
$casesFile = Join-Path $work 'cases.tsv'
($cases | ForEach-Object { $_ -join "`t" }) | Set-Content -Path $casesFile -Encoding utf8

# ---- Harness --------------------------------------------------------------------------------------------------------
$harness = @'
using System;
using System.Globalization;
using System.IO;
using ProjectTask;

internal static class PKTemplateDatesHarness
{
    private static readonly string[] Formats = { "yyyy-MM-dd", "yyyy-MM-dd HH:mm" };

    private static DateTime? D(string s)
    {
        s = s.Trim();
        if (s == "-") return null;
        return DateTime.ParseExact(s, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    private static string S(DateTime? d)
    {
        if (d == null) return "-";
        string f = d.Value.TimeOfDay == TimeSpan.Zero ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm";
        return d.Value.ToString(f, CultureInfo.InvariantCulture);
    }

    private static int Main(string[] args)
    {
        Console.WriteLine("ActiveRule = " + PKTemplateDueDate.ActiveRule);
        Console.WriteLine(string.Format("{0,-4} {1,-3} {2,-58} {3,-11} {4,-16} {5,-16} {6,-11} {7,-11} {8}",
            "", "#", "Case", "Rule", "Template", "Start", "Fallback", "Expected", "Actual"));
        int failed = 0, n = 0;
        foreach (string line in File.ReadAllLines(args[0]))
        {
            string l = line.TrimStart('\uFEFF');
            if (l.Trim().Length == 0) continue;
            string[] c = l.Split('\t');
            n++;
            var rule = (PKTemplateDueDate.DueDateRule)Enum.Parse(typeof(PKTemplateDueDate.DueDateRule), c[1]);
            DateTime? template = D(c[2]), start = D(c[3]), fallback = D(c[4]), expected = D(c[5]);
            string actualText;
            bool ok;
            try
            {
                DateTime? actual = PKTemplateDueDate.GetDueDate(rule, template, start, fallback);
                actualText = S(actual);
                ok = actualText == S(expected);
                // The 3-argument overload (what PKProjectTemplateExt calls) must equal the explicit ActiveRule call.
                if (PKTemplateDueDate.GetDueDate(template, start, fallback) != PKTemplateDueDate.GetDueDate(PKTemplateDueDate.ActiveRule, template, start, fallback))
                { ok = false; actualText += " (3-arg overload mismatch)"; }
            }
            catch (Exception ex) { actualText = "EXCEPTION " + ex.GetType().Name + ": " + ex.Message; ok = false; }
            if (!ok) failed++;
            Console.WriteLine(string.Format("{0,-4} {1,-3} {2,-58} {3,-11} {4,-16} {5,-16} {6,-11} {7,-11} {8}",
                ok ? "PASS" : "FAIL", n, c[0].Length > 58 ? c[0].Substring(0, 55) + "..." : c[0], c[1],
                c[2], c[3], c[4], c[5], actualText));
        }
        Console.WriteLine(string.Format("Cases: {0}, passed {1}, failed {2}", n, n - failed, failed));
        return failed == 0 ? 0 : 1;
    }
}
'@
$harnessFile = Join-Path $work 'PKTemplateDatesHarness.cs'
Set-Content -Path $harnessFile -Value $harness -Encoding utf8

# ---- Static checks ----------------------------------------------------------------------------------------------------
Write-Host "Static checks"
$dueText = [IO.File]::ReadAllText($dueFile)
if ($dueText -match '(?m)^\s*using\s+PX') { $fail++; Write-Host '  FAIL  PKTemplateDueDate.cs references PX namespaces (must stay standalone)' -ForegroundColor Red }
else { Write-Host '  ok    PKTemplateDueDate.cs has no Acumatica usings' }
if (-not (Test-Path $extFile)) { $fail++; Write-Host "  FAIL  $extFile not found" -ForegroundColor Red }
elseif ([IO.File]::ReadAllText($extFile) -notmatch 'PKTemplateDueDate\.GetDueDate\(') { $fail++; Write-Host '  FAIL  PKProjectTemplateExt.cs does not call PKTemplateDueDate.GetDueDate' -ForegroundColor Red }
else { Write-Host '  ok    PKProjectTemplateExt.cs calls PKTemplateDueDate.GetDueDate' }
if ($dueText -match 'ActiveRule\s*=\s*DueDateRule\.(\w+)') {
    if ($Matches[1] -eq $ExpectRule) { Write-Host "  ok    ActiveRule = $($Matches[1]) (expected $ExpectRule)" }
    else { $fail++; Write-Host "  FAIL  ActiveRule = $($Matches[1]), expected $ExpectRule (update -ExpectRule after switching the rule)" -ForegroundColor Red }
} else { $fail++; Write-Host '  FAIL  ActiveRule declaration not found' -ForegroundColor Red }

# ---- Compile and run --------------------------------------------------------------------------------------------------
$exe = Join-Path $work 'PKTemplateDatesTest.exe'
if (Test-Path $exe) { Remove-Item $exe -Force -Confirm:$false }
Write-Host "Compiling $dueFile + harness with $csc ..."
& $csc /nologo /target:exe /langversion:latest /nostdlib- "/r:$fw\System.dll" "/r:$fw\System.Core.dll" "/out:$exe" $dueFile $harnessFile
if ($LASTEXITCODE -ne 0 -or -not (Test-Path $exe)) {
    Write-Host 'COMPILE FAILED' -ForegroundColor Red
    Write-Host 'DATE TEST FAIL' -ForegroundColor Red
    exit 1
}
Write-Host "Running $($cases.Count) case(s)"
& $exe $casesFile
if ($LASTEXITCODE -ne 0) { $fail++ }

if ($fail -eq 0) { Write-Host 'DATE TEST PASS' -ForegroundColor Green; exit 0 }
Write-Host 'DATE TEST FAIL' -ForegroundColor Red
exit 1
