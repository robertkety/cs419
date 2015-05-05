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

                HttpResponseMessage response = await client.GetAsync("api/organizations");
                if (response.IsSuccessStatusCode)
                {
                    Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                    DataTable dt = new DataTable();
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
                        dr["OrganizationName"] = organization.RowKey;
                        dr["OrganizationPhone"] = organization.Phone;
                        dr["OrganizationAddressLine1"] = organization.AddressLine1;
                        dr["OrganizationAddressLine2"] = organization.AddressLine2;
                        dr["OrganizationAddressLine3"] = organization.AddressLine3;
                        dr["OrganizationZipCode"] = organization.ZipCode;
                        dr["OrganizationWebsite"] = organization.Website;
                        dr["OrganizationHours"] = organization.Hours;
                        dr["OrganizationNotes"] = organization.Hours;
                        dt.Rows.Add(dr);
                    }

                    DataView dv = dt.DefaultView;
                    dv.Sort = "OrganizationName ASC";
                    DataTable dt_sorted = dv.ToTable();

                    GridViewOrganizationInfo.DataSource = dt_sorted;
                    GridViewOrganizationInfo.DataBind();

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
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/organizations");
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                DataTable dt = new DataTable();
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
                    dr["OrganizationName"] = organization.RowKey;
                    dr["OrganizationPhone"] = organization.Phone;
                    dr["OrganizationAddressLine1"] = organization.AddressLine1;
                    dr["OrganizationAddressLine2"] = organization.AddressLine2;
                    dr["OrganizationAddressLine3"] = organization.AddressLine3;
                    dr["OrganizationZipCode"] = organization.ZipCode;
                    dr["OrganizationWebsite"] = organization.Website;
                    dr["OrganizationHours"] = organization.Hours;
                    dr["OrganizationNotes"] = organization.Hours;
                    dt.Rows.Add(dr);
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "OrganizationName ASC";
                DataTable sorted_dt = dv.ToTable();

                if (SearchString != "")
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
            bool status = await BindData();

            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
        }

        protected async void GridViewOrganizationInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewOrganizationInfo.EditIndex = -1;
            bool status = await BindData();

            if (status == false)
            {
                PanelErrorMessages.Visible = true;
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
            await BindData();
        }

        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            TextBox Search = GridViewOrganizationInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
            await BindData();
        }
    }
}