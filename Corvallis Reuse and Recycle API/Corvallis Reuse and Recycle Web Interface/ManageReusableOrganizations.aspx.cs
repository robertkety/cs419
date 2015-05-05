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
    public partial class ManageReusableOrganizations : System.Web.UI.Page
    {
        protected string SearchString = String.Empty;
        protected bool Authenticated = false;   // Flag to prevent the rest of the page being rendered when user is not authenitcated

        protected async Task Page_Load(object sender, EventArgs e)
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

                        DropDownListReusableItems.Items.Add(new ListItem(item.RowKey, item.PartitionKey));
                        PanelErrorMessages.Visible = false;
                    }
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelReusableOrganization.Visible = false;
                }
            }
        }

        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/ItemOrganization?ItemId=" + DropDownListReusableItems.SelectedValue + "&Offering=true");
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                DataTable dt = new DataTable();
                dt.Columns.Add("OrganizationName");
                dt.Columns.Add("OrganizationAddressLine1");

                foreach (Organization organization in organizations)
                {
                    if (organization.Offering == 1 || organization.Offering == 3)
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
                        GridViewReusableOrganizations.DataSource = sorted_dt;
                        GridViewReusableOrganizations.DataBind();
                        return true;
                    }

                    foreach (DataRow row in FilteredRows)
                    {
                        filtered_dt.Rows.Add(row.ItemArray);
                    }

                    GridViewReusableOrganizations.DataSource = filtered_dt;
                    GridViewReusableOrganizations.DataBind();
                    return true;
                }

                GridViewReusableOrganizations.DataSource = sorted_dt;
                GridViewReusableOrganizations.DataBind();

                return true;
            }

            return false;
        }

        protected async void DropDownListReusableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListReusableItems.SelectedValue == "-1")
            {
                return;
            }

            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelReusableOrganization.Visible = false;
            }
            else
            {
                PanelReusableOrganization.Visible = true;
            }
        }

        protected void GridViewReusableOrganizations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewReusableOrganizations.PageIndex = e.NewPageIndex;
            GridViewReusableOrganizations.EditIndex = -1;
            GridViewReusableOrganizations.SelectedIndex = -1;
        }

        protected async void GridViewReusableOrganizations_PageIndexChanged(object sender, EventArgs e)
        {
            await BindData();
        }

        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            TextBox Search = GridViewReusableOrganizations.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelReusableOrganization.Visible = false;
            }
            else
            {
                PanelReusableOrganization.Visible = true;
            }
        }
    }
}