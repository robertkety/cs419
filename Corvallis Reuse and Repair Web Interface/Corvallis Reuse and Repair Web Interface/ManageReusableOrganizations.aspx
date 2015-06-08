<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageReusableOrganizations.aspx.cs" MasterPageFile="~/Site.Master" Inherits="CRRD_Web_Interface.ManageReusableOrganizations" async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Reusable Item/Organization Combinations</h2>
     <p class="text-danger">
        <asp:Literal runat="server" ID="LiteralErrorMessageGridView" />
    </p>
    <asp:Label ID="LabelInstruction" runat="server" Text="Begin by selecting an item:"></asp:Label>
    <asp:DropDownList ID="DropDownListReusableItems" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListReusableItems_SelectedIndexChanged">
        <asp:ListItem Value="-1">&lt;Select an Item&gt;</asp:ListItem>
    </asp:DropDownList>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelReusableOrganization" runat="server" Visible="false">
        <asp:GridView ID="GridViewReusableOrganizations" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            AllowPaging="True" OnPageIndexChanged="GridViewReusableOrganizations_PageIndexChanged" 
            OnPageIndexChanging="GridViewReusableOrganizations_PageIndexChanging" ShowFooter="True" OnRowDeleting="GridViewOrganizationInfo_RowDeleting">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Organization ID" Visible="false">
                    <ItemTemplate>
                        <%# Eval("OrganizationID") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Organization Name">
                    <ItemTemplate>
                        <%# Eval("OrganizationName") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click"/>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Left" />
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>
                <asp:BoundField DataField="OrganizationAddressLine1" HeaderText="Address 1" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="20%" />
                </asp:BoundField>
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
            <asp:LinkButton ID="LinkButtonAddReusable" runat="server" OnClick="LinkButtonAddReusable_Click">+ Add a New Item/Organization Combination</asp:LinkButton>
        </div>
        <asp:Panel ID="PanelAddReusable" runat="server" Visible="false">
            <div class="form-horizontal">
                <h4>Create a New Reusable Relationship</h4>
                <p class="text-danger">
                    <asp:Literal runat="server" ID="LiteralErrorMessageAddOrganization" />
                </p>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="DropDownListItem" CssClass="col-md-2 control-label">Item</asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList ID="DropDownListItem" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="DropDownListAddReusableOrganization" CssClass="col-md-2 control-label">Organization</asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList ID="DropDownListAddReusableOrganization" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server" ID="ButtonAddRelationship" OnClick="ButtonAddRelationship_Click" Text="Add Relationship" CssClass="btn btn-default" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
