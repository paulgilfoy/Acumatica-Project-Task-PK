using PX.Data;

namespace ProjectTask
{
    public static class DocStatus
    {
        public const string Contacted1 = "C1";
        public const string Contacted2 = "C2";
        public const string ClientCalled = "CC";
        public const string WaitingonInfo = "WI";
        public const string InProcess = "IP";
        public const string Completed = "CM";
        public const string Ghost = "GH";
        public const string NA = "NA";
    }
    public static class AccessStatus
    {
        public const string Requested = "RQ";
        public const string Completed = "CM";
        public const string NA = "NA";
    }
    public static class PlanDesignStatus
    {
        public const string WaitingonInfo = "WI";
        public const string InProcess = "IP";
        public const string Completed = "CM";
    }

    public static class RecordKeeperList
    {
        public const string ADP = "AD1";
        public const string Alerus = "ARB";
        public const string Ascensus = "ACS";
        public const string ATRetirement = "ATR";
        public const string American = "AUL";
        public const string Betterment = "BFB";
        public const string BOK = "BOK";
        public const string CapitalGroup = "CGR";
        public const string CapitalAmerican = "CAF";
        public const string CMFG = "CMF";
        public const string EmployeeFid = "EFY";
        public const string Empower1 = "EM1";
        public const string Empower2 = "EM2";
        public const string Empower3 = "EM3";
        public const string EPIC = "ERP";
        public const string Equitable = "EQU";
        public const string ERISA = "ERI";
        public const string Fidelity = "FWS";
        public const string John1 = "JH1";
        public const string John2 = "JH2";
        public const string Lincoln1 = "LND";
        public const string Lincoln2 = "LNA";
        public const string Leading = "LRS";
        public const string MidAtlantic = "MAT";
        public const string MassMutual1 = "MM1";
        public const string MassMutual2 = "MM2";
        public const string Newport = "NWG";
        public const string Nationwide = "NFS";
        public const string OneAmerica = "OAR";
        public const string Paychex = "PCX";
        public const string Professional = "PCS";
        public const string Principal = "PLI";
        public const string Prudential = "PRI";
        public const string RHIAcquisition = "RAL";
        public const string Retirement = "RPC";
        public const string Charles = "CST";
        public const string Securian = "SFG";
        public const string Standard = "SIC";
        public const string TRowe = "TRP";
        public const string Transamerica = "TRS";
        public const string USIConslting = "UCG";
        public const string Voya = "VYF";
        public const string OtherRK = "ORK";
    }
    public static class PayrollProviderList
    {
        public const string ADP1 = "AD1";
        public const string ADP2 = "AD2";
        public const string ADP3 = "AD3";
        public const string Alliance = "APS";
        public const string CBIZ = "CBI";
        public const string Coastal = "CPS";
        public const string CertiPay = "CPA";
        public const string Dominion = "DPS";
        public const string ExponentHR = "EHR";
        public const string GAPartners = "GAP";
        public const string Heartland = "HPS";
        public const string ISolved = "ISH";
        public const string Kelly = "KBP";
        public const string Netchex = "NET";
        public const string Paychex = "PCX";
        public const string Paycom = "PSW";
        public const string Paycor = "PCR";
        public const string Paylocity = "PLC";
        public const string Payday = "PDY";
        public const string PayMaster = "PMR";
        public const string Proliant = "PRO";
        public const string Paytime = "PYT";
        public const string Ultimate = "UKG";
        public const string Viventium = "VSW";
        public const string OtherPR = "OPP";
    }
    public static class SampleStatus
    {
        public const string Contacted1 = "C1";
        public const string Contacted2 = "C2";
        public const string ClientCalled = "CC";
        public const string TPARKContacted = "RK";
        public const string WaitingonInfo = "WI";
        public const string InProcess = "IP";
        public const string ClearingPoints = "CP";
        public const string Completed = "CM";
        public const string Ghost = "GH";
        public const string NA = "NA";
        public const string Reviewed = "RW";
    }

    public static class SamplePriority
    {
        public const string High = "H";
        public const string Medium = "M";
        public const string Low = "L";
        public const string Post = "P";
        public const string InitialReview = "I";
    }

    public static class ConstantTaskCD
    {
        private const string Payroll = "02PAYROLL             ";
        private const string PlanDoc = "03PLANDOC             ";
        private const string PlanDesign = "04PLANDESIGN          ";
        public class payroll : PX.Data.BQL.BqlString.Constant<payroll>
        {
            public payroll() : base(Payroll) { }
        }
        public class plandoc : PX.Data.BQL.BqlString.Constant<plandoc>
        {
            public plandoc() : base(PlanDoc) { }
        }
        public class plandesign : PX.Data.BQL.BqlString.Constant<plandesign>
        {
            public plandesign() : base(PlanDesign) { }
        }
    }

    public static class Department
    {
        public const string n401k = "401K";
    }
}
