<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageCategories.aspx.cs" Inherits="CRRD_Web_Interface.ManageCategories" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Categories</h2>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelCategoryInfo" runat="server" Visible="false">
        <h3>Categories</h3>
        <asp:GridView ID="GridViewCategoryInfo" runat="server" AutoGenerateColumns="False" OnRowEditing="GridViewCategoryInfo_RowEditing"
             OnRowCancelingEdit="GridViewCategoryInfo_RowCancelingEdit" AllowPaging="True" AllowSorting="True" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanged="GridViewOrganizationInfo_PageIndexChanged" OnPageIndexChanging="GridViewOrganizationInfo_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="CategoryName" HeaderText="Name" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="40%" />
                </asp:BoundField>
                <asp:CommandField ShowEditButton="True">
                <ControlStyle Width="100%" />
                <HeaderStyle Width="5%" />
                </asp:CommandField>
                <asp:CommandField ShowDeleteButton="True">
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
            <asp:LinkButton ID="LinkButtonAddCategory" runat="server">+ Add a New Category</asp:LinkButton>
        </div>
    </asp:Panel>
 </asp:Content>