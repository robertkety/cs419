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

                if(o == null)
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
                bool sortAscending = this.SortDirection == SortDirection.Ascending ? true : false;
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
                switch(this.SortExpression)
                {
                    case "OrganizationName":
                        dv.Sort = "OrganizationName ASC";
                        break;
                    case "OrganizationPhone":
                        dv.Sort = "OrganizationPhone ASC";
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
                    case "OrganizationZipCode":
                        dv.Sort = "OrganizationZipCode ASC";
                        break;
                    case "OrganizationWebsite":
                        dv.Sort = "OrganizationWebsite ASC";
                        break;
                    case "OrganizationHours":
                        dv.Sort = "OrganizationHours ASC";
                        break;
                    case "OrganizationNotes":
                        dv.Sort = "OrganizationNotes ASC";
                        break;
                    default:
                        dv.Sort = "OrganizationName ASC";
                        break;
                }

                DataTable sorted_dt = dv.ToTable();
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

        protected void GridViewOrganizationInfo_Sorting(object sender, GridViewSortEventArgs e)
        {
            if(this.SortExpression == e.SortExpression)
            {
                this.SortDirection = this.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            }
            else
            {
                this.SortDirection = SortDirection.Ascending;
            }

            this.SortExpression = e.SortExpression;

            GridViewOrganizationInfo.EditIndex = -1;
            GridViewOrganizationInfo.SelectedIndex = -1;
        }
    }
}