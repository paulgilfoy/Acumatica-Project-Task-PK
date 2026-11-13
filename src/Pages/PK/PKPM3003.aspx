<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="PKPM3003.aspx.cs" Inherits="Page_PKPM3003" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="ProjectTask.PMDocGatheringMaint"
        PrimaryView="DocGathering"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="DocGathering" Width="100%" Height="100px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ControlSize="XM" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule1" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule9" ColumnSpan="2" ></px:PXLayoutRule>
			<px:PXSegmentMask runat="server" ID="CstPXSegmentMask5" DataField="ProjectID" ></px:PXSegmentMask>
			<px:PXDropDown runat="server" ID="CstPXDropDown4" DataField="PMProject__Usr401kStatus" ></px:PXDropDown>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit204" DataField="AdditionalNotes" ></px:PXTextEdit></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab DataMember="DocGatheringProperties" ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Payroll Preprocess">
				<Template>
					<px:PXLayoutRule ColumnSpan="1" GroupCaption="" ControlSize="XM" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule11" StartRow="True" ></px:PXLayoutRule>
					<px:PXLayoutRule GroupCaption="Documents" runat="server" ID="CstPXLayoutRule133" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown157" DataField="MPR" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown158" DataField="PayrollVerification" ></px:PXDropDown>
					<px:PXDropDown runat="server" ID="CstPXDropDown236" DataField="RKAccess" ></px:PXDropDown>
					<px:PXDropDown runat="server" ID="CstPXDropDown241" DataField="RKAccessAutire" ></px:PXDropDown>
					<px:PXDropDown runat="server" ID="CstPXDropDown235" DataField="RecordKeeperName" ></px:PXDropDown>
					<px:PXDropDown runat="server" ID="CstPXDropDown234" DataField="PayrollProvider" ></px:PXDropDown>
					<px:PXDropDown runat="server" ID="CstPXDropDown242" DataField="Finch" ></px:PXDropDown>
					<px:PXLayoutRule SuppressLabel="True" GroupCaption="Last Contact" runat="server" ID="CstPXLayoutRule134" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit33" DataField="MPRLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit129" DataField="PayrollVerLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLabel runat="server" ID="CstLabel238" ></px:PXLabel>
					<px:PXLabel runat="server" ID="CstLabel237" ></px:PXLabel>
					<px:PXLabel runat="server" ID="CstLabel243" ></px:PXLabel>
					<px:PXLabel runat="server" ID="CstLabel244" ></px:PXLabel>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit250" DataField="FinchLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLayoutRule SuppressLabel="True" GroupCaption="Modified By" runat="server" ID="CstPXLayoutRule135" StartColumn="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit168" DataField="MPRLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit169" DataField="PayrollVerLastModUserName" ></px:PXTextEdit>
					<px:PXLabel runat="server" ID="CstLabel245" ></px:PXLabel>
					<px:PXLabel runat="server" ID="CstLabel246" ></px:PXLabel>
					<px:PXLabel runat="server" ID="CstLabel247" ></px:PXLabel>
					<px:PXLabel runat="server" ID="CstLabel248" ></px:PXLabel>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit252" DataField="FinchLastModUserName" ></px:PXTextEdit>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule253" StartColumn="True" GroupCaption="Ash Autire" SuppressLabel="True" ></px:PXLayoutRule>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox254" DataField="MPRAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox255" DataField="PayrollAshAutire" />
					<px:PXLayoutRule ColumnSpan="2" ControlSize="XM" LabelsWidth="SM" GroupCaption="First Contact + Notes" runat="server" ID="CstPXLayoutRule10" StartRow="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit22" DataField="FirstContactDatePayroll" ></px:PXDateTimeEdit>
					<px:PXTextEdit TextMode="MultiLine" TextAlign="Left" Width="255" Height="120" runat="server" ID="CstPXTextEdit198" DataField="NotesPayroll" ></px:PXTextEdit>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox219" DataField="PayrollComplete" ></px:PXCheckBox>
					<px:PXSegmentMask runat="server" ID="CstPXSegmentMask229" DataField="PayrollPreparer" ></px:PXSegmentMask></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Plan Documents">
				<Template>
					<px:PXLayoutRule ControlSize="XM" LabelsWidth="SM" GroupCaption="" runat="server" ID="CstPXLayoutRule13" StartRow="True" ></px:PXLayoutRule>
					<px:PXLayoutRule GroupCaption="Documents" runat="server" ID="CstPXLayoutRule35" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown52" DataField="SPD" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown51" DataField="AA" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown50" DataField="BPD" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown49" DataField="IRS" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown48" DataField="SATrustree" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown47" DataField="SATPA" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown46" DataField="SAInvestment" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown45" DataField="LoanPolicy" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown44" DataField="QDIA" ></px:PXDropDown>
					<px:PXLayoutRule SuppressLabel="True" GroupCaption="Last Contact" runat="server" ID="CstPXLayoutRule34" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit62" DataField="SPDLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit61" DataField="AALastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit60" DataField="BPDLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit59" DataField="IRSLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit58" DataField="SATrustreeLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit57" DataField="SATPALastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit56" DataField="SAInvestmentLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit55" DataField="LoanPolicyLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit53" DataField="QDIALastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule136" StartColumn="True" GroupCaption="Modified By" SuppressLabel="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit154" DataField="SPDLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit146" DataField="AALastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit147" DataField="BPDLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit172" DataField="IRSLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit153" DataField="SATrustreeLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit152" DataField="SATPALastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit151" DataField="SAInvestmentLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit148" DataField="LoanPolicyLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit171" DataField="QDIALastModUserName" ></px:PXTextEdit>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule256" StartColumn="True" GroupCaption="Ash Autire" SuppressLabel="True" ></px:PXLayoutRule>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox265" DataField="SPDAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox257" DataField="AAAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox258" DataField="BPDAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox259" DataField="IRSAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox264" DataField="SATrustreeAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox263" DataField="SATPAAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox262" DataField="SAInvestmentAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox260" DataField="LoanPolicyAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox261" DataField="QDIAAshAutire" />
					<px:PXLayoutRule ColumnSpan="2" ControlSize="XM" LabelsWidth="SM" GroupCaption="First Contact + Notes" runat="server" ID="CstPXLayoutRule12" StartRow="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit23" DataField="FirstContactDatePlanDocuments" ></px:PXDateTimeEdit>
					<px:PXTextEdit Height="120" TextMode="MultiLine" TextAlign="Left" Width="255" runat="server" ID="CstPXTextEdit203" DataField="NotesPlanDocuments" ></px:PXTextEdit>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox221" DataField="PlanDocumentsComplete" ></px:PXCheckBox>
					<px:PXSegmentMask runat="server" ID="CstPXSegmentMask231" DataField="PlanDocPreparer" ></px:PXSegmentMask></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="PlanDoc Preprocess" >
				<Template>
					<px:PXLayoutRule ControlSize="XM" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule15" StartRow="True" ></px:PXLayoutRule>
					<px:PXLayoutRule GroupCaption="Documents" runat="server" ID="CstPXLayoutRule36" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown66" DataField="EC" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown68" DataField="PAS" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown63" DataField="Cont" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown65" DataField="Dist" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown67" DataField="Loans" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown64" DataField="DefElection" ></px:PXDropDown>
					<px:PXLayoutRule SuppressLabel="True" GroupCaption="Last Contact" runat="server" ID="CstPXLayoutRule37" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit77" DataField="ECLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit79" DataField="PASLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit74" DataField="ContLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit76" DataField="DistLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit78" DataField="LoansLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit75" DataField="DefElectionLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLayoutRule SuppressLabel="True" runat="server" ID="CstPXLayoutRule161" StartColumn="True" GroupCaption="Modified By" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit174" DataField="ECLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit178" DataField="PASLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit176" DataField="ContLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit177" DataField="DistLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit175" DataField="LoansLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit173" DataField="DefElectionLastModUserName" ></px:PXTextEdit>
					<px:PXLayoutRule SuppressLabel="True" runat="server" ID="CstPXLayoutRule266" StartColumn="True" GroupCaption="Ash Autire" ></px:PXLayoutRule>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox270" DataField="ECAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox272" DataField="PASAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox267" DataField="ContAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox269" DataField="DistAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox271" DataField="LoansAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox268" DataField="DefElectionAshAutire" />
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule205" StartRow="True" GroupCaption="First Contact + Notes" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit206" DataField="FirstContactDatePlanDocPreproc" ></px:PXDateTimeEdit>
					<px:PXTextEdit TextMode="MultiLine" Height="120" TextAlign="Left" Width="255" runat="server" ID="CstPXTextEdit207" DataField="NotesPlanDocPreproc" ></px:PXTextEdit>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox222" DataField="PlanDocPreprocComplete" ></px:PXCheckBox></Template></px:PXTabItem>
			<px:PXTabItem Text="Plan Design" >
				<Template>
					<px:PXLayoutRule ControlSize="XM" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule21" StartRow="True" ></px:PXLayoutRule>
					<px:PXLayoutRule GroupCaption="Documents" runat="server" ID="CstPXLayoutRule42" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown120" DataField="PlanProvisions" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown122" DataField="PreProcessDate" ></px:PXDropDown>
					<px:PXLayoutRule SuppressLabel="True" GroupCaption="Last Contact" runat="server" ID="CstPXLayoutRule43" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit121" DataField="PlanProvisionsLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit123" DataField="PreProcessDateLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLayoutRule GroupCaption="Modified By" SuppressLabel="True" runat="server" ID="CstPXLayoutRule179" StartColumn="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit180" DataField="PlanProvisionsLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit181" DataField="PreProcessDateLastModUserName" ></px:PXTextEdit>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule273" StartColumn="True" SuppressLabel="True" GroupCaption="Ash Autire" ></px:PXLayoutRule>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox276" DataField="PlanProvisionsAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox277" DataField="PreProcessDateAshAutire" />
					<px:PXLayoutRule ColumnSpan="2" ControlSize="XM" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule208" StartRow="True" GroupCaption="First Contact + Notes" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit209" DataField="FirstContactDatePlanDesign" ></px:PXDateTimeEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit210" DataField="NotesPlanDesign" Height="120" TextAlign="Left" TextMode="MultiLine" Width="255" ></px:PXTextEdit>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox224" DataField="PlanDesignComplete" ></px:PXCheckBox>
					<px:PXSegmentMask runat="server" ID="CstPXSegmentMask233" DataField="PlanDesignPreparer" ></px:PXSegmentMask></Template></px:PXTabItem>
			<px:PXTabItem Text="Annual Documents" >
				<Template>
					<px:PXLayoutRule ControlSize="XM" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule17" StartRow="True" ></px:PXLayoutRule>
					<px:PXLayoutRule GroupCaption="Documents" runat="server" ID="CstPXLayoutRule38" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown runat="server" ID="CstPXDropDown159" DataField="Form5500" CommitChanges="True" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown104" DataField="TrustState" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown102" DataField="TrustCert" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown96" DataField="FidelityBond" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown94" DataField="ComplianceTest" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown100" DataField="RKFeeDisc" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown106" DataField="InvestAllocation" ></px:PXDropDown>
					<px:PXLayoutRule SuppressLabel="True" GroupCaption="Last Contact" runat="server" ID="CstPXLayoutRule39" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit160" DataField="Form5500LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit105" DataField="TrustStateLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit103" DataField="TrustCertLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit97" DataField="FidelityBondLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit95" DataField="ComplianceTestLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit101" DataField="RKFeeDiscLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit107" DataField="InvestAllocationLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLayoutRule GroupCaption="Modified By" runat="server" ID="CstPXLayoutRule182" StartColumn="True" SuppressLabel="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit185" DataField="Form5500LastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit189" DataField="TrustStateLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit188" DataField="TrustCertLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit184" DataField="FidelityBondLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit183" DataField="ComplianceTestLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit187" DataField="RKFeeDiscLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit186" DataField="InvestAllocationLastModUserName" ></px:PXTextEdit>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule278" StartColumn="True" SuppressLabel="True" GroupCaption="Ash Autire" ></px:PXLayoutRule>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox281" DataField="Form5500AshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox285" DataField="TrustStateAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox284" DataField="TrustCertAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox280" DataField="FidelityBondAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox279" DataField="ComplianceTestAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox283" DataField="RKFeeDiscAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox282" DataField="InvestAllocationAshAutire" />
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule211" StartRow="True" GroupCaption="First Contact + Notes" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit214" DataField="FirstContactDateAnnualDocuments" ></px:PXDateTimeEdit>
					<px:PXTextEdit Height="120" TextAlign="Left" TextMode="MultiLine" Width="255" runat="server" ID="CstPXTextEdit215" DataField="NotesAnnualDocuments" ></px:PXTextEdit>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox226" DataField="AnnualDocumentsComplete" ></px:PXCheckBox></Template></px:PXTabItem>
			<px:PXTabItem Text="First Year PK" >
				<Template>
					<px:PXLayoutRule ControlSize="XM" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule19" StartRow="True" ></px:PXLayoutRule>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule40" StartColumn="True" GroupCaption="Documents" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown108" DataField="ClientAuth" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown110" DataField="Communication" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown112" DataField="PY5500" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown118" DataField="PYTrustState" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown116" DataField="PYTrustCert" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown114" DataField="PYCompliance" ></px:PXDropDown>
					<px:PXLayoutRule SuppressLabel="True" GroupCaption="Last Contact" runat="server" ID="CstPXLayoutRule41" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit109" DataField="ClientAuthLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit111" DataField="CommunicationLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit113" DataField="PY5500LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit119" DataField="PYTrustStateLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit117" DataField="PYTrustCertLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit115" DataField="PYComplianceLastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule190" StartColumn="True" SuppressLabel="True" GroupCaption="Modified By" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit191" DataField="ClientAuthLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit192" DataField="CommunicationLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit193" DataField="PY5500LastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit196" DataField="PYTrustStateLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit195" DataField="PYTrustCertLastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit194" DataField="PYComplianceLastModUserName" ></px:PXTextEdit>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule286" StartColumn="True" SuppressLabel="True" GroupCaption="Ash Autire" ></px:PXLayoutRule>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox287" DataField="ClientAuthAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox288" DataField="CommunicationAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox289" DataField="PY5500AshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox292" DataField="PYTrustStateAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox291" DataField="PYTrustCertAshAutire" />
					<px:PXCheckBox runat="server" ID="CstPXCheckBox290" DataField="PYComplianceAshAutire" />
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule216" StartRow="True" GroupCaption="First Contact + Notes" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit217" DataField="FirstContactDateFirstYearPK" ></px:PXDateTimeEdit>
					<px:PXTextEdit Height="120" TextMode="MultiLine" TextAlign="Left" Width="255" runat="server" ID="CstPXTextEdit218" DataField="NotesFirstYearPK" ></px:PXTextEdit>
					<px:PXCheckBox runat="server" ID="CstPXCheckBox228" DataField="FirstYearPKComplete" ></px:PXCheckBox></Template></px:PXTabItem></Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
	</px:PXTab>
</asp:Content>