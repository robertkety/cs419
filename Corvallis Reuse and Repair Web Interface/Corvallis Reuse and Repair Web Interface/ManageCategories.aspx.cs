using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRRD_Web_Interface.Models;

// Manual grid view implementation borrowed from: http://aarongoldenthal.com/post/2009/04/19/Manually-Databinding-a-GridView.aspx
// Sorting data table: http://stackoverflow.com/questions/9107916/sorting-rows-in-a-data-table
namespace CRRD_Web_Interface
{
    public partial class ManageCategories : Page
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
                Session["SearchEnabled"] = false;   // Prevents text in search box from filtering results unless search is requested
                bool status = BindData();
                if (status == true)
                {
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

        protected bool BindData()
        {
            List<Category> categories = DataAccess.Get<Category>("categories");

            DataView dv = (Transforms.ConvertToDataTable<Category>(categories)).DefaultView;
            dv.Sort = "Name ASC";
            DataTable sorted_dt = dv.ToTable();

            try
            {
                if ((bool)Session["SearchEnabled"])
                {
                    GridViewCategoryInfo.DataSource = Transforms.FilterByName(sorted_dt, SearchString);
                    GridViewCategoryInfo.DataBind();

                    return true;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }            

            GridViewCategoryInfo.DataSource = sorted_dt;
            GridViewCategoryInfo.DataBind();

            return true;
        }

        protected void GridViewCategoryInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCategoryInfo.EditIndex = e.NewEditIndex;
            StoreSearchTerm();

            bool status = BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
            else
            {
                // Populates the edit box with the old category name
                DataTable dt = (DataTable)GridViewCategoryInfo.DataSource;
                TextBox TextBoxEditCategoryName = GridViewCategoryInfo.Rows[e.NewEditIndex].FindControl("TextBoxEditCategoryName") as TextBox;
                TextBoxEditCategoryName.Text = dt.Rows[(10 * GridViewCategoryInfo.PageIndex) + e.NewEditIndex][1].ToString();

                RestoreSearchTerm();
            }
        }

        protected void GridViewCategoryInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCategoryInfo.EditIndex = -1;
            LiteralErrorMessageGridView.Text = "";
            StoreSearchTerm();

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

        protected void GridViewCategoryInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewCategoryInfo.PageIndex = e.NewPageIndex;
            GridViewCategoryInfo.EditIndex = -1;
            GridViewCategoryInfo.SelectedIndex = -1;
        }

        protected void GridViewCategoryInfo_PageIndexChanged(object sender, EventArgs e)
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

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            StoreSearchTerm();
            SetSearchStatus();
            GridViewCategoryInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(0));

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

        protected void LinkButtonAddCategory_Click(object sender, EventArgs e)
        {
            GridViewCategoryInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(0));

            if (PanelAddCategory.Visible == true)
            {
                LinkButtonAddCategory.Text = "+ Add a New Category";
                PanelAddCategory.Visible = false;
                ClearAddCategoryInput();
            }
            else
            {
                LinkButtonAddCategory.Text = "- Add a New Category";
                PanelAddCategory.Visible = true;
            }
        }

        protected void ButtonAddCategory_Click(object sender, EventArgs e)
        {
            // Validate input
            if(TextBoxCategoryName.Text == "")
            {
                LiteralErrorMessageAddCategory.Text = "The category name field is required.";
                return;
            }

            // Attempt POST
            var result = DataAccess.postDataToService(DataAccess.url + "api/Categories/?Name=" + TextBoxCategoryName.Text, ("").ToCharArray());
            ClearAddCategoryInput();
            Response.Redirect((Page.Request.Url.ToString()), false);
        }

        protected void GridViewCategoryInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            StoreSearchTerm();

            // Get edit text box value before call to data bind
            TextBox EditTextBox = GridViewCategoryInfo.Rows[e.RowIndex].FindControl("TextBoxEditCategoryName") as TextBox;
            string NewName = EditTextBox.Text;

            // Must bind data to grid to get category ID
            BindData();   
            DataTable dt = (DataTable)GridViewCategoryInfo.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string CategoryID = dt.Rows[(10 * GridViewCategoryInfo.PageIndex) + e.RowIndex][0] as String;
            string OldName = dt.Rows[(10 * GridViewCategoryInfo.PageIndex) + e.RowIndex][1] as String;

            // Validate input
            if(NewName == "")
            {
                LiteralErrorMessageGridView.Text = "The category name field is required.";
                RestoreSearchTerm();
                GridViewCategoryInfo_RowEditing(sender, new GridViewEditEventArgs(e.RowIndex));
                return;
            }

            // Build Query
            string QueryString = CategoryID + "?OldName=" + OldName + "&NewName=" + NewName;

            // Attempt PUT
            var result = DataAccess.putDataToService(DataAccess.url + "api/Categories/" + QueryString, ("").ToCharArray());
            RestoreSearchTerm();

            // Cancel row edit (cancelling will call bind and show the updated data)
            GridViewCategoryInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(e.RowIndex));
        }

        protected void GridViewCategoryInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            StoreSearchTerm();

            BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewCategoryInfo.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string CategoryID = dt.Rows[(10 * GridViewCategoryInfo.PageIndex) + e.RowIndex][0] as String;
            string CategoryName = dt.Rows[(10 * GridViewCategoryInfo.PageIndex) + e.RowIndex][1] as String;

            // Attempt DELETE
            DataAccess.deleteDataToService(DataAccess.url + "api/Categories/" + CategoryID + "?Name=" + CategoryName, ("").ToCharArray());

            BindData();
            RestoreSearchTerm();
        }

        protected void StoreSearchTerm()
        {
            // Retrieve the search box text for upcomming data bind
            TextBox Search = GridViewCategoryInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
        }

        protected void RestoreSearchTerm()
        {
            // Repopulate search box with search string
            TextBox Search = GridViewCategoryInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            Search.Text = SearchString;
        }

        protected void ClearAddCategoryInput()
        {
            TextBoxCategoryName.Text = "";
            LiteralErrorMessageAddCategory.Text = "";
        }

        protected void SetSearchStatus()
        {
            TextBox Search = GridViewCategoryInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            if (Search.Text == "")
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