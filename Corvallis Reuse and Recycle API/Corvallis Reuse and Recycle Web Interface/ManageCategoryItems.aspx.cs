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

namespace CRRD_Web_Interface
{
    public partial class ManageCategoryItems : System.Web.UI.Page
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
                    foreach (Category category in categories)
                    {
                        DropDownListCategories.Items.Add(new ListItem(category.RowKey, category.PartitionKey));
                        PanelErrorMessages.Visible = false;
                    }
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelCategoryItems.Visible = false;
                }
            }
        }

        /*
         * Usage: Binds item data to gridview
         */
        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/categories/" + DropDownListCategories.SelectedValue);
            if (response.IsSuccessStatusCode)
            {
                Item[] items = await response.Content.ReadAsAsync<Item[]>();
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemID");
                dt.Columns.Add("ItemName");

                foreach (Item item in items)
                {
                    var dr = dt.NewRow();
                    dr["ItemID"] = item.PartitionKey;
                    dr["ItemName"] = item.RowKey;
                    dt.Rows.Add(dr);
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "ItemName ASC";
                DataTable sorted_dt = dv.ToTable();

                // Special instructions if search operation is underway
                if (SearchString != "")
                {
                    DataRow[] FilteredRows = sorted_dt.Select("ItemName like '%" + SearchString + "%'");
                    DataTable filtered_dt = new DataTable();
                    filtered_dt = sorted_dt.Clone();

                    // If search result is 0, return full table
                    if (FilteredRows.Count() == 0)
                    {
                        GridViewCategoryItems.DataSource = sorted_dt;
                        GridViewCategoryItems.DataBind();
                        return true;
                    }

                    foreach (DataRow row in FilteredRows)
                    {
                        filtered_dt.Rows.Add(row.ItemArray);
                    }

                    GridViewCategoryItems.DataSource = filtered_dt;
                    GridViewCategoryItems.DataBind();
                    return true;
                }

                // Else return full sorted table
                GridViewCategoryItems.DataSource = sorted_dt;
                GridViewCategoryItems.DataBind();

                return true;
            }

            return false;
        }

        /*
         * Usage: Loads gridview content based on category drop down list
         */
        protected async void DropDownListCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If no selection has been made, return
            if (DropDownListCategories.SelectedValue == "-1")
            {
                return;
            }

            // Else attempt to create grid view from selected category
            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelCategoryItems.Visible = false;
            }
            else
            {
                PanelCategoryItems.Visible = true;
            }
        }

        /*
         * Usage: Implements page indexing for manual gridview
         */
        protected void GridViewCategoryItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewCategoryItems.PageIndex = e.NewPageIndex;
            GridViewCategoryItems.EditIndex = -1;
            GridViewCategoryItems.SelectedIndex = -1;
        }

        /*
         * Usage: Implements page indexing for manual gridview
         */
        protected async void GridViewCategoryItems_PageIndexChanged(object sender, EventArgs e)
        {
            await BindData();
        }

        /*
         * Usage: Implements gridview search feature
         */
        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            TextBox Search = GridViewCategoryItems.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelCategoryItems.Visible = false;
            }
            else
            {
                PanelCategoryItems.Visible = true;
            }
        }

        /*
         * Usage: Make relationship creation panel visible/not visible
         */
        protected async void LinkButtonAddRelationship_Click(object sender, EventArgs e)
        {
            if (PanelAddRelationship.Visible == true)
            {
                LinkButtonAddRelationship.Text = "+ Add a New Item/Category Relationship";
                PanelAddRelationship.Visible = false;
            }
            else
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/items/");
                if (response.IsSuccessStatusCode)
                {
                    Item[] items = await response.Content.ReadAsAsync<Item[]>();

                    foreach (Item item in items)
                    {
                        DropDownListItem.Items.Add(new ListItem(item.RowKey, item.PartitionKey));
                    }
                }

                LinkButtonAddRelationship.Text = "- Add a New Item/Category Relationship";
                PanelAddRelationship.Visible = true;
            }
        }

        /*
        * Usage: Add relationship between category and item
        */
        protected async void ButtonAddRelationship_Click(object sender, EventArgs e)
        {
            string ItemID = DropDownListItem.SelectedValue;
            string CategoryID = DropDownListItem.SelectedValue;

            if (CategoryID == "" || CategoryID == "-1")
            {
                LiteralErrorMessageAddRelationship.Text = "Category must be slected from drop-down list.";
            }
            if (ItemID == "")
            {
                LiteralErrorMessageAddRelationship.Text = "Item must be selected from drop-down list.";
            }

            var client = new HttpClient();
            StringContent ContentString = new StringContent("");
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // NEED TO CHECK IF RELATIONSHIP EXISTS? 
            HttpResponseMessage response = await client.PostAsync("api/categoryitem?id=" + CategoryID + "&items=[" + ItemID + "]", ContentString);
            if (response.IsSuccessStatusCode)
            {
                Response.Redirect((Page.Request.Url.ToString()), false);
            }
            else
            {
                LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
            }
        }

        /*
         * Usage: Delete relationship from table
        */
        protected async void GridViewCategoryItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}