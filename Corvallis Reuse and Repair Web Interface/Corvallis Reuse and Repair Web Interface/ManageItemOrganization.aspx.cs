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
    public partial class ManageItemOrganization : Page
    {
        protected string SearchString;
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

                // Get all items
                List<Item> items = DataAccess.Get<Item>("items");
                items.Sort(new Comparison<Item>((x, y) => string.Compare(x.Name, y.Name)));
                DropDownListItems.Items.Add(new ListItem("<Select an Item>", "-1"));
                foreach (Item item in items)
                {
                    DropDownListItems.Items.Add(new ListItem(item.Name, item.Id));
                }

                PanelErrorMessages.Visible = false;
                PanelItemOrganization.Visible = true;
            }   
        }

        protected bool BindData()
        {
            // Get organizations
            List<Organization> organizations = DataAccess.Get<Organization>("organizations");
            organizations.Sort(new Comparison<Organization>((x, y) => string.Compare(x.Name, y.Name)));

            // Get item organizations
            List<Organization> itemOrganizations = DataAccess.Get<Organization>("items/" + DropDownListItems.SelectedValue);
            itemOrganizations.Sort(new Comparison<Organization>((x, y) => string.Compare(x.Name, y.Name)));
            
            // Build table
            DataTable dt = new DataTable();
            dt.Columns.Add("OrganizationID");
            dt.Columns.Add("OrganizationName");
            dt.Columns.Add("Reusable");
            dt.Columns.Add("Repairable");

            Organization itemOrganization = itemOrganizations.FirstOrDefault();
            if(itemOrganization == null)
            {
                itemOrganization = new Organization();
                itemOrganization.Id = "-1";
            }
            int index = 0;
            foreach (Organization organization in organizations)
            {
                var dr = dt.NewRow();

                dr["OrganizationID"] = organization.Id;
                dr["OrganizationName"] = organization.Name + " (" + organization.AddressLine1 + ")";
                
                if(organization.Id == itemOrganization.Id)
                {
                    switch (itemOrganization.Offering)
                    {
                        case Enums.offering.reuse:
                            dr["Reusable"] = true;
                            dr["Repairable"] = false;
                            break;
                        case Enums.offering.repair:
                            dr["Reusable"] = false;
                            dr["Repairable"] = true;
                            break;
                        case Enums.offering.both:
                            dr["Reusable"] = true;
                            dr["Repairable"] = true;
                            break;
                        default:
                            break;
                    }

                    index++;
                    if(index < itemOrganizations.Count)
                    {
                        itemOrganization = itemOrganizations[index];
                    }
                }
                else
                {
                    dr["Reusable"] = false;
                    dr["Repairable"] = false;
                }

                dt.Rows.Add(dr);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "OrganizationName ASC";
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
                DataRow[] FilteredRows = sorted_dt.Select("OrganizationName like '%" + SearchString + "%'");
                DataTable filtered_dt = new DataTable();
                filtered_dt = sorted_dt.Clone();

                if (FilteredRows.Count() == 0)
                {
                    GridViewItemOrganization.DataSource = sorted_dt;
                    GridViewItemOrganization.DataBind();
                    return true;
                }

                foreach (DataRow row in FilteredRows)
                {
                    filtered_dt.Rows.Add(row.ItemArray);
                }
                GridViewItemOrganization.DataSource = filtered_dt;
                GridViewItemOrganization.DataBind();
                return true;
            }

            GridViewItemOrganization.DataSource = sorted_dt;
            GridViewItemOrganization.DataBind();

            return true;
        }

        protected void DropDownListItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(DropDownListItems.SelectedValue == "-1")
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
                PanelItemOrganization.Visible = false;
            }
            else
            {
                PanelItemOrganization.Visible = true;
                PanelErrorMessages.Visible = false;
            }
        }

        protected void GridViewItemOrganization_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewItemOrganization.EditIndex = e.NewEditIndex;
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
                PanelItemOrganization.Visible = false;
            }
            else
            {
                PanelItemOrganization.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        protected void GridViewItemOrganization_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewItemOrganization.EditIndex = -1;
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
                PanelItemOrganization.Visible = false;
            }
            else
            {
                PanelItemOrganization.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        protected void GridViewItemOrganization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewItemOrganization.PageIndex = e.NewPageIndex;
            GridViewItemOrganization.EditIndex = -1;
            GridViewItemOrganization.SelectedIndex = -1;
        }

        protected void GridViewItemOrganization_PageIndexChanged(object sender, EventArgs e)
        {
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
                PanelItemOrganization.Visible = false;
            }
            else
            {
                PanelItemOrganization.Visible = true;
                PanelErrorMessages.Visible = false;
                RestoreSearchTerm();
            }
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            StoreSearchTerm();
            SetSearchStatus();

            GridViewItemOrganization_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(0));

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

        protected void GridViewItemOrganization_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            StoreSearchTerm();

            // Get input values
            bool Reusable = ((CheckBox)(GridViewItemOrganization.Rows[e.RowIndex].Cells[2].Controls[0])).Checked;
            bool Repairable = ((CheckBox)(GridViewItemOrganization.Rows[e.RowIndex].Cells[3].Controls[0])).Checked;
            string ItemID = DropDownListItems.SelectedValue;

            // Must bind data to get organization's ID
            try
            {
                BindData();
            }
            catch (Exception ex) 
            {
                PanelErrorMessages.Visible = true;
                PanelItemOrganization.Visible = false;
                return;
            }


            // Get organization ID
            DataTable dt = (DataTable)GridViewItemOrganization.DataSource;
            string OrganizationID = dt.Rows[(10 * GridViewItemOrganization.PageIndex) + e.RowIndex][0] as String;
            string OldName = dt.Rows[(10 * GridViewItemOrganization.PageIndex) + e.RowIndex][1] as String;

            // Calculate offering
            string Offering;
            if(Reusable && Repairable)
            {
                Offering = "3";
            }
            else if(Reusable)
            {
                Offering = "1";
            }
            else if(Repairable)
            {
                Offering = "2";
            }
            else
            {
                Offering = "0";
            }

            // Build parameter string
            string ParameterString = "?ItemId=" + ItemID;
            ParameterString += "&OrganizationId=" + OrganizationID;
            ParameterString += "&Offering=" + Offering;


            // Atempt PUT
            var result = DataAccess.putDataToService(DataAccess.url + "api/ItemOrganization" + ParameterString, ("").ToCharArray());

            // Cancel row edit (cancelling will call bind and show the updated data)
            RestoreSearchTerm();
            GridViewItemOrganization_RowCancelingEdit(sender, new GridViewCancelEditEventArgs(e.RowIndex));
        }

        protected void StoreSearchTerm()
        {
            // Retrieve the search box text for upcomming data bind
            try
            {
                TextBox Search = GridViewItemOrganization.FooterRow.FindControl("TextBoxSearch") as TextBox;
                SearchString = Search.Text;
            }
            catch(Exception ex) {}
        }

        protected void RestoreSearchTerm()
        {
            // Repopulate search box with search string
            try
            {
                TextBox Search = GridViewItemOrganization.FooterRow.FindControl("TextBoxSearch") as TextBox;
                Search.Text = SearchString;
            }
            catch (Exception ex) {}
        }

        protected void SetSearchStatus()
        {
            try
            {
                TextBox Search = GridViewItemOrganization.FooterRow.FindControl("TextBoxSearch") as TextBox;
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