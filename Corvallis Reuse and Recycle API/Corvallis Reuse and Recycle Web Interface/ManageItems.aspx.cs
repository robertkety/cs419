using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CRRD_Web_Interface.Models;
using System.Data;

// Manual grid view implementation borrowed from: http://aarongoldenthal.com/post/2009/04/19/Manually-Databinding-a-GridView.aspx
// Sorting data table: http://stackoverflow.com/questions/9107916/sorting-rows-in-a-data-table
namespace CRRD_Web_Interface
{
    public partial class ManageItems : System.Web.UI.Page
    {
        protected string SortExpression
        {
            get
            {
                return ViewState["SortExpression"] as string;
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        protected SortDirection SortDirection
        {
            get
            {
                object o = ViewState["SortDirection"];

                if (o == null)
                {
                    return SortDirection.Ascending;
                }
                else
                {
                    return SortDirection.Descending;
                }
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/items");
                if (response.IsSuccessStatusCode)
                {
                    Item[] items = await response.Content.ReadAsAsync<Item[]>();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ItemName");

                    foreach (Item item in items)
                    {
                        var dr = dt.NewRow();
                        dr["ItemName"] = item.RowKey;
                        dt.Rows.Add(dr);

                        DropDownListReusableItems.Items.Add(new ListItem(item.RowKey, item.PartitionKey));
                        DropDownListRepairableItems.Items.Add(new ListItem(item.RowKey, item.PartitionKey));
                    }

                    DataView dv = dt.DefaultView;
                    dv.Sort = "ItemName ASC";
                    DataTable dt_sorted = dv.ToTable();

                    GridViewItemInfo.DataSource = dt_sorted;
                    GridViewItemInfo.DataBind();
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelItemInfo.Visible = false;
                    PanelReusableItems.Visible = false;
                    PanelRepairableItems.Visible = false;
                }

                PanelErrorMessages.Visible = false;
                PanelItemInfo.Visible = true;
                PanelReusableItems.Visible = true;
                PanelRepairableItems.Visible = true;
            }
        }

        protected async Task<bool> BindItemData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/items");
            if (response.IsSuccessStatusCode)
            {
                Item[] items = await response.Content.ReadAsAsync<Item[]>();
                DataTable dt = new DataTable();
                bool sortAscending = this.SortDirection == SortDirection.Ascending ? true : false;
                dt.Columns.Add("ItemName");

                foreach (Item item in items)
                {
                    var dr = dt.NewRow();
                    dr["ItemName"] = item.RowKey;
                    dt.Rows.Add(dr);
                }

                DataView dv = dt.DefaultView;
                switch (this.SortExpression)
                {
                    case "ItemName":
                        dv.Sort = "ItemName ASC";
                        break;
                    default:
                        dv.Sort = "ItemName ASC";
                        break;
                }

                DataTable sorted_dt = dv.ToTable();
                GridViewItemInfo.DataSource = sorted_dt;
                GridViewItemInfo.DataBind();

                return true;
            }

            return false;
        }

        protected async Task<bool> BindOrganizationData(string Extension, GridView GV, int Offering)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(Extension);
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                DataTable dt = new DataTable();
                bool sortAscending = this.SortDirection == SortDirection.Ascending ? true : false;
                dt.Columns.Add("OrganizationName");
                dt.Columns.Add("OrganizationAddressLine1");
                dt.Columns.Add("OrganizationAddressLine2");
                dt.Columns.Add("OrganizationAddressLine3");

                foreach (Organization organization in organizations)
                {
                    if(Offering == 1 && organization.Offering == 1)
                    {
                        var dr = dt.NewRow();
                        dr["OrganizationName"] = organization.RowKey;
                        dr["OrganizationAddressLine1"] = organization.AddressLine1;
                        dr["OrganizationAddressLine2"] = organization.AddressLine2;
                        dr["OrganizationAddressLine3"] = organization.AddressLine3;
                        dt.Rows.Add(dr);
                    }
                    else if(Offering == 2 && organization.Offering == 2)
                    {
                        var dr = dt.NewRow();
                        dr["OrganizationName"] = organization.RowKey;
                        dr["OrganizationAddressLine1"] = organization.AddressLine1;
                        dr["OrganizationAddressLine2"] = organization.AddressLine2;
                        dr["OrganizationAddressLine3"] = organization.AddressLine3;
                        dt.Rows.Add(dr);
                    }
                    else if(organization.Offering == 3)
                    {
                        var dr = dt.NewRow();
                        dr["OrganizationName"] = organization.RowKey;
                        dr["OrganizationAddressLine1"] = organization.AddressLine1;
                        dr["OrganizationAddressLine2"] = organization.AddressLine2;
                        dr["OrganizationAddressLine3"] = organization.AddressLine3;
                        dt.Rows.Add(dr);
                    }
                }

                DataView dv = dt.DefaultView;
                switch (this.SortExpression)
                {
                    case "OrganizationName":
                        dv.Sort = "OrganizationName ASC";
                        break;
                    case "OrganizationAddressLine1":
                        dv.Sort = "OrganizationAddressLine1 ASC";
                        break;
                    case "OrganizationAddressLine2":
                        dv.Sort = "OrganizationAddressLine2 ASC";
                        break;
                    case "OrganizationAddressLine3":
                        dv.Sort = "OrganizationAddressLine3 ASC";
                        break;
                    default:
                        dv.Sort = "OrganizationName ASC";
                        break;
                }

                DataTable sorted_dt = dv.ToTable();
                GV.DataSource = sorted_dt;
                GV.DataBind();

                return true;
            }

            return false;
        }

        protected async void GridViewItemInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewItemInfo.EditIndex = e.NewEditIndex;
            bool status = await BindItemData();

            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
        }

        protected async void GridViewItemInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewItemInfo.EditIndex = -1;
            bool status = await BindItemData();

            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
        }

        protected void GridViewItemInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewItemInfo.PageIndex = e.NewPageIndex;
            GridViewItemInfo.EditIndex = -1;
            GridViewItemInfo.SelectedIndex = -1;
        }

        protected async void GridViewItemInfo_PageIndexChanged(object sender, EventArgs e)
        {
            await BindItemData();
        }

        protected void GridViewItemInfo_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (this.SortExpression == e.SortExpression)
            {
                this.SortDirection = this.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            }
            else
            {
                this.SortDirection = SortDirection.Ascending;
            }

            this.SortExpression = e.SortExpression;

            GridViewItemInfo.EditIndex = -1;
            GridViewItemInfo.SelectedIndex = -1;
        }

        protected async void DropDownListReusableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListReusableItems.SelectedValue == "-1")
            {
                return;
            }

            bool status = await BindOrganizationData("api/items/" + DropDownListReusableItems.SelectedValue, GridViewReusableItems, 1);
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelItemInfo.Visible = false;
                PanelReusableItems.Visible = false;
                PanelRepairableItems.Visible = false;
            }
        }

        protected async void DropDownListRepairableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListRepairableItems.SelectedValue == "-1")
            {
                return;
            }

            bool status = await BindOrganizationData("api/items/" + DropDownListRepairableItems.SelectedValue, GridViewRepairableItems, 2);
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelItemInfo.Visible = false;
                PanelReusableItems.Visible = false;
                PanelRepairableItems.Visible = false;
            }
        }

        protected void GridViewReusableItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewReusableItems.PageIndex = e.NewPageIndex;
            GridViewReusableItems.EditIndex = -1;
            GridViewReusableItems.SelectedIndex = -1;
        }

        protected async void GridViewReusableItems_PageIndexChanged(object sender, EventArgs e)
        {
            await BindOrganizationData("api/items/" + DropDownListReusableItems.SelectedValue, GridViewReusableItems, 1);
        }

        protected void GridViewReusableItems_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (this.SortExpression == e.SortExpression)
            {
                this.SortDirection = this.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            }
            else
            {
                this.SortDirection = SortDirection.Ascending;
            }

            this.SortExpression = e.SortExpression;

            GridViewReusableItems.EditIndex = -1;
            GridViewReusableItems.SelectedIndex = -1;
        }

        protected void GridViewRepairableItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRepairableItems.PageIndex = e.NewPageIndex;
            GridViewRepairableItems.EditIndex = -1;
            GridViewRepairableItems.SelectedIndex = -1;
        }

        protected async void GridViewRepairableItems_PageIndexChanged(object sender, EventArgs e)
        {
            await BindOrganizationData("api/items/" + DropDownListRepairableItems.SelectedValue, GridViewRepairableItems, 2);
        }

        protected void GridViewRepairableItems_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (this.SortExpression == e.SortExpression)
            {
                this.SortDirection = this.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            }
            else
            {
                this.SortDirection = SortDirection.Ascending;
            }

            this.SortExpression = e.SortExpression;

            GridViewRepairableItems.EditIndex = -1;
            GridViewRepairableItems.SelectedIndex = -1;
        }
    }
}