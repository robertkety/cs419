using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using CRRD_Web_Interface.Models;

namespace CRRD_Web_Interface
{
    public partial class ManageItemOrganization : System.Web.UI.Page
    {
        protected string SearchString;
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

            if(!IsPostBack && Authenticated == true)
            {
                Session["SearchEnabled"] = false; 

                bool result = false;
                try
                {
                    result = await LoadItems();
                }
                catch(Exception ex) {}
                if(result == false)
                {
                    PanelErrorMessages.Visible = true;
                    PanelItemOrganization.Visible = false;
                }
                else
                {
                    PanelItemOrganization.Visible = true;
                    PanelErrorMessages.Visible = false;
                }
            }   
        }

        protected async Task<bool> LoadItems()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(DataAccess.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/items/");
            if (response.IsSuccessStatusCode)
            {
                List<Item> items = await response.Content.ReadAsAsync<List<Item>>();
                items.Sort(new Comparison<Item>((x, y) => string.Compare(x.RowKey, y.RowKey)));
                DropDownListItems.Items.Add(new ListItem("<Select an Item>", "-1"));

                foreach (Item item in items)
                {
                    DropDownListItems.Items.Add(new ListItem(item.RowKey, item.PartitionKey));
                }

                return true;
            }
            
            return false;
        }

        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(DataAccess.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get organizations
            List<Organization> organizations;
            HttpResponseMessage response = await client.GetAsync("api/organizations/");
            if (response.IsSuccessStatusCode)
            {
                organizations = await response.Content.ReadAsAsync<List<Organization>>();
                organizations.Sort(new Comparison<Organization>((x, y) => string.Compare(x.RowKey, y.RowKey)));
            }
            else
            {
                return false;
            }

            // Get item organizations
            List<Organization> itemOrganizations;
            response = await client.GetAsync("api/items/" + DropDownListItems.SelectedValue);
            if (response.IsSuccessStatusCode)
            {
                    itemOrganizations = await response.Content.ReadAsAsync<List<Organization>>();
                    itemOrganizations.Sort(new Comparison<Organization>((x, y) => string.Compare(x.RowKey, y.RowKey)));

            }
            else
            {
                return false;
            }

            // Build table
            DataTable dt = new DataTable();
            dt.Columns.Add("OrganizationID");
            dt.Columns.Add("OrganizationName");
            dt.Columns.Add("Reusable");
            dt.Columns.Add("Repairable");

            Organization itemOrganization = itemOrganizations.FirstOrDefault();
            int index = 0;
            foreach (Organization organization in organizations)
            {
                var dr = dt.NewRow();

                dr["OrganizationID"] = organization.PartitionKey;
                dr["OrganizationName"] = organization.RowKey + " (" + organization.AddressLine1 + ")";
                
                if(organization.PartitionKey == itemOrganization.PartitionKey)
                {
                    switch (itemOrganization.Offering)
                    {
                        case 1:
                            dr["Reusable"] = true;
                            dr["Repairable"] = false;
                            break;
                        case 2:
                            dr["Reusable"] = false;
                            dr["Repairable"] = true;
                            break;
                        case 3:
                            dr["Reusable"] = true;
                            dr["Repairable"] = true;
                            break;
                        default:
                            break;
                    }

                    index++;
                    if(index < itemOrganizations.Count)
                    {
                        itemOrganization = itemOrganizations[index];
                    }
                }
                else
                {
                    dr["Reusable"] = false;
                    dr["Repairable"] = false;
                }

                dt.Rows.Add(dr);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "OrganizationName ASC";
            DataTable sorted_dt = dv.ToTable();

            // See if search is enabled
            bool SearchEnabled = false;
            try
            {
                SearchEnabled = (bool)Session["SearchEnabled"];
            }
            catch (Exception ex) { }

            if (SearchEnabled)
            {
                DataRow[] FilteredRows = sorted_dt.Select("OrganizationName like '%" + SearchString + "%'");
                DataTable filtered_dt = new DataTable();
                filtered_dt = sorted_dt.Clone();

                if (FilteredRows.Count() == 0)
                {
                    GridViewItemOrganization.DataSource = sorted_dt;
                    GridViewItemOrganization.DataBind();
                    return true;
                }

                foreach (DataRow row in FilteredRows)
                {
                    filtered_dt.Rows.Add(row.ItemArray);
                }
                GridViewItemOrganization.DataSource = filtered_dt;
                GridViewItemOrganization.DataBind();
                return true;
            }

            GridViewItemOrganization.DataSource = sorted_dt;
            GridViewItemOrganization.DataBind();

            return true;
        }

        protected async void DropDownListOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(DropDownListItems.SelectedValue == "-1")
            {
                return;
            }

            bool result = false;
            try
            {
                result = await BindData();
            }
            catch (Exception ex) { }
            if (result == false)
            {
                PanelErrorMessages.Visible = true;
                PanelItemOrganization.Visible = false;
            }
            else
            {
                PanelItemOrganization.Visible = true;
                PanelErrorMessages.Visible = false;
            }
        }

        protected async void GridViewItemOrganization_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewItemOrganization.EditIndex = e.NewEditIndex;
            StoreSearchTerm();

            bool result = false;
            try
            {
                result = await BindData();
            }
            catch (Exception ex) { }
            if (result == false)
            {
                PanelErrorMessages.Visible = true;
                PanelItemOrganization.Visible = false;
            }
            else
            {
                PanelItemOrganization.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        protected async void GridViewItemOrganization_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewItemOrganization.EditIndex = -1;
            StoreSearchTerm();

            bool result = false;
            try
            {
                result = await BindData();
            }
            catch (Exception ex) { }
            if (result == false)
            {
                PanelErrorMessages.Visible = true;
                PanelItemOrganization.Visible = false;
            }
            else
            {
                PanelItemOrganization.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        protected void GridViewItemOrganization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewItemOrganization.PageIndex = e.NewPageIndex;
            GridViewItemOrganization.EditIndex = -1;
            GridViewItemOrganization.SelectedIndex = -1;
        }

        protected async void GridViewItemOrganization_PageIndexChanged(object sender, EventArgs e)
        {
            StoreSearchTerm();

            bool result = false;
            try
            {
                result = await BindData();
            }
            catch (Exception ex) { }
            if (result == false)
            {
                PanelErrorMessages.Visible = true;
                PanelItemOrganization.Visible = false;
            }
            else
            {
                PanelItemOrganization.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            StoreSearchTerm();
            SetSearchStatus();

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

        protected async void GridViewItemOrganization_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            StoreSearchTerm();

            // Get input values
            bool Reusable = ((CheckBox)(GridViewItemOrganization.Rows[e.RowIndex].Cells[2].Controls[0])).Checked;
            bool Repairable = ((CheckBox)(GridViewItemOrganization.Rows[e.RowIndex].Cells[3].Controls[0])).Checked;
            string ItemID = DropDownListItems.SelectedValue;

            // Must bind data to get organization's ID
            try
            {
                await BindData();
            }
            catch (Exception ex) 
            {
                PanelErrorMessages.Visible = true;
                PanelItemOrganization.Visible = false;
                return;
            }


            // Get organization ID
            DataTable dt = (DataTable)GridViewItemOrganization.DataSource;
            string OrganizationID = dt.Rows[(10 * GridViewItemOrganization.PageIndex) + e.RowIndex][0] as String;
            string OldName = dt.Rows[(10 * GridViewItemOrganization.PageIndex) + e.RowIndex][1] as String;

            // Calculate offering
            string Offering;
            if(Reusable && Repairable)
            {
                Offering = "3";
            }
            else if(Reusable)
            {
                Offering = "1";
            }
            else if(Repairable)
            {
                Offering = "2";
            }
            else
            {
                Offering = "0";
            }

            // Build parameter string
            string ParameterString = "?ItemId=" + ItemID;
            ParameterString += "&OrganizationId=" + OrganizationID;
            ParameterString += "&Offering=" + Offering;


            // Atempt PUT
            var result = DataAccess.putDataToService(DataAccess.url + "api/ItemOrganization" + ParameterString, ("").ToCharArray());

            // Cancel row edit (cancelling will call bind and show the updated data)
            RestoreSearchTerm();
            GridViewItemOrganization_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(e.RowIndex));
        }

        protected void StoreSearchTerm()
        {
            // Retrieve the search box text for upcomming data bind
            try
            {
                TextBox Search = GridViewItemOrganization.FooterRow.FindControl("TextBoxSearch") as TextBox;
                SearchString = Search.Text;
            }
            catch(Exception ex) {}
        }

        protected void RestoreSearchTerm()
        {
            // Repopulate search box with search string
            try
            {
                TextBox Search = GridViewItemOrganization.FooterRow.FindControl("TextBoxSearch") as TextBox;
                Search.Text = SearchString;
            }
            catch (Exception ex) {}
        }

        protected void SetSearchStatus()
        {
            try
            {
                TextBox Search = GridViewItemOrganization.FooterRow.FindControl("TextBoxSearch") as TextBox;
                if (Search.Text == "")
                {
                    Session["SearchEnabled"] = false;
                }
                else
                {
                    Session["SearchEnabled"] = true;
                }
            }
            catch (Exception ex) { }
        }
    }
}