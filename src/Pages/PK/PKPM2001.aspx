<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="PKPM2001.aspx.cs" Inherits="Page_PKPM2001" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="ProjectTask.PKProjectStatusMaint"
        PrimaryView="Filter"
        >
		<CallbackCommands>
			<px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Caption="Selection" AllowAutoHide="false" AllowCollapse="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
			<px:PXSelector ID="edDepartmentID" runat="server" DataField="DepartmentID" CommitChanges="True" />
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" Caption="Statuses" AllowAutoHide="false" SyncPosition="true">
		<Levels>
			<px:PXGridLevel DataMember="Statuses">
				<Columns>
					<px:PXGridColumn DataField="StatusID" TextAlign="Right" CommitChanges="True" />
					<px:PXGridColumn DataField="Description" Width="300px" />
					<px:PXGridColumn DataField="SortOrder" TextAlign="Right" />
					<px:PXGridColumn DataField="Active" Type="CheckBox" TextAlign="Center" />
				</Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<Mode InitNewRow="true" />
	</px:PXGrid>
</asp:Content>
