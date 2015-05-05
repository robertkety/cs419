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
    public partial class ManageRepairables : System.Web.UI.Page
    {
        protected string SearchString = String.Empty;
        protected bool Authenticated = false;   // Flag to prevent the rest of the page being rendered when user is not authenitcated

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login", false);
                Context.ApplicationInstance.CompleteRequest();  // Most complete thread to avoid thread exit exception
            }
            else
            {
                Authenticated = true;
            }

            if (!IsPostBack && Authenticated == true)
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

                        DropDownListRepairableItems.Items.Add(new ListItem(item.RowKey, item.PartitionKey));
                        PanelErrorMessages.Visible = false;
                    }
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelRepairableOrganization.Visible = false;
                }
            }
        }

        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/ItemOrganization?ItemId=" + DropDownListRepairableItems.SelectedValue + "&Offering=true");
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                DataTable dt = new DataTable();
                dt.Columns.Add("OrganizationName");
                dt.Columns.Add("OrganizationAddressLine1");

                foreach (Organization organization in organizations)
                {
                    if (organization.Offering == 2 || organization.Offering == 3)
                    {
                        var dr = dt.NewRow();
                        dr["OrganizationName"] = organization.RowKey;
                        dr["OrganizationAddressLine1"] = organization.AddressLine1;
                        dt.Rows.Add(dr);
                    }
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "OrganizationName ASC";
                DataTable sorted_dt = dv.ToTable();

                if (SearchString != "")
                {
                    DataRow[] FilteredRows = sorted_dt.Select("OrganizationName like '%" + SearchString + "%'");
                    DataTable filtered_dt = new DataTable();
                    filtered_dt = sorted_dt.Clone();

                    if (FilteredRows.Count() == 0)
                    {
                        GridViewRepairableOrganizations.DataSource = sorted_dt;
                        GridViewRepairableOrganizations.DataBind();
                        return true;
                    }

                    foreach (DataRow row in FilteredRows)
                    {
                        filtered_dt.Rows.Add(row.ItemArray);
                    }

                    GridViewRepairableOrganizations.DataSource = filtered_dt;
                    GridViewRepairableOrganizations.DataBind();
                    return true;
                }

                GridViewRepairableOrganizations.DataSource = sorted_dt;
                GridViewRepairableOrganizations.DataBind();

                return true;
            }

            return false;
        }

        protected async void DropDownListRepairableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListRepairableItems.SelectedValue == "-1")
            {
                return;
            }

            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelRepairableOrganization.Visible = false;
            }
            else
            {
                PanelRepairableOrganization.Visible = true;
            }
        }

        protected void GridViewRepairableOrganizations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRepairableOrganizations.PageIndex = e.NewPageIndex;
            GridViewRepairableOrganizations.EditIndex = -1;
            GridViewRepairableOrganizations.SelectedIndex = -1;
        }

        protected async void GridViewRepairableOrganizations_PageIndexChanged(object sender, EventArgs e)
        {
            await BindData();
        }

        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            TextBox Search = GridViewRepairableOrganizations.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelRepairableOrganization.Visible = false;
            }
            else
            {
                PanelRepairableOrganization.Visible = true;
            }
        }
    }
}