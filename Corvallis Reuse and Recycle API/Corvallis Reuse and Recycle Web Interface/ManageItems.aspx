<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageItems.aspx.cs" Inherits="CRRD_Web_Interface.ManageItems" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Items</h2>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelItemInfo" runat="server" Visible="false">
        <h3>Items</h3>
        <asp:GridView ID="GridViewItemInfo" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowEditing="GridViewItemInfo_RowEditing"
            OnRowCancelingEdit="GridViewItemInfo_RowCancelingEdit" AllowPaging="True" AllowSorting="True" OnPageIndexChanged="GridViewItemInfo_PageIndexChanged" 
            OnPageIndexChanging="GridViewItemInfo_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="ItemName" HeaderText="Name" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="40%" />
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
            <asp:LinkButton ID="LinkButtonAddItem" runat="server">+ Add a New Item</asp:LinkButton>
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelReusableItems" runat="server">
        <h3>Reusable Item/Organization Combinations</h3>
        <asp:DropDownList ID="DropDownListReusableItems" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListReusableItems_SelectedIndexChanged">
            <asp:ListItem Value="-1">&lt;Select an Item&gt;</asp:ListItem>
        </asp:DropDownList>
        <asp:GridView ID="GridViewReusableItems" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            AllowPaging="True" AllowSorting="True" OnPageIndexChanged="GridViewReusableItems_PageIndexChanged" 
            OnPageIndexChanging="GridViewReusableItems_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="OrganizationName" HeaderText="Name" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine1" HeaderText="Address 1" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine2" HeaderText="Address 2" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine3" HeaderText="Address 3" >
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
            <asp:LinkButton ID="LinkButtonAddReusable" runat="server">+ Add a New Item/Organization Combination</asp:LinkButton>
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelRepairableItems" runat="server">
        <h3>Repairable Item/Organization Combinations</h3>
        <asp:DropDownList ID="DropDownListRepairableItems" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListRepairableItems_SelectedIndexChanged">
            <asp:ListItem Value="-1">&lt;Select an Item&gt;</asp:ListItem>
        </asp:DropDownList>
        <asp:GridView ID="GridViewRepairableItems" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            AllowPaging="True" AllowSorting="True" OnPageIndexChanged="GridViewRepairableItems_PageIndexChanged" 
            OnPageIndexChanging="GridViewRepairableItems_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="OrganizationName" HeaderText="Name" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine1" HeaderText="Address 1" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine2" HeaderText="Address 2" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrganizationAddressLine3" HeaderText="Address 3" >
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
