<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AH201000.aspx.cs" Inherits="Page_AH201000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="transcriptdetail" TypeName="PX.FirefliesConnector.Ext.ProcessTranscript" >
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid ID="grid" runat="server" Height="400px" Width="100%" Style="z-index: 100"
		AllowPaging="True" AllowSearch="True" AdjustPageSize="Auto" DataSourceID="ds" SkinID="Primary" TabIndex="900">
		<Levels>
			<px:PXGridLevel DataKeyNames="Noteid" DataMember="transcriptdetail">
			    <RowTemplate>
                    <px:PXCheckBox ID="edSelected" runat="server" AlreadyLocalized="False" DataField="Selected" Text="Selected">
                    </px:PXCheckBox>
                    <px:PXTextEdit ID="edId" runat="server" AlreadyLocalized="False" DataField="Id" DefaultLocale="">
                    </px:PXTextEdit>
                    <px:PXTextEdit ID="edTitle" runat="server" AlreadyLocalized="False" DataField="Title" DefaultLocale="">
                    </px:PXTextEdit>
                    <px:PXDateTimeEdit ID="edDate" runat="server" AlreadyLocalized="False" DataField="Date" DefaultLocale="">
                    </px:PXDateTimeEdit>
                    <px:PXTextEdit ID="edTranscripturl" runat="server" AlreadyLocalized="False" DataField="Transcripturl" DefaultLocale="">
                    </px:PXTextEdit>
                    <px:PXTextEdit ID="edParticipants" runat="server" AlreadyLocalized="False" DataField="Participants" DefaultLocale="">
                    </px:PXTextEdit>
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn DataField="Selected" TextAlign="Center" Type="CheckBox" Width="60px" AllowCheckAll="true">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="Id" Width="280px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="Title" Width="280px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="Date" Width="90px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="Transcripturl" Width="280px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="Participants" Width="280px">
                    </px:PXGridColumn>
                </Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" />
	</px:PXGrid>
</asp:Content>
