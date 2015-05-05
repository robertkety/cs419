﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageRepairables.aspx.cs" MasterPageFile="~/Site.Master" Inherits="CRRD_Web_Interface.ManageRepairables" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Repairable Item/Organization Combinations</h2>
    <asp:Label ID="LabelInstruction" runat="server" Text="Begin by selecting an item:"></asp:Label>
    <asp:DropDownList ID="DropDownListRepairableItems" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListRepairableItems_SelectedIndexChanged">
        <asp:ListItem Value="-1">&lt;Select an Item&gt;</asp:ListItem>
    </asp:DropDownList>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
     <asp:Panel ID="PanelRepairableOrganization" runat="server" Visible="false">
        <asp:GridView ID="GridViewRepairableOrganizations" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            AllowPaging="True" OnPageIndexChanged="GridViewRepairableOrganizations_PageIndexChanged" 
            OnPageIndexChanging="GridViewRepairableOrganizations_PageIndexChanging" ShowFooter="True">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
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
            <asp:LinkButton ID="LinkButtonAddRepairable" runat="server">+ Add a New Item/Organization Combination</asp:LinkButton>
        </div>
    </asp:Panel>
</asp:Content>
