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
    public partial class ManageOrganizations : System.Web.UI.Page
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
                Session["SearchEnabled"] = false;   // Prevents text in search box from filtering results unless search is requested
                bool status = await BindData();
                if(status == true)
                {
                    PanelErrorMessages.Visible = false;
                    PanelOrganizationInfo.Visible = true;
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelOrganizationInfo.Visible = false;
                }
            }
        }

        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(DataAccess.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/organizations");
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                DataTable dt = new DataTable();
                dt.Columns.Add("OrganizationID");
                dt.Columns.Add("OrganizationName");
                dt.Columns.Add("OrganizationPhone");
                dt.Columns.Add("OrganizationAddressLine1");
                dt.Columns.Add("OrganizationAddressLine2");
                dt.Columns.Add("OrganizationAddressLine3");
                dt.Columns.Add("OrganizationZipCode");
                dt.Columns.Add("OrganizationWebsite");
                dt.Columns.Add("OrganizationHours");
                dt.Columns.Add("OrganizationNotes");

                foreach (Organization organization in organizations)
                {
                    var dr = dt.NewRow();
                    dr["OrganizationID"] = organization.PartitionKey;
                    dr["OrganizationName"] = organization.RowKey;
                    if(organization.Phone != null && organization.Phone != "")
                    {
                        dr["OrganizationPhone"] = organization.Phone;
                    }
                    else
                    {
                        dr["OrganizationPhone"] = "Private";
                    }
                    dr["OrganizationAddressLine1"] = organization.AddressLine1;
                    dr["OrganizationAddressLine2"] = organization.AddressLine2;
                    dr["OrganizationAddressLine3"] = organization.AddressLine3;
                    dr["OrganizationZipCode"] = organization.ZipCode;
                    dr["OrganizationWebsite"] = organization.Website;
                    dr["OrganizationHours"] = organization.Hours;
                    dr["OrganizationNotes"] = organization.Notes;
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

                    if(FilteredRows.Count() == 0)
                    {
                        GridViewOrganizationInfo.DataSource = sorted_dt;
                        GridViewOrganizationInfo.DataBind();
                        return true;
                    }

                   foreach(DataRow row in FilteredRows)
                   {
                       filtered_dt.Rows.Add(row.ItemArray);
                   }
                   GridViewOrganizationInfo.DataSource = filtered_dt;
                   GridViewOrganizationInfo.DataBind();
                   return true;
                }

                GridViewOrganizationInfo.DataSource = sorted_dt;
                GridViewOrganizationInfo.DataBind();

                return true;
            }

            return false;
        }

        protected async void GridViewOrganizationInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {

            GridViewOrganizationInfo.EditIndex = e.NewEditIndex;
            StoreSearchTerm();

            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
            else
            {
                // Populates the edit box with the old organization name
                DataTable dt = (DataTable)GridViewOrganizationInfo.DataSource;
                TextBox TextBoxEditOrganization = GridViewOrganizationInfo.Rows[e.NewEditIndex].FindControl("TextBoxEditOrganizationName") as TextBox;
                TextBoxEditOrganization.Text = dt.Rows[(10 * GridViewOrganizationInfo.PageIndex) + e.NewEditIndex][1].ToString();

                RestoreSearchTerm();
            }
        }

        protected async void GridViewOrganizationInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewOrganizationInfo.EditIndex = -1;
            LiteralErrorMessageGridView.Text = "";
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

        protected void GridViewOrganizationInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewOrganizationInfo.PageIndex = e.NewPageIndex;
            GridViewOrganizationInfo.EditIndex = -1;
            GridViewOrganizationInfo.SelectedIndex = -1;
        }

        protected async void GridViewOrganizationInfo_PageIndexChanged(object sender, EventArgs e)
        {
            StoreSearchTerm();
            LiteralErrorMessageGridView.Text = "";
            RestoreSearchTerm();
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

        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            StoreSearchTerm();
            SetSearchStatus();

            GridViewOrganizationInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(0));

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

        protected void LinkButtonAddOrganization_Click(object sender, EventArgs e)
        {
            GridViewOrganizationInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(0));

            if (PanelAddOrganization.Visible == true)
            {
                LinkButtonAddOrganization.Text = "+ Add a New Organization";
                PanelAddOrganization.Visible = false;
                ClearAddOrganizationInput();
            }
            else
            {
                LinkButtonAddOrganization.Text = "- Add a New Organization";
                PanelAddOrganization.Visible = true;
            }
        }

        protected void ButtonAddOrganization_Click(object sender, EventArgs e)
        {
            long PhoneNumberNumeric;
            string PhoneNumberString = TextBoxPhone.Text;

            // Validate input
            if (TextBoxName.Text == "")
            {
                LiteralErrorMessageAddOrganization.Text = "The organization name field is required.";
                return;
            }
            else if(ContainsSpecialCharacters(TextBoxName.Text))
            {
                LiteralErrorMessageAddOrganization.Text = "The organization name cannot contain special characters (/, \\, #, ?)";
                return;
            }
            else if (PhoneNumberString != "")
            {
                if (!Int64.TryParse(PhoneNumberString, out PhoneNumberNumeric))
                {
                    LiteralErrorMessageAddOrganization.Text = "The phone number cannot contain non-numeric characters";
                    return;
                }
            }

            // Build query
            String QueryString = "?Name=" + TextBoxName.Text;
            QueryString += "&Phone=" + TextBoxPhone.Text;
            QueryString += "&Address1=" + TextBoxAddress1.Text;
            QueryString += "&Address2=" + TextBoxAddress2.Text;
            QueryString += "&Address3=" + TextBoxAddress3.Text;
            QueryString += "&ZipCode=" + TextBoxZipCode.Text;
            QueryString += "&Website=" + TextBoxWebsite.Text;
            QueryString += "&Hours=" + TextBoxHours.Text;
            QueryString += "&Notes=" + TextBoxNotes.Text;

            // Attempt POST
            var result = DataAccess.postDataToService(DataAccess.url + "api/Organizations/" + QueryString, ("").ToCharArray());
            ClearAddOrganizationInput();
            Response.Redirect((Page.Request.Url.ToString()), false);
        }

        protected async void GridViewOrganizationInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            StoreSearchTerm();

            // Get edit text box values before call to data bind
            TextBox EditTextBox = GridViewOrganizationInfo.Rows[e.RowIndex].FindControl("TextBoxEditOrganizationName") as TextBox;
            string NewName = EditTextBox.Text;
            string Phone = ((TextBox)(GridViewOrganizationInfo.Rows[e.RowIndex].Cells[2].Controls[0])).Text;
            string Address1 = ((TextBox)(GridViewOrganizationInfo.Rows[e.RowIndex].Cells[3].Controls[0])).Text;
            string Address2 = ((TextBox)(GridViewOrganizationInfo.Rows[e.RowIndex].Cells[4].Controls[0])).Text;
            string Address3 = ((TextBox)(GridViewOrganizationInfo.Rows[e.RowIndex].Cells[5].Controls[0])).Text;
            string ZipCode = ((TextBox)(GridViewOrganizationInfo.Rows[e.RowIndex].Cells[6].Controls[0])).Text;
            string Website = ((TextBox)(GridViewOrganizationInfo.Rows[e.RowIndex].Cells[7].Controls[0])).Text;
            string Hours = ((TextBox)(GridViewOrganizationInfo.Rows[e.RowIndex].Cells[8].Controls[0])).Text;
            string Notes = ((TextBox)(GridViewOrganizationInfo.Rows[e.RowIndex].Cells[9].Controls[0])).Text;

            // Must bind data to get organization's ID
            await BindData();
            DataTable dt = (DataTable)GridViewOrganizationInfo.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string OrganizationID = dt.Rows[(10 * GridViewOrganizationInfo.PageIndex) + e.RowIndex][0] as String;
            string OldName = dt.Rows[(10 * GridViewOrganizationInfo.PageIndex) + e.RowIndex][1] as String;

            string QueryString = OrganizationID;
            QueryString += "?OldName=" + OldName;
            QueryString += "&NewName=" + NewName;
            QueryString += "&Phone=" + Phone;
            QueryString += "&AddressLine1=" + Address1;
            QueryString += "&AddressLine2=" + Address2;
            QueryString += "&AddressLine3=" + Address3;
            QueryString += "&ZipCode=" + ZipCode;
            QueryString += "&Website=" + Website;
            QueryString += "&Hours=" + Hours;
            QueryString += "&Notes=" + Notes;

            // Check entries against restrictions
            long PhoneNumeric;
            if (NewName == "")
            {
                LiteralErrorMessageGridView.Text = "The organization name field is required.";
                RestoreSearchTerm();
                GridViewOrganizationInfo_RowEditing(sender, new GridViewEditEventArgs(e.RowIndex));
                return;
            }
            else if(ContainsSpecialCharacters(NewName))
            {
                LiteralErrorMessageGridView.Text = "The organization name cannot contain special characters (/, \\, #, ?)";
                RestoreSearchTerm();
                GridViewOrganizationInfo_RowEditing(sender, new GridViewEditEventArgs(e.RowIndex));
                return;
            }
            else if(Phone != "" && Phone != "Private")
            {
                if(!Int64.TryParse(Phone, out PhoneNumeric))
                {
                    LiteralErrorMessageGridView.Text = "The phone number cannot contain non-numeric characters";
                    RestoreSearchTerm();
                    GridViewOrganizationInfo_RowEditing(sender, new GridViewEditEventArgs(e.RowIndex));
                    return;
                }
            }


            // Atempt PUT
            var result = DataAccess.putDataToService(DataAccess.url + "api/Organizations/" + QueryString, ("").ToCharArray());
          
            RestoreSearchTerm();

            // Cancel row edit (cancelling will call bind and show the updated data)
            GridViewOrganizationInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(e.RowIndex));
        }

        protected async void GridViewOrganizationInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            StoreSearchTerm();

            await BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewOrganizationInfo.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string OrganizationID = dt.Rows[(10 * GridViewOrganizationInfo.PageIndex) + e.RowIndex][0] as String;
            string OrganizationName = dt.Rows[(10 * GridViewOrganizationInfo.PageIndex) + e.RowIndex][1] as String;

            // Attempt DELETE
            DataAccess.deleteDataToService(DataAccess.url + "api/Organizations/" + OrganizationID + "?Name=" + OrganizationName, ("").ToCharArray());

            await BindData();
            RestoreSearchTerm();
        }

        protected void StoreSearchTerm()
        {
            // Retrieve the search box text for upcomming data bind
            TextBox Search = GridViewOrganizationInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
        }

        protected void RestoreSearchTerm()
        {
            // Repopulate search box with search string
            TextBox Search = GridViewOrganizationInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            Search.Text = SearchString;
        }

        protected bool ContainsSpecialCharacters(string Text)
        {
            if(Text.Contains("/"))
            {
                return true;
            }
            else if(Text.Contains("\\"))
            {
                return true;
            }
            else if(Text.Contains("#"))
            {
                return true;
            }
            else if(Text.Contains("?"))
            {
                return true;
            }

            return false;
        }

        protected void ClearAddOrganizationInput()
        {
            TextBoxName.Text = "";
            TextBoxPhone.Text = "";
            TextBoxAddress1.Text = "";
            TextBoxAddress2.Text = "";
            TextBoxAddress3.Text = "";
            TextBoxZipCode.Text = "";
            TextBoxWebsite.Text = "";
            TextBoxHours.Text = "";
            TextBoxNotes.Text = "";
            LiteralErrorMessageAddOrganization.Text = "";
        }

        protected void SetSearchStatus()
        {
            TextBox Search = GridViewOrganizationInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            if(Search.Text == "")
            {
                Session["SearchEnabled"] = false;
            }
            else
            {
                Session["SearchEnabled"] = true;
            }
        }
    }
}