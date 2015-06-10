using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRRD_Web_Interface.Models;

namespace CRRD_Web_Interface
{
    public partial class ManageCategoryItems : Page
    {
        protected string SearchString = String.Empty;
        protected bool Authenticated = false;   // Flag to prevent the rest of the page being rendered when user is not authenitcated

        protected void Page_Load(object sender, EventArgs e)
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

                // Get all categories
                List<Category> categories = DataAccess.Get<Category>("categories");
                categories.Sort(new Comparison<Category>((x, y) => string.Compare(x.Name, y.Name)));
                DropDownListCategories.Items.Add(new ListItem("<Select a Category>", "-1"));
                foreach (Category category in categories)
                {
                    DropDownListCategories.Items.Add(new ListItem(category.Name, category.Id));
                }

                PanelErrorMessages.Visible = false;
                PanelCategoryItems.Visible = true;
            }
        }

        /*
         * Usage: Binds item data to gridview
         */
        protected bool BindData()
        {
            // Get all items
            List<Item> items = DataAccess.Get<Item>("items");
            items.Sort(new Comparison<Item>((x, y) => string.Compare(x.Name, y.Name)));

            // Get items that are members of selected category
            List<Item> categoryItems = DataAccess.Get<Item>("CategoryItem/" + DropDownListCategories.SelectedValue);
            categoryItems.Sort(new Comparison<Item>((x, y) => string.Compare(x.Name, y.Name)));

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Member");

            // Add all items to data table
            Item categoryItem = categoryItems.FirstOrDefault();
            if (categoryItem == null)
            {
                categoryItem = new Item();
                categoryItem.Id = "-1";
            }
            int index = 0;
            foreach (Item item in items)
            {
                var dr = dt.NewRow();
                dr["ItemID"] = item.Id;
                dr["ItemName"] = item.Name;

                if (categoryItem.Id == item.Id)
                {
                    dr["Member"] = true;

                    if (index < categoryItems.Count)
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
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }

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
        protected void DropDownListCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If no selection has been made, return
            if (DropDownListCategories.SelectedValue == "-1")
            {
                return;
            }

            bool result = false;
            try
            {
                result = BindData();
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
            if (result == false)
            {
                PanelErrorMessages.Visible = true;
                PanelCategoryItems.Visible = false;
            }
            else
            {
                PanelCategoryItems.Visible = true;
                PanelErrorMessages.Visible = false;
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
        protected void GridViewCategoryItems_PageIndexChanged(object sender, EventArgs e)
        {
            StoreSearchTerm();
            LiteralErrorMessageGridView.Text = "";

            bool status = BindData();
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
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            StoreSearchTerm();
            SetSearchStatus();

            bool status = BindData();
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

        protected void GridViewCategoryItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCategoryItems.EditIndex = e.NewEditIndex;
            StoreSearchTerm();

            bool result = false;
            try
            {
                result = BindData();
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
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

        protected void GridViewCategoryItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCategoryItems.EditIndex = -1;
            StoreSearchTerm();

            bool result = false;
            try
            {
                result = BindData();
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
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
            StoreSearchTerm();
            //DataTable dt = (DataTable)GridViewCategoryItems.DataSource;
            string ItemID = ((DataBoundLiteralControl) (GridViewCategoryItems.Rows[(10 * GridViewCategoryItems.PageIndex) + e.RowIndex].Cells[0].Controls[0])).Text.Trim();
            string CategoryID = DropDownListCategories.SelectedValue;
            bool Member = ((CheckBox)(GridViewCategoryItems.Rows[e.RowIndex].Cells[2].Controls[0])).Checked;

            dynamic response = DataAccess.putDataToService(DataAccess.url + "api/CategoryItem/" + CategoryID + "?Items%5B%5D=" + ItemID + "&CreateRelation=" + Member.ToString(), ("").ToCharArray());
            
            //if (Member)
            //    response = DataAccess.putDataToService(DataAccess.url + "api/CategoryItem/" + CategoryID + "?Items%5B%5D=" + ItemID, ("").ToCharArray());
            //else
            //    response = DataAccess.deleteDataToService(DataAccess.url + "api/CategoryItem/" + CategoryID + "?Items%5B%5D=" + ItemID, ("").ToCharArray());

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
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
        }
    }
}