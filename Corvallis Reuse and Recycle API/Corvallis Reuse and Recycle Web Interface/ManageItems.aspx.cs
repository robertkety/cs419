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
    public partial class ManageItems : System.Web.UI.Page
    {
        protected string SearchStringItems = String.Empty;
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

                HttpResponseMessage response = await client.GetAsync("api/items");
                if (response.IsSuccessStatusCode)
                {
                    Item[] items = await response.Content.ReadAsAsync<Item[]>();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ItemName");

                    foreach (Item item in items)
                    {
                        var dr = dt.NewRow();
                        dr["ItemName"] = item.RowKey;
                        dt.Rows.Add(dr);
                    }

                    DataView dv = dt.DefaultView;
                    dv.Sort = "ItemName ASC";
                    DataTable dt_sorted = dv.ToTable();

                    GridViewItemInfo.DataSource = dt_sorted;
                    GridViewItemInfo.DataBind();
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelItemInfo.Visible = false;
                }

                PanelErrorMessages.Visible = false;
                PanelItemInfo.Visible = true;
            }
        }

        protected async Task<bool> BindItemData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/items");
            if (response.IsSuccessStatusCode)
            {
                Item[] items = await response.Content.ReadAsAsync<Item[]>();
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemName");

                foreach (Item item in items)
                {
                    var dr = dt.NewRow();
                    dr["ItemName"] = item.RowKey;
                    dt.Rows.Add(dr);
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "ItemName ASC";
                DataTable sorted_dt = dv.ToTable();

                if (SearchStringItems != "")
                {
                    DataRow[] FilteredRows = sorted_dt.Select("ItemName like '%" + SearchStringItems + "%'");
                    DataTable filtered_dt = new DataTable();
                    filtered_dt = sorted_dt.Clone();

                    if (FilteredRows.Count() == 0)
                    {
                        GridViewItemInfo.DataSource = sorted_dt;
                        GridViewItemInfo.DataBind();
                        return true;
                    }

                    foreach (DataRow row in FilteredRows)
                    {
                        filtered_dt.Rows.Add(row.ItemArray);
                    }
                    GridViewItemInfo.DataSource = filtered_dt;
                    GridViewItemInfo.DataBind();
                    return true;
                }

                GridViewItemInfo.DataSource = sorted_dt;
                GridViewItemInfo.DataBind();

                return true;
            }

            return false;
        }

        protected async void GridViewItemInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewItemInfo.EditIndex = e.NewEditIndex;
            bool status = await BindItemData();

            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
        }

        protected async void GridViewItemInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewItemInfo.EditIndex = -1;
            bool status = await BindItemData();

            if (status == false)
            {
                PanelErrorMessages.Visible = true;
            }
        }

        protected void GridViewItemInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewItemInfo.PageIndex = e.NewPageIndex;
            GridViewItemInfo.EditIndex = -1;
            GridViewItemInfo.SelectedIndex = -1;
        }

        protected async void GridViewItemInfo_PageIndexChanged(object sender, EventArgs e)
        {
            await BindItemData();
        }

        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            TextBox Search = GridViewItemInfo.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchStringItems = Search.Text;
            await BindItemData();
        }
    }
}