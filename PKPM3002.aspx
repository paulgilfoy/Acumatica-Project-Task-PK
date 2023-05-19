<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="PKPM3002.aspx.cs" Inherits="Page_PKPM3002" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="ProjectTask.PMSampleTestingMaint"
        PrimaryView="Sampletest"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Sampletest" Width="100%" Height="100px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ControlSize="XM" LabelsWidth="SM" StartColumn="True" ID="PXLayoutRule1" runat="server" StartRow=""></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule51" ColumnSpan="2" ></px:PXLayoutRule>
			<px:PXSegmentMask runat="server" ID="CstPXSegmentMask1" DataField="ProjectID" ></px:PXSegmentMask>
			<px:PXDropDown runat="server" ID="CstPXDropDown2" DataField="PMProject__Usr401kStatus" ></px:PXDropDown>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit4" DataField="PMProject__CustomerID_BAccountR_acctName" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule50" StartColumn="True" ></px:PXLayoutRule>
			<px:PXCheckBox runat="server" ID="CstPXCheckBox52" DataField="Complete" ></px:PXCheckBox>
			<px:PXDropDown runat="server" ID="CstPXDropDown53" DataField="Priority" ></px:PXDropDown></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab DataMember="SampletestProperties" ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Sample Tracking">
				<Template>
					<px:PXLayoutRule ControlSize="XM" LabelsWidth="SM" ColumnSpan="1" runat="server" ID="CstPXLayoutRule5" StartRow="True" ></px:PXLayoutRule>
					<px:PXLayoutRule GroupCaption="Sample Collecting" runat="server" ID="CstPXLayoutRule7" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown10" DataField="CO01" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown12" DataField="CO02" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown13" DataField="CO03" ></px:PXDropDown>
					<px:PXLabel runat="server" ID="CstLabel76" ></px:PXLabel>
					<px:PXLayoutRule GroupCaption="Last Contact" SuppressLabel="True" runat="server" ID="CstPXLayoutRule8" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit23" DataField="CO01LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit24" DataField="CO02LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit25" DataField="CO03LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLabel runat="server" ID="CstLabel77" ></px:PXLabel>
					<px:PXLayoutRule GroupCaption="Modified By" SuppressLabel="True" runat="server" ID="CstPXLayoutRule9" StartColumn="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit34" DataField="CO01LastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit35" DataField="CO02LastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit36" DataField="CO03LastModUserName" ></px:PXTextEdit>
					<px:PXLabel runat="server" ID="CstLabel78" ></px:PXLabel>
					<px:PXLayoutRule ColumnSpan="1" ControlSize="XM" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule54" StartRow="True" ></px:PXLayoutRule>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule55" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown14" DataField="CO04" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown15" DataField="CO05" ></px:PXDropDown>
					<px:PXLabel Height="4" runat="server" ID="CstLabel79" ></px:PXLabel>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule56" StartColumn="True" SuppressLabel="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit26" DataField="CO04LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit27" DataField="CO05LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLabel Height="4" runat="server" ID="CstLabel80" ></px:PXLabel>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule57" StartColumn="True" SuppressLabel="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit37" DataField="CO04LastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit38" DataField="CO05LastModUserName" ></px:PXTextEdit>
					<px:PXLabel Height="4" runat="server" ID="CstLabel81" ></px:PXLabel>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule58" StartRow="True" ColumnSpan="1" ControlSize="XM" LabelsWidth="SM" ></px:PXLayoutRule>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule59" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown16" DataField="CO06" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown17" DataField="CO07" ></px:PXDropDown>
					<px:PXLabel runat="server" ID="CstLabel82" ></px:PXLabel>
					<px:PXLayoutRule SuppressLabel="True" runat="server" ID="CstPXLayoutRule60" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit28" DataField="CO06LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit29" DataField="CO07LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLabel runat="server" ID="CstLabel83" ></px:PXLabel>
					<px:PXLayoutRule SuppressLabel="True" runat="server" ID="CstPXLayoutRule61" StartColumn="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit39" DataField="CO06LastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit40" DataField="CO07LastModUserName" ></px:PXTextEdit>
					<px:PXLabel runat="server" ID="CstLabel84" ></px:PXLabel>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule62" StartRow="True" ColumnSpan="1" ControlSize="XM" LabelsWidth="SM" ></px:PXLayoutRule>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule63" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown18" DataField="CO08" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown19" DataField="IA08" ></px:PXDropDown>
					<px:PXLabel runat="server" ID="CstLabel85" ></px:PXLabel>
					<px:PXLayoutRule SuppressLabel="True" runat="server" ID="CstPXLayoutRule64" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit30" DataField="CO08LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit31" DataField="IA08LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLabel runat="server" ID="CstLabel86" ></px:PXLabel>
					<px:PXLayoutRule SuppressLabel="True" runat="server" ID="CstPXLayoutRule65" StartColumn="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit41" DataField="CO08LastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit42" DataField="IA08LastModUserName" ></px:PXTextEdit>
					<px:PXLabel runat="server" ID="CstLabel87" ></px:PXLabel>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule66" StartRow="True" ColumnSpan="1" ControlSize="XM" LabelsWidth="SM" ></px:PXLayoutRule>
					<px:PXLayoutRule runat="server" ID="CstPXLayoutRule67" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown11" DataField="BP01" ></px:PXDropDown>
					<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown20" DataField="LO01" ></px:PXDropDown>
					<px:PXLayoutRule SuppressLabel="True" runat="server" ID="CstPXLayoutRule68" StartColumn="True" ></px:PXLayoutRule>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit22" DataField="BP01LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit32" DataField="LO01LastModifiedDateTime" ></px:PXDateTimeEdit>
					<px:PXLayoutRule SuppressLabel="True" runat="server" ID="CstPXLayoutRule69" StartColumn="True" ></px:PXLayoutRule>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit33" DataField="BP01LastModUserName" ></px:PXTextEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit43" DataField="LO01LastModUserName" ></px:PXTextEdit>
					<px:PXLayoutRule ColumnSpan="2" ControlSize="XM" GroupCaption="First Contact + Notes" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule6" StartRow="True" ></px:PXLayoutRule>
					<px:PXDropDown runat="server" ID="CstPXDropDown48" DataField="RecordKeeperName" ></px:PXDropDown>
					<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit47" DataField="FirstContactDate" ></px:PXDateTimeEdit>
					<px:PXTextEdit runat="server" ID="CstPXTextEdit45" DataField="AdditionalNotes" Height="120" TextAlign="Left" TextMode="MultiLine" Width="255" ></px:PXTextEdit>
					<px:PXSegmentMask runat="server" ID="CstPXSegmentMask49" DataField="Tester" ></px:PXSegmentMask></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Tab item 2">
				<Template>
					
				</Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
	</px:PXTab>
</asp:Content>