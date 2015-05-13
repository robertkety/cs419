<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CRRD_Web_Interface._Default"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="PanelAnon" runat="server" Visible="false">
        <p>
            Please <asp:HyperLink ID="HyperLinkLogin" runat="server" NavigateUrl="~/Account/Login.aspx">login</asp:HyperLink> to view content.
        </p>
    </asp:Panel>
    <asp:Panel ID="PanelLogged" runat="server">
        <h2>Management Portal</h2>
        <h3>Users and Security</h3>
        <ul class="LinkList">
            <li>
                <asp:HyperLink ID="HyperLinkManageAccount" runat="server" NavigateUrl="~/Account/Manage.aspx">Manage Account</asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="HyperLinkManageUsers" runat="server" NavigateUrl="~/ManageUsers.aspx">Manage Users</asp:HyperLink>
            </li>
        </ul>
        <h3>Table Managemenet</h3>
        <ul class="LinkList">
            <li>
                <asp:HyperLink ID="HyperLinkManageOrganizations" runat="server" NavigateUrl="~/ManageOrganizations.aspx">Manage Organizations</asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="HyperLinkManageCategories" runat="server" NavigateUrl="~/ManageCategories.aspx">Manage Categories</asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="HyperLinkManageItems" runat="server" NavigateUrl="~/ManageItems.aspx">Manage Items</asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="HyperLinkManageCategoryItems" runat="server" NavigateUrl="~/ManageCategoryItems">Manage Category/Item Relationships</asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="HyperLinkManageItemOrganization" runat="server" NavigateUrl="~/ManageItemOrganization.aspx">Manage Item/Organization Relationships</asp:HyperLink>
            </li>
        </ul>
    </asp:Panel>
</asp:Content>
