using System;
using System.Collections.Generic;
using System.Linq;
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
                client.BaseAddress = new Uri(DataAccess.url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/items");
                if (response.IsSuccessStatusCode)
                {
                    // How to sort list of objects: http://stackoverflow.com/questions/1301822/how-to-sort-an-array-of-object-by-a-specific-field-in-c
                    List<Item> items = await response.Content.ReadAsAsync<List<Item>>();
                    items.Sort(new Comparison<Item>((x, y) => string.Compare(x.Name, y.Name)));

                    foreach (Item item in items)
                    {
                        DropDownListRepairableItems.Items.Add(new ListItem(item.Name, item.Id));
                        DropDownListItem.Items.Add(new ListItem(item.Name, item.Id));
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

        /*
         * Usage: Binds organization data to gridview
         */
        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(DataAccess.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/Items/" + DropDownListRepairableItems.SelectedValue);
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                DataTable dt = new DataTable();
                dt.Columns.Add("OrganizationID");
                dt.Columns.Add("OrganizationName");
                dt.Columns.Add("OrganizationAddressLine1");

                foreach (Organization organization in organizations)
                {
                    if (organization.Offering == Enums.offering.recycle || organization.Offering == Enums.offering.both)
                    {
                        var dr = dt.NewRow();
                        dr["OrganizationID"] = organization.Id;
                        dr["OrganizationName"] = organization.Name;
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

        /*
         * Usage: Loads gridview content based on item drop down list
         */
        protected async void DropDownListRepairableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListRepairableItems.SelectedValue == "-1")
            {
                return;
            }

            StoreSearchTerm();

            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelRepairableOrganization.Visible = false;
            }
            else
            {
                PanelRepairableOrganization.Visible = true;
                RestoreSearchTerm();
            }
        }

        /*
         * Usage: Implements page indexing for manual gridview
         */
        protected void GridViewRepairableOrganizations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRepairableOrganizations.PageIndex = e.NewPageIndex;
            GridViewRepairableOrganizations.EditIndex = -1;
            GridViewRepairableOrganizations.SelectedIndex = -1;
        }

        /*
         * Usage: Implements page indexing for manual gridview
         */
        protected async void GridViewRepairableOrganizations_PageIndexChanged(object sender, EventArgs e)
        {
            StoreSearchTerm();

            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
            else
            {
                RestoreSearchTerm();
            }
        }

        /*
         * Usage: Implements gridview search feature
         */
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

        /*
         * Usage: Make relationship creation panel visible/not visible
         */
        protected async void LinkButtonAddRepairable_Click(object sender, EventArgs e)
        {
            if (PanelAddRepairable.Visible == true)
            {
                LinkButtonAddRepairable.Text = "+ Add a New Item/Organization Combination";
                PanelAddRepairable.Visible = false;
            }
            else
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(DataAccess.url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/organizations/");
                if (response.IsSuccessStatusCode)
                {
                    // How to sort list of objects: http://stackoverflow.com/questions/1301822/how-to-sort-an-array-of-object-by-a-specific-field-in-c
                    List<Organization> organizations = await response.Content.ReadAsAsync<List<Organization>>();
                    organizations.Sort(new Comparison<Organization>((x, y) => string.Compare(x.Name, y.Name)));

                    foreach (Organization organization in organizations)
                    {
                        DropDownListAddRepairableOrganization.Items.Add(new ListItem(organization.Name + " (" + organization.AddressLine1 + ")", organization.Id));
                    }
                }

                LinkButtonAddRepairable.Text = "- Add a New Item/Organization Combination";
                PanelAddRepairable.Visible = true;
            }
        }

        /*
        * Usage: Add repairable relationship between organization and item
        */
        protected async void ButtonAddRelationship_Click(object sender, EventArgs e)
        {
            string ItemID = DropDownListItem.SelectedValue;
            string OrganizationID = DropDownListAddRepairableOrganization.SelectedValue;
            Enums.offering Offering = Enums.offering.none;

            if (ItemID == "")
            {
                LiteralErrorMessageAddRepairable.Text = "Item must be slected from drop-down list.";
                return;
            }
            if (OrganizationID == "")
            {
                LiteralErrorMessageAddRepairable.Text = "Organization must be selected from drop-down list.";
                return;
            }

            var client = new HttpClient();
            StringContent ContentString = new StringContent("");
            client.BaseAddress = new Uri(DataAccess.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get the exisiting offering for the item/organization relationship
            HttpResponseMessage response = await client.GetAsync("api/items/" + ItemID);
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();

                foreach (Organization organization in organizations)
                {
                    if (organization.Id == OrganizationID)
                    {
                        Offering = organization.Offering;
                    }
                }
            }
            else
            {
                LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
            }

            // Build Parameter
            string Parameter = DataAccess.url + "api/itemorganization?ItemId=" + ItemID + "&OrganizationId=" + OrganizationID + "&Offering=";

            // Post if offering is 0, put if offering is 1 (reusable), error if repairable relationship already exists
            if (Offering == 0)
            {
                DataAccess.postDataToService(Parameter + "2", ("").ToCharArray());
            }
            else if (Offering == Enums.offering.reuse)
            {
                DataAccess.putDataToService(Parameter + "3", ("").ToCharArray());
            }
            else
            {
                LiteralErrorMessageAddRepairable.Text = "Item already has a repairable relationship with selected organization.";
                return;
            }
        }

        /*
         * Usage: Delete repairable relationship between organization and item
        */
        protected async void GridViewOrganizationInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Enums.offering Offering = Enums.offering.none;
            string ItemId = DropDownListRepairableItems.SelectedValue;
            await BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewRepairableOrganizations.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string OrganizationId = dt.Rows[e.RowIndex][0] as string;

            // Get the existing offering between the item and organization
            var client = new HttpClient();
            StringContent ContentString = new StringContent("");
            client.BaseAddress = new Uri(DataAccess.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/items/" + ItemId);
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();

                foreach (Organization organization in organizations)
                {
                    if (organization.Id == OrganizationId)
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

            // Build Parameter
            string Parameter = DataAccess.url + "api/itemorganization?ItemId=" + ItemId + "&OrganizationId=" + OrganizationId + "&Offering=";

            // If reusable relationship exists, PUT with offering of 1, else PUT with offering of 0
            if (Offering == Enums.offering.both)
            {
                DataAccess.putDataToService(Parameter + "1", ("").ToCharArray());
            }
            else
            {
                DataAccess.putDataToService(Parameter + "0", ("").ToCharArray());
            }
        }

        protected void StoreSearchTerm()
        {
            // Retrieve the search box text for upcomming data bind
            if (GridViewRepairableOrganizations.Rows.Count > 0)
            {
                TextBox Search = GridViewRepairableOrganizations.FooterRow.FindControl("TextBoxSearch") as TextBox;
                SearchString = Search.Text;
            }
        }

        protected void RestoreSearchTerm()
        {
            // Repopulate search box with search string
            if (GridViewRepairableOrganizations.Rows.Count > 0)
            {
                TextBox Search = GridViewRepairableOrganizations.FooterRow.FindControl("TextBoxSearch") as TextBox;
                Search.Text = SearchString;
            }
        }
    }
}