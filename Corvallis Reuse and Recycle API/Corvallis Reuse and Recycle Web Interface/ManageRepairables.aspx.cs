﻿using System;
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
    public partial class ManageRepairables : System.Web.UI.Page
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

                        DropDownListRepairableItems.Items.Add(new ListItem(item.RowKey, item.PartitionKey));
                        PanelErrorMessages.Visible = false;
                    }
                }
                else
                {
                    PanelErrorMessages.Visible = true;
                    PanelRepairableOrganization.Visible = false;
                }
            }
        }

        /*
         * Usage: Binds organization data to gridview
         */
        protected async Task<bool> BindData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/ItemOrganization?ItemId=" + DropDownListRepairableItems.SelectedValue + "&Offering=true");
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();
                DataTable dt = new DataTable();
                dt.Columns.Add("OrganizationID");
                dt.Columns.Add("OrganizationName");
                dt.Columns.Add("OrganizationAddressLine1");

                foreach (Organization organization in organizations)
                {
                    if (organization.Offering == 2 || organization.Offering == 3)
                    {
                        var dr = dt.NewRow();
                        dr["OrganizationID"] = organization.PartitionKey;
                        dr["OrganizationName"] = organization.RowKey;
                        dr["OrganizationAddressLine1"] = organization.AddressLine1;
                        dt.Rows.Add(dr);
                    }
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "OrganizationName ASC";
                DataTable sorted_dt = dv.ToTable();

                if (SearchString != "")
                {
                    DataRow[] FilteredRows = sorted_dt.Select("OrganizationName like '%" + SearchString + "%'");
                    DataTable filtered_dt = new DataTable();
                    filtered_dt = sorted_dt.Clone();

                    if (FilteredRows.Count() == 0)
                    {
                        GridViewRepairableOrganizations.DataSource = sorted_dt;
                        GridViewRepairableOrganizations.DataBind();
                        return true;
                    }

                    foreach (DataRow row in FilteredRows)
                    {
                        filtered_dt.Rows.Add(row.ItemArray);
                    }

                    GridViewRepairableOrganizations.DataSource = filtered_dt;
                    GridViewRepairableOrganizations.DataBind();
                    return true;
                }

                GridViewRepairableOrganizations.DataSource = sorted_dt;
                GridViewRepairableOrganizations.DataBind();

                return true;
            }

            return false;
        }

        /*
         * Usage: Loads gridview content based on item drop down list
         */
        protected async void DropDownListRepairableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListRepairableItems.SelectedValue == "-1")
            {
                return;
            }

            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelRepairableOrganization.Visible = false;
            }
            else
            {
                PanelRepairableOrganization.Visible = true;
            }
        }

        /*
         * Usage: Implements page indexing for manual gridview
         */
        protected void GridViewRepairableOrganizations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRepairableOrganizations.PageIndex = e.NewPageIndex;
            GridViewRepairableOrganizations.EditIndex = -1;
            GridViewRepairableOrganizations.SelectedIndex = -1;
        }

        /*
         * Usage: Implements page indexing for manual gridview
         */
        protected async void GridViewRepairableOrganizations_PageIndexChanged(object sender, EventArgs e)
        {
            await BindData();
        }

        /*
         * Usage: Implements gridview search feature
         */
        protected async void ButtonSearch_Click(object sender, EventArgs e)
        {
            TextBox Search = GridViewRepairableOrganizations.FooterRow.FindControl("TextBoxSearch") as TextBox;
            SearchString = Search.Text;
            bool status = await BindData();
            if (status == false)
            {
                PanelErrorMessages.Visible = true;
                PanelRepairableOrganization.Visible = false;
            }
            else
            {
                PanelRepairableOrganization.Visible = true;
            }
        }

        /*
         * Usage: Make relationship creation panel visible/not visible
         */
        protected async void LinkButtonAddRepairable_Click(object sender, EventArgs e)
        {
            if (PanelAddRepairable.Visible == true)
            {
                LinkButtonAddRepairable.Text = "+ Add a New Item/Organization Combination";
                PanelAddRepairable.Visible = false;
            }
            else
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/organizations/");
                if (response.IsSuccessStatusCode)
                {
                    Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();

                    foreach (Organization organization in organizations)
                    {
                        DropDownListAddRepairableOrganization.Items.Add(new ListItem(organization.RowKey, organization.PartitionKey));
                    }
                }

                LinkButtonAddRepairable.Text = "- Add a New Item/Organization Combination";
                PanelAddRepairable.Visible = true;
            }
        }

        /*
        * Usage: Add repairable relationship between organization and item
        */
        protected async void ButtonAddRelationship_Click(object sender, EventArgs e)
        {
            string ItemID = DropDownListRepairableItems.SelectedValue;
            string OrganizationID = DropDownListAddRepairableOrganization.SelectedValue;
            int Offering = 0;

            if (ItemID == "" || ItemID == "-1")
            {
                LiteralErrorMessageAddRepairable.Text = "Item must be slected from drop-down list.";
            }
            if (OrganizationID == "")
            {
                LiteralErrorMessageAddRepairable.Text = "Organization must be selected from drop-down list.";
            }

            var client = new HttpClient();
            StringContent ContentString = new StringContent("");
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get the exisiting offering for the item/organization relationship
            HttpResponseMessage response = await client.GetAsync("api/items/" + ItemID);
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();

                foreach (Organization organization in organizations)
                {
                    if (organization.PartitionKey == OrganizationID)
                    {
                        Offering = organization.Offering;
                    }
                }
            }
            else
            {
                LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
            }

            // Post if offering is 0, put if offering is 1 (reusable), error if repairable relationship already exists
            if (Offering == 0)
            {
                response = await client.PostAsync("api/itemorganization?ItemId=" + ItemID + "&OrganizationId=" + OrganizationID + "&Offering=" + "2", ContentString);
                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect((Page.Request.Url.ToString()), false);
                }
                else
                {
                    LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                }
            }
            else if (Offering == 1)
            {
                response = await client.PutAsync("api/itemorganization?ItemId=" + ItemID + "&OrganizationId=" + OrganizationID + "&Offering=" + "3", ContentString);
                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect((Page.Request.Url.ToString()), false);
                }
                else
                {
                    LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                }
            }
            else
            {
                LiteralErrorMessageAddRepairable.Text = "Item already has a repairable relationship with selected organization.";
                return;
            }
        }

        /*
         * Usage: Delete repairable relationship between organization and item
        */
        protected async void GridViewOrganizationInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int Offering = 0;
            string ItemId = DropDownListRepairableItems.SelectedValue;
            await BindData();   // Must bind data to grid to get datasource
            DataTable dt = (DataTable)GridViewRepairableOrganizations.DataSource;  // Accessing the cell's value in the grid view always returned null, so datatable was used
            string OrganizationId = dt.Rows[e.RowIndex][0] as string;

            // Get the existing offering between the item and organization
            var client = new HttpClient();
            StringContent ContentString = new StringContent("");
            client.BaseAddress = new Uri("http://cs419.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/items/" + ItemId);
            if (response.IsSuccessStatusCode)
            {
                Organization[] organizations = await response.Content.ReadAsAsync<Organization[]>();

                foreach (Organization organization in organizations)
                {
                    if (organization.PartitionKey == OrganizationId)
                    {
                        Offering = organization.Offering;
                    }
                }
            }
            else
            {
                LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                return;
            }

            // If reusable relationship exists, PUT with offering of 1, else PUT with offering of 0
            if (Offering == 3)
            {
                response = await client.PutAsync("api/ItemOrganization?ItemId=" + ItemId + "&OrganizationId=" + OrganizationId + "&Offering=" + "1", ContentString);
                if (response.IsSuccessStatusCode)
                {
                    Response.Redirect((Page.Request.Url.ToString()), false);
                }
                else
                {
                    LiteralErrorMessageGridView.Text = response.StatusCode.ToString();
                }
            }
            else
            {
                response = await client.PutAsync("api/ItemOrganization?ItemId=" + ItemId + "&OrganizationId=" + OrganizationId + "&Offering=" + "0", ContentString);
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
}