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

                HttpResponseMessage response = await client.GetAsync("api/categories");
                if (response.IsSuccessStatusCode)
                {
                    Category[] categories = await response.Content.ReadAsAsync<Category[]>();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("CategoryID");
                    dt.Columns.Add("CategoryName");

                    foreach (Category category in categories)
                    {
                        var dr = dt.NewRow();
                        dr["CategoryID"] = category.PartitionKey;
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
                dt.Columns.Add("CategoryID");
                dt.Columns.Add("CategoryName");

                foreach (Category category in categories)
                {
                    var dr = dt.NewRow();
                    dr["CategoryID"] = category.PartitionKey;
                    dr["CategoryName"] = category.RowKey;
                    dt.Rows.Add(dr);
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "CategoryName ASC";
                DataTable sorted_dt = dv.ToTable();

                if (SearchString != "")
                {
                    DataRow[] FilteredRows = sorted_dt.Select("CategoryName like '%" + SearchString + "%'");
                    DataTable filtered_dt = new DataTable();
                    filtered_dt = sorted_dt.Clone();

                    if (FilteredRows.Count() == 0)
                    {
                        GridViewCategoryInfo.DataSource = sorted_dt;
                        GridViewCategoryInfo.DataBind();
                        return true;
                    }

                    foreach (DataRow row in FilteredRows)
                    {
                        filtered_dt.Rows.Add(row.ItemArray);
                    }
                    GridViewCategoryInfo.DataSource = filtered_dt;
                    GridViewCategoryInfo.DataBind();
                    return true;
                }

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

        protected void GridViewCategoryInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewCategoryInfo.PageIndex = e.NewPageIndex;
            GridViewCategoryInfo.EditIndex = -1;
            GridViewCategoryInfo.SelectedIndex = -1;
        }

        protected async void GridViewCategoryInfo_PageIndexChanged(object sender, EventArgs e)
        {
            await BindData();
        }

        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            TextBox Search = GridViewCategoryInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
            await BindData();
        }

        protected void LinkButtonAddCategory_Click(object sender, EventArgs e)
        {
            if (PanelAddCategory.Visible == true)
            {
                LinkButtonAddCategory.Text = "+ Add a New Category";
                PanelAddCategory.Visible = false;
            }
            else
            {
                LinkButtonAddCategory.Text = "- Add a New Category";
                PanelAddCategory.Visible = true;
            }
        }

        protected async void ButtonAddCategory_Click(object sender, EventArgs e)
        {
            if(TextBoxCategoryName.Text == "")
            {
                LiteralErrorMessageAddCategory.Text = "The category name field is required.";
                return;
            }

            var client = new HttpClient();
            StringContent queryString = new StringContent("");
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsync("api/categories?Name=" + TextBoxCategoryName.Text, queryString);
            if (response.IsSuccessStatusCode)
            {
                Response.Redirect((Page.Request.Url.ToString()), false);
            }
            else
            {
                LiteralErrorMessageAddCategory.Text = response.StatusCode.ToString();
            }
        }

        protected async void GridViewCategoryInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get edit text box value before call to data bind
            TextBox EditTextBox = GridViewCategoryInfo.Rows[e.RowIndex].FindControl("TextBoxEditCategoryName") as TextBox;
            string NewName = EditTextBox.Text;

            await BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewCategoryInfo.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string CategoryID = dt.Rows[e.RowIndex][0] as String;
            string OldName = dt.Rows[e.RowIndex][1] as String;

            if(NewName == "")
            {
                LiteralErrorMessageGridView.Text = "The category name field is required.";
                return;
            }

            var client = new HttpClient();
            StringContent ContentString = new StringContent("");
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PutAsync("api/categories/" + CategoryID + "?OldName=" + OldName + "&Name=" + NewName, ContentString);
            if (response.IsSuccessStatusCode)
            {
                Response.Redirect((Page.Request.Url.ToString()), false);
            }
            else
            {
                LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
            }
        }

        protected async void GridViewCategoryInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            await BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewCategoryInfo.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string CategoryID = dt.Rows[e.RowIndex][0] as String;
            string CategoryName = dt.Rows[e.RowIndex][1] as String;

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.DeleteAsync("api/categories/" + CategoryID + "?Name=" + CategoryName);
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