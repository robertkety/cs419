<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageOrganizations.aspx.cs" Inherits="CRRD_Web_Interface.ManageOrganizations" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/WebForms/ManageOrganizations.js" type="text/javascript"></script>
    <h2>Manage Organizations</h2>
    <asp:Panel ID="PanelErrorMessages" runat="server" Visible="false">
        <asp:Label ID="LabelErrorMessageConnection" runat="server" Text="Unable to connect with database, please try again later."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelOrganizationInfo" runat="server" Visible="false">
        <h3>Organization Info</h3>
        <p class="text-danger">
            <asp:Literal runat="server" ID="LiteralErrorMessageGridView" />
        </p>
        <asp:GridView ID="GridViewOrganizationInfo" runat="server" AutoGenerateColumns="False" OnRowEditing="GridViewOrganizationInfo_RowEditing" 
            OnRowCancelingEdit="GridViewOrganizationInfo_RowCancelingEdit" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True"
            OnPageIndexChanged="GridViewOrganizationInfo_PageIndexChanged" OnPageIndexChanging="GridViewOrganizationInfo_PageIndexChanging" ShowFooter="true"
            OnRowDeleting="GridViewOrganizationInfo_RowDeleting" OnRowUpdating="GridViewOrganizationInfo_RowUpdating">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Organization ID" Visible="false">
                    <ItemTemplate>
                        <%# Eval("Id") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Organization Name">
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxEditName" Width="100%" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click"/>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Left" />
                    <HeaderStyle Width="30%" />
                </asp:TemplateField>
                <asp:BoundField DataField="Phone" HeaderText="Phone" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="AddressLine1" HeaderText="Address 1" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="AddressLine2" HeaderText="Address 2" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="AddressLine3" HeaderText="Address 3" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="ZipCode" HeaderText="Zip Code" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="Website" HeaderText="Website" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="Hours" HeaderText="Hours" >
                <ControlStyle Width="100%" />
                <HeaderStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="Notes" HeaderText="Notes" >
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
            <asp:LinkButton ID="LinkButtonAddOrganization" runat="server" OnClick="LinkButtonAddOrganization_Click">+ Add a New Organization</asp:LinkButton>
        </div>
        <asp:Panel ID="PanelAddOrganization" runat="server" Visible="false">
            <div class="form-horizontal">
                <h4>Create a New Organization</h4>
                <p class="text-danger">
                    <asp:Literal runat="server" ID="LiteralErrorMessageAddOrganization" />
                </p>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxName" CssClass="col-md-2 control-label">Name</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxName" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxPhone" CssClass="col-md-2 control-label">Phone</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxPhone" CssClass="form-control" TextMode="Phone" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxAddress1" CssClass="col-md-2 control-label">Address 1</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxAddress1" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxAddress2" CssClass="col-md-2 control-label">Address 2</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxAddress2" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxAddress3" CssClass="col-md-2 control-label">Address 3</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxAddress3" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxZipCode" CssClass="col-md-2 control-label">Zip Code</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxZipCode" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxWebsite" CssClass="col-md-2 control-label">Website</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxWebsite" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxHours" CssClass="col-md-2 control-label">Hours</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxHours" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TextBoxNotes" CssClass="col-md-2 control-label">Notes</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TextBoxNotes" CssClass="form-control" TextMode="MultiLine" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server" ID="ButtonAddOrganization" OnClick="ButtonAddOrganization_Click" Text="Add Organization" CssClass="btn btn-default" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
