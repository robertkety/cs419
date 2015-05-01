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
    public partial class ManageCategories : System.Web.UI.Page
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

                HttpResponseMessage response = await client.GetAsync("api/categories");
                if (response.IsSuccessStatusCode)
                {
                    Category[] categories = await response.Content.ReadAsAsync<Category[]>();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("CategoryName");

                    foreach (Category category in categories)
                    {
                        var dr = dt.NewRow();
                        dr["CategoryName"] = category.RowKey;
                        dt.Rows.Add(dr);
                    }

                    // Convert to dataview to sort categories, then convert back to table
                    DataView dv = dt.DefaultView;
                    dv.Sort = "CategoryName ASC";
                    DataTable dt_sorted = dv.ToTable();

                    GridViewCategoryInfo.DataSource = dt_sorted;
                    GridViewCategoryInfo.DataBind();

                    PanelErrorMessages.Visible = false;
                    PanelCategoryInfo.Visible = true;
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelCategoryInfo.Visible = false;
                }
            }
        }

        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/categories");
            if (response.IsSuccessStatusCode)
            {
                Category[] categories = await response.Content.ReadAsAsync<Category[]>();
                DataTable dt = new DataTable();
                bool sortAscending = this.SortDirection == SortDirection.Ascending ? true : false;
                dt.Columns.Add("CategoryName");

                foreach (Category category in categories)
                {
                    var dr = dt.NewRow();
                    dr["CategoryName"] = category.RowKey;
                    dt.Rows.Add(dr);
                }

                DataView dv = dt.DefaultView;
                switch (this.SortExpression)
                {
                    case "CategoryName":
                        dv.Sort = "CategoryName ASC";
                        break;
                    default:
                        dv.Sort = "CategoryName ASC";
                        break;
                }

                DataTable sorted_dt = dv.ToTable();
                GridViewCategoryInfo.DataSource = sorted_dt;
                GridViewCategoryInfo.DataBind();

                return true;
            }

            return false;
        }

        protected async void GridViewCategoryInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCategoryInfo.EditIndex = e.NewEditIndex;
            bool status = await BindData();

            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
        }

        protected async void GridViewCategoryInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCategoryInfo.EditIndex = -1;
            bool status = await BindData();

            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
        }

        protected void GridViewOrganizationInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewCategoryInfo.PageIndex = e.NewPageIndex;
            GridViewCategoryInfo.EditIndex = -1;
            GridViewCategoryInfo.SelectedIndex = -1;
        }

        protected async void GridViewOrganizationInfo_PageIndexChanged(object sender, EventArgs e)
        {
            await BindData();
        }

        protected void GridViewOrganizationInfo_Sorting(object sender, GridViewSortEventArgs e)
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

            GridViewCategoryInfo.EditIndex = -1;
            GridViewCategoryInfo.SelectedIndex = -1;
        }
    }
}