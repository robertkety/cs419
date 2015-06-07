using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using CRRD_Web_Interface.Models;
using System.Data;
using System.Diagnostics;

// Manual grid view implementation borrowed from: http://aarongoldenthal.com/post/2009/04/19/Manually-Databinding-a-GridView.aspx
// Sorting data table: http://stackoverflow.com/questions/9107916/sorting-rows-in-a-data-table
namespace CRRD_Web_Interface
{
    public partial class ManageItems : System.Web.UI.Page
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
                if(status == true)
                {
                    PanelErrorMessages.Visible = false;
                    PanelItemInfo.Visible = true;
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelItemInfo.Visible = false;
                }
            }
        }

        protected bool BindData()
        {
            List<Item> items = DataAccess.Get<Item>("items");

            DataView dv = (Transforms.ConvertToDataTable<Item>(items)).DefaultView;

            dv.Sort = "Name ASC";
            DataTable sorted_dt = dv.ToTable();

            try
            {
                if ((bool)Session["SearchEnabled"])
                {
                    GridViewItemInfo.DataSource = Transforms.FilterByName(sorted_dt, SearchString);
                    GridViewItemInfo.DataBind();

                    return true;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }

            GridViewItemInfo.DataSource = sorted_dt;
            GridViewItemInfo.DataBind();

            return true;
        }

        protected void GridViewItemInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewItemInfo.EditIndex = e.NewEditIndex;
            StoreSearchTerm();

            bool status = BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
            else
            {
                // Populates the edit box with the old item name
                DataTable dt = (DataTable)GridViewItemInfo.DataSource;
                TextBox TextBoxEditItem = GridViewItemInfo.Rows[e.NewEditIndex].FindControl("TextBoxEditItemName") as TextBox;
                TextBoxEditItem.Text = dt.Rows[(10 * GridViewItemInfo.PageIndex) + e.NewEditIndex][1].ToString();

                RestoreSearchTerm();
            }
        }

        protected void GridViewItemInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewItemInfo.EditIndex = -1;
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

        protected void GridViewItemInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewItemInfo.PageIndex = e.NewPageIndex;
            GridViewItemInfo.EditIndex = -1;
            GridViewItemInfo.SelectedIndex = -1;
        }

        protected void GridViewItemInfo_PageIndexChanged(object sender, EventArgs e)
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
            GridViewItemInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(0));

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

        protected void LinkButtonAddItem_Click(object sender, EventArgs e)
        {
            GridViewItemInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(0));

            if (PanelAddItem.Visible == true)
            {
                LinkButtonAddItem.Text = "+ Add a New Item";
                PanelAddItem.Visible = false;
                ClearAddItemInput();
            }
            else
            {
                LinkButtonAddItem.Text = "- Add a New Item";
                PanelAddItem.Visible = true;
            }
        }

        protected void ButtonAddItem_Click(object sender, EventArgs e)
        {
            // Validate Input
            if (TextBoxItemName.Text == "")
            {
                LiteralErrorMessageAddItem.Text = "The item name field is required.";
                return;
            }

            // Attempt POST
            var result = DataAccess.postDataToService(DataAccess.url + "api/Items/?Name=" + TextBoxItemName.Text, ("").ToCharArray());
            ClearAddItemInput();
            Response.Redirect((Page.Request.Url.ToString()), false);
        }

        protected void GridViewItemInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            StoreSearchTerm();

            // Get edit text box value before call to data bind
            TextBox EditTextBox = GridViewItemInfo.Rows[e.RowIndex].FindControl("TextBoxEditItemName") as TextBox;
            string NewName = EditTextBox.Text;

            // Must bind data to get Item's ID
            BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewItemInfo.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string ItemID = dt.Rows[(10 * GridViewItemInfo.PageIndex) + e.RowIndex][0] as String;
            string OldName = dt.Rows[(10 * GridViewItemInfo.PageIndex) + e.RowIndex][1] as String;

            // Validate name
            if (NewName == "")
            {
                LiteralErrorMessageGridView.Text = "The item name field is required.";
                RestoreSearchTerm();
                GridViewItemInfo_RowEditing(sender, new GridViewEditEventArgs(e.RowIndex));
                return;
            }

            // Build Query
            string QueryString = ItemID;
            QueryString += "?OldName=" + OldName;
            QueryString += "&NewName=" + NewName;

            // Atempt PUT
            var result = DataAccess.putDataToService(DataAccess.url + "api/Items/" + QueryString, ("").ToCharArray());

            RestoreSearchTerm();

            // Cancel row edit (cancelling will call bind and show the updated data)
            GridViewItemInfo_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(e.RowIndex));
        }

        protected void GridViewItemInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            StoreSearchTerm();

            BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewItemInfo.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string ItemID = dt.Rows[(10 * GridViewItemInfo.PageIndex) + e.RowIndex][0] as String;
            string ItemName = dt.Rows[(10 * GridViewItemInfo.PageIndex) + e.RowIndex][1] as String;

            // Attempt DELETE
            DataAccess.deleteDataToService(DataAccess.url + "api/Items/" + ItemID + "?Name=" + ItemName, ("").ToCharArray());

            BindData();
            RestoreSearchTerm();
        }

        protected void StoreSearchTerm()
        {
            // Retrieve the search box text for upcomming data bind
            TextBox Search = GridViewItemInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
        }

        protected void RestoreSearchTerm()
        {
            // Repopulate search box with search string
            TextBox Search = GridViewItemInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            Search.Text = SearchString;
        }

        protected void ClearAddItemInput()
        {
            TextBoxItemName.Text = "";
            LiteralErrorMessageAddItem.Text = "";
        }

        protected void SetSearchStatus()
        {
            TextBox Search = GridViewItemInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
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