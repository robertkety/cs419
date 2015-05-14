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
                Session["SearchEnabled"] = false; 

                var client = new HttpClient();
                client.BaseAddress = new Uri(DataAccess.url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/categories");
                if (response.IsSuccessStatusCode)
                {
                    // How to sort list of objects: http://stackoverflow.com/questions/1301822/how-to-sort-an-array-of-object-by-a-specific-field-in-c
                    List<Category> categories = await response.Content.ReadAsAsync<List<Category>>();
                    categories.Sort(new Comparison<Category>((x, y) => string.Compare(x.RowKey, y.RowKey)));
                    foreach (Category category in categories)
                    {
                        DropDownListCategories.Items.Add(new ListItem(category.RowKey, category.PartitionKey));
                    }

                    PanelErrorMessages.Visible = false;
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
            client.BaseAddress = new Uri(DataAccess.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get all items
            List<Item> items;
            HttpResponseMessage response = await client.GetAsync("api/items/");
            if (response.IsSuccessStatusCode)
            {
                items = await response.Content.ReadAsAsync<List<Item>>();
                items.Sort(new Comparison<Item>((x, y) => string.Compare(x.RowKey, y.RowKey)));
            }
            else
            {
                return false;
            }

            // Get items that are members of selected category
            List<Item>categoryItems;
            response = await client.GetAsync("api/categories/" + DropDownListCategories.SelectedValue);
            if (response.IsSuccessStatusCode)
            {
                categoryItems = await response.Content.ReadAsAsync<List<Item>>();
                categoryItems.Sort(new Comparison<Item>((x, y) => string.Compare(x.RowKey, y.RowKey)));
            }
            else
            {
                return false;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Member");

            // Add all items to data table
            Item categoryItem = categoryItems.FirstOrDefault();
            if(categoryItem == null)
            {
                categoryItem = new Item();
                categoryItem.PartitionKey = "-1";
            }
            int index = 1;
            foreach (Item item in items)
            {
                var dr = dt.NewRow();
                dr["ItemID"] = item.PartitionKey;
                dr["ItemName"] = item.RowKey;

                if(categoryItem.PartitionKey == item.PartitionKey)
                {
                    dr["Member"] = true;
                    
                    if(index < categoryItems.Count)
                    {
                        categoryItem = categoryItems[index];
                        index++;
                    }
                }
                else
                {
                    dr["Member"] = false;
                }

                dt.Rows.Add(dr);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "ItemName ASC";
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
            StoreSearchTerm();
            LiteralErrorMessageGridView.Text = "";

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
            StoreSearchTerm();
            SetSearchStatus();

            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelCategoryItems.Visible = false;
            }
            else
            {
                PanelCategoryItems.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        /*
         * Usage: Delete relationship from table
        */
        protected void GridViewCategoryItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // API does not appear to support this feature, need to ask about it.
        }

        protected void StoreSearchTerm()
        {
            // Retrieve the search box text for upcomming data bind
            TextBox Search = GridViewCategoryItems.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
        }

        protected void RestoreSearchTerm()
        {
            // Repopulate search box with search string
            TextBox Search = GridViewCategoryItems.FooterRow.FindControl("TextBoxSearch") as TextBox;
            Search.Text = SearchString;
        }

        protected async void GridViewCategoryItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCategoryItems.EditIndex = e.NewEditIndex;
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
                PanelCategoryItems.Visible = false;
            }
            else
            {
                PanelCategoryItems.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        protected async void GridViewCategoryItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCategoryItems.EditIndex = -1;
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
                PanelCategoryItems.Visible = false;
            }
            else
            {
                PanelCategoryItems.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        protected void GridViewCategoryItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Cancel row edit (cancelling will call bind and show the updated data)
            RestoreSearchTerm();
            GridViewCategoryItems_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(e.RowIndex));
        }

        protected void SetSearchStatus()
        {
            try
            {
                TextBox Search = GridViewCategoryItems.FooterRow.FindControl("TextBoxSearch") as TextBox;
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