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

        /*
         * Usage: Binds organization data to gridview
         */
        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/items/" + DropDownListReusableItems.SelectedValue);
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                DataTable dt = new DataTable();
                dt.Columns.Add("OrganizationID");
                dt.Columns.Add("OrganizationName");
                dt.Columns.Add("OrganizationAddressLine1");

                foreach (Organization organization in organizations)
                {
                    if (organization.Offering == 1 || organization.Offering == 3)
                    {
                        var dr = dt.NewRow();
                        dr["OrganizationID"] = organization.PartitionKey;
                        dr["OrganizationName"] = organization.RowKey;
                        dr["OrganizationAddressLine1"] = organization.AddressLine1;
                        dt.Rows.Add(dr);
                    }

                    DropDownListAddReusableOrganization.Items.Add(new ListItem(organization.RowKey, organization.PartitionKey));
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

        /*
         * Usage: Loads gridview content based on item drop down list
         */
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

        /*
         * Usage: Implements page indexing for manual gridview
         */
        protected void GridViewReusableOrganizations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewReusableOrganizations.PageIndex = e.NewPageIndex;
            GridViewReusableOrganizations.EditIndex = -1;
            GridViewReusableOrganizations.SelectedIndex = -1;
        }

        /*
         * Usage: Implements page indexing for manual gridview
         */
        protected async void GridViewReusableOrganizations_PageIndexChanged(object sender, EventArgs e)
        {
            await BindData();
        }

        /*
         * Usage: Implements gridview search feature
         */
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

        /*
         * Usage: Make relationship creation panel visible/not visible
         */
        protected async void LinkButtonAddReusable_Click(object sender, EventArgs e)
        {
            if (PanelAddReusable.Visible == true)
            {
                LinkButtonAddReusable.Text = "+ Add a New Item/Organization Combination";
                PanelAddReusable.Visible = false;
            }
            else
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/organizations/");
                if (response.IsSuccessStatusCode)
                {
                    Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();

                    foreach (Organization organization in organizations)
                    {
                        DropDownListAddReusableOrganization.Items.Add(new ListItem(organization.RowKey, organization.PartitionKey));
                    }
                }

                LinkButtonAddReusable.Text = "- Add a New Item/Organization Combination";
                PanelAddReusable.Visible = true;
            }
        }

        /*
         * Usage: Add reusable relationship between organization and item
         */
        protected async void ButtonAddRelationship_Click(object sender, EventArgs e)
        {
            string ItemID = DropDownListReusableItems.SelectedValue;
            string OrganizationID = DropDownListAddReusableOrganization.SelectedValue;
            int Offering = 0;

            if(ItemID == "" || ItemID == "-1")
            {
                LiteralErrorMessageAddOrganization.Text = "Item must be slected from drop-down list.";
            }
            if(OrganizationID == "")
            {
                LiteralErrorMessageAddOrganization.Text = "Organization must be selected from drop-down list.";
            }

            var client = new HttpClient();
            StringContent ContentString = new StringContent("");
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get the exisiting offering for the item/organization relationship
            HttpResponseMessage response = await client.GetAsync("api/items/" + ItemID);
            if(response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();

                foreach (Organization organization in organizations)
                {
                    if(organization.PartitionKey == OrganizationID)
                    {
                        Offering = organization.Offering;
                    }
                }
            }
            else
            {
                LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
            }

            // Post if offering is 0, put if offering is 2 (repairable), error if reusable relationship already exists
            if(Offering == 0)
            {
                response = await client.PostAsync("api/itemorganization?ItemId=" + ItemID + "&OrganizationId=" + OrganizationID + "&Offering=" + "1", ContentString);
                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect((Page.Request.Url.ToString()), false);
                }
                else
                {
                    LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                }
            }
            else if(Offering == 2)
            {
                response = await client.PutAsync("api/itemorganization?ItemId=" + ItemID + "&OrganizationId=" + OrganizationID + "&Offering=" + "3", ContentString);
                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect((Page.Request.Url.ToString()), false);
                }
                else
                {
                    LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                }
            }
            else
            {
                LiteralErrorMessageAddOrganization.Text = "Item already has a reusable relationship with selected organization.";
                return;
            }
        }

        /*
         * Usage: Delete reusable relationship between organization and item
         */
        protected async void GridViewOrganizationInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int Offering = 0;
            string ItemId = DropDownListReusableItems.SelectedValue;
            await BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewReusableOrganizations.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string OrganizationId = dt.Rows[e.RowIndex][0] as string;

            // Get the existing offering between the item and organization
            var client = new HttpClient();
            StringContent ContentString = new StringContent("");
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/items/" + ItemId);
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();

                foreach (Organization organization in organizations)
                {
                    if (organization.PartitionKey == OrganizationId)
                    {
                        Offering = organization.Offering;
                    }
                }
            }
            else
            {
                LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                return;
            }

            // If repairable relationship exists, PUT with offering of 2, else PUT with offering of 0
            if (Offering == 3)
            {
                response = await client.PutAsync("api/ItemOrganization?ItemId=" + ItemId + "&OrganizationId=" + OrganizationId + "&Offering=" + "2", ContentString);
                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect((Page.Request.Url.ToString()), false);
                }
                else
                {
                    LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                }
            }
            else
            {
                response = await client.PutAsync("api/ItemOrganization?ItemId=" + ItemId + "&OrganizationId=" + OrganizationId + "&Offering=" + "0", ContentString);
                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect((Page.Request.Url.ToString()), false);
                }
                else
                {
                    LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                }
            }
        }
    }
}