<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageCategoryItems.aspx.cs" Inherits="CRRD_Web_Interface.ManageCategoryItems" async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="Stylesheet" href="Styles/StyleSheetPageElements.css" type="text/css" />
    <h2>Manage Category/Item Relationships</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="LiteralErrorMessageGridView" />
    </p>
    <asp:Label ID="LabelInstruction" runat="server" Text="Begin by selecting a category:" AssociatedControlID="DropDownListCategories" CssClass="DropDownListLabel"></asp:Label>
    <asp:DropDownList ID="DropDownListCategories" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListCategories_SelectedIndexChanged" cssclass="DropDownList">
        <asp:ListItem Value="-1">&lt;Select a Category&gt;</asp:ListItem>
    </asp:DropDownList>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelCategoryItems" runat="server" Visible="false">
        <asp:GridView ID="GridViewCategoryItems" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            AllowPaging="True" OnPageIndexChanged="GridViewCategoryItems_PageIndexChanged" OnPageIndexChanging="GridViewCategoryItems_PageIndexChanging" ShowFooter="True" 
            OnRowCancelingEdit="GridViewCategoryItems_RowCancelingEdit" OnRowEditing="GridViewCategoryItems_RowEditing" OnRowUpdating="GridViewCategoryItems_RowUpdating">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" CssClass="GridView"/>
            <Columns>
                <asp:TemplateField HeaderText="Item ID" Visible="false" ItemStyle-Width="0%">
                    <ItemTemplate>
                        <%# Eval("ItemID") %>
                    </ItemTemplate>
                    <ItemStyle Width="0%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Name" ItemStyle-Width="60%" HeaderStyle-Width="60%">
                    <ItemTemplate>
                        <%# Eval("ItemName") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click"/>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Member" HeaderText="Category Member" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                </asp:CheckBoxField>
                <asp:CommandField ShowEditButton="True" HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center"/>
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
    </asp:Panel>
</asp:Content>
