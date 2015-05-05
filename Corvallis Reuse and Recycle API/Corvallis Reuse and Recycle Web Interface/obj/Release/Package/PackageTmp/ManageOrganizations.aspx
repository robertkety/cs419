<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageOrganizations.aspx.cs" Inherits="CRRD_Web_Interface.ManageOrganizations" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Organizations</h2>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelOrganizationInfo" runat="server" Visible="false">
        <h3>Organization Info</h3>
        <asp:GridView ID="GridViewOrganizationInfo" runat="server" AutoGenerateColumns="False" OnRowEditing="GridViewOrganizationInfo_RowEditing" 
            OnRowCancelingEdit="GridViewOrganizationInfo_RowCancelingEdit" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True"
             OnPageIndexChanged="GridViewOrganizationInfo_PageIndexChanged" OnPageIndexChanging="GridViewOrganizationInfo_PageIndexChanging" ShowFooter="true">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Category Name">
                    <ItemTemplate>
                        <%# Eval("OrganizationName") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click"/>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Left" />
                    <HeaderStyle Width="30%" />
                </asp:TemplateField>
                <asp:BoundField DataField="OrganizationPhone" HeaderText="Phone" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine1" HeaderText="Address 1" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine2" HeaderText="Address 2" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine3" HeaderText="Address 3" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationZipCode" HeaderText="Zip Code" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationWebsite" HeaderText="Website" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationHours" HeaderText="Hours" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationNotes" HeaderText="Notes" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:CommandField ShowEditButton="True" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="5%" />
                </asp:CommandField>
                <asp:CommandField ShowDeleteButton="True" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="5%" />
                </asp:CommandField>
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <div>
            <asp:LinkButton ID="LinkButtonAddOrganization" runat="server">+ Add a New Organization</asp:LinkButton>
        </div>
    </asp:Panel>
</asp:Content>
