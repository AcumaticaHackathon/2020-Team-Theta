<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AH100000.aspx.cs" Inherits="Page_AH100000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="config" TypeName="PX.FirefliesConnector.Ext.FirefliesConfigEntry">
        <CallbackCommands>
        </CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="config" TabIndex="100">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" ColumnWidth="L" GroupCaption="Connection Details"/>
            <px:PXTextEdit ID="edAPIEndPoint" runat="server" AlreadyLocalized="False" DataField="APIEndPoint" />
            <px:PXTextEdit ID="edApikey" runat="server" AlreadyLocalized="False" DataField="Apikey" />
		</Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" />
	</px:PXFormView>
</asp:Content>