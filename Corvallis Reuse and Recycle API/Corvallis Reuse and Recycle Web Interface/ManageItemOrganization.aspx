<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageItemOrganization.aspx.cs" Inherits="CRRD_Web_Interface.ManageItemOrganization" async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelItemOrganization" runat="server" Visible="false">
        <h3>Item/Organization Relationships</h3>
        <p class="text-danger">
            <asp:Literal runat="server" ID="LiteralErrorMessageGridView" />
        </p>
        <asp:DropDownList ID="DropDownListItems" runat="server" OnSelectedIndexChanged="DropDownListOrganizations_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
        <asp:GridView ID="GridViewItemOrganization" runat="server" AutoGenerateColumns="False" AllowPaging="True" CellPadding="4" ForeColor="#333333" 
            GridLines="None" OnPageIndexChanged="GridViewItemOrganization_PageIndexChanged" OnPageIndexChanging="GridViewItemOrganization_PageIndexChanging"
            ShowFooter="True" OnRowCancelingEdit="GridViewItemOrganization_RowCancelingEdit" OnRowEditing="GridViewItemOrganization_RowEditing" OnRowUpdating="GridViewItemOrganization_RowUpdating"
            >
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Organization ID" Visible="false">
                    <ItemTemplate>
                        <%# Eval("OrganizationID") %>
                    </ItemTemplate>
                    <ItemStyle Width="40%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Organization" Visible="true">
                    <ItemTemplate>
                        <%# Eval("OrganizationName") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click"/>
                    </FooterTemplate>
                    <HeaderStyle Width="40%" />
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Reusable" HeaderText="Reusable"/>
                <asp:CheckBoxField DataField="Repairable" HeaderText="Repairable" />
                <asp:CommandField ShowEditButton="True" />
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
