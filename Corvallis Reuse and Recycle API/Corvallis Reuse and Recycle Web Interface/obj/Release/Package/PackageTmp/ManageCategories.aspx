<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageCategories.aspx.cs" Inherits="CRRD_Web_Interface.ManageCategories" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Categories</h2>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelCategoryInfo" runat="server" Visible="false">
        <h3>Categories</h3>
        <p class="text-danger">
            <asp:Literal runat="server" ID="LiteralErrorMessageGridView" />
        </p>
        <asp:GridView ID="GridViewCategoryInfo" runat="server" AutoGenerateColumns="False" OnRowEditing="GridViewCategoryInfo_RowEditing"
            OnRowCancelingEdit="GridViewCategoryInfo_RowCancelingEdit" AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None" 
            OnPageIndexChanged="GridViewCategoryInfo_PageIndexChanged" OnPageIndexChanging="GridViewCategoryInfo_PageIndexChanging" ShowFooter="true"
            OnRowUpdating="GridViewCategoryInfo_RowUpdating" OnRowDeleting="GridViewCategoryInfo_RowDeleting">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Category ID" Visible="false">
                    <ItemTemplate>
                        <%# Eval("CategoryID") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Category Name">
                    <ItemTemplate>
                        <%# Eval("CategoryName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxEditCategoryName"  runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click"/>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Left" />
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>
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
            <asp:LinkButton ID="LinkButtonAddCategory" runat="server" OnClick="LinkButtonAddCategory_Click">+ Add a New Category</asp:LinkButton>
        </div>
        <asp:Panel ID="PanelAddCategory" runat="server" Visible="false">
            <div class="form-horizontal">
                <h4>Create a New Category</h4>
                <p class="text-danger">
                    <asp:Literal runat="server" ID="LiteralErrorMessageAddCategory" />
                </p>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxCategoryName" CssClass="col-md-2 control-label">Category Name</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxCategoryName" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server" ID="ButtonAddCategory" OnClick="ButtonAddCategory_Click" Text="Add Category" CssClass="btn btn-default" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
 </asp:Content>