<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageItems.aspx.cs" Inherits="CRRD_Web_Interface.ManageItems" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Items</h2>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelItemInfo" runat="server" Visible="false">
        <h3>Items</h3>
        <p class="text-danger">
            <asp:Literal runat="server" ID="LiteralErrorMessageGridView" />
        </p>
        <asp:GridView ID="GridViewItemInfo" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
            OnRowEditing="GridViewItemInfo_RowEditing" OnRowCancelingEdit="GridViewItemInfo_RowCancelingEdit" AllowPaging="True" 
            OnPageIndexChanged="GridViewItemInfo_PageIndexChanged" OnPageIndexChanging="GridViewItemInfo_PageIndexChanging" ShowFooter="true"
            OnRowDeleting="GridViewItemInfo_RowDeleting" OnRowUpdating="GridViewItemInfo_RowUpdating">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Item ID" Visible="false">
                    <ItemTemplate>
                        <%# Eval("ItemID") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Name">
                    <ItemTemplate>
                        <%# Eval("ItemName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxEditItemName"  runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click"/>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Left" />
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>
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
            <asp:LinkButton ID="LinkButtonAddItem" runat="server" OnClick="LinkButtonAddItem_Click">+ Add a New Item</asp:LinkButton>
        </div>
        <asp:Panel ID="PanelAddItem" runat="server" Visible="false">
            <div class="form-horizontal">
                <h4>Create a New Item</h4>
                <p class="text-danger">
                    <asp:Literal runat="server" ID="LiteralErrorMessageAddItem" />
                </p>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxItemName" CssClass="col-md-2 control-label">Item Name</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxItemName" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server" ID="ButtonAddItem" OnClick="ButtonAddItem_Click" Text="Add Item" CssClass="btn btn-default" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
