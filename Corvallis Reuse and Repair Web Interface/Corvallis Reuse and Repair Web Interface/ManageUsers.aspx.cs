using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using CRRD_Web_Interface.Models;
using System.Data;

namespace CRRD_Web_Interface
{
    public partial class ManageUsers : System.Web.UI.Page
    {
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
                var context = new ApplicationDbContext();
                if(context == null)
                {
                    PanelErrorMessages.Visible = true;
                    return;
                }

                var allUsers = context.Users.ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("UserName");

                foreach (var user in allUsers)
                {
                    var dr = dt.NewRow();
                    dr["UserName"] = user.UserName;
                    dt.Rows.Add(dr);
                }

                GridViewUserInfo.DataSource = dt;
                GridViewUserInfo.DataBind();

                PanelUserInfo.Visible = true;
            }
        }

        protected void LinkButtonAddUser_Click(object sender, EventArgs e)
        {
            if (PanelAddUser.Visible == true)
            {
                LinkButtonAddUser.Text = "+ Add a New User";
                PanelAddUser.Visible = false;
            }
            else
            {
                LinkButtonAddUser.Text = "- Add a New User";
                PanelAddUser.Visible = true;
            }
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (Email.Text == "")
            {
                LiteralErrorMessageAddUser.Text = "The email field is required.";
                return;
            }
            else if (Password.Text == "")
            {
                LiteralErrorMessageAddUser.Text = "The password field is required.";
                return;
            }
            else if(ConfirmPassword.Text == "")
            {
                LiteralErrorMessageAddUser.Text = "The confirm password field is required.";
                return;
            }
            else if(Password.Text != ConfirmPassword.Text)
            {
                LiteralErrorMessageAddUser.Text = "The password and confirmation password do not match.";
                return;
            }

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                Response.Redirect((Page.Request.Url.ToString()), false);
            }
            else
            {
                LiteralErrorMessageAddUser.Text = result.Errors.FirstOrDefault();
            }
        }

        protected async void GridViewUserInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string username = GridViewUserInfo.Rows[e.RowIndex].Cells[0].Text;
            var UserManager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await UserManager.FindByNameAsync(username);

            if(user == null)
            {
                LiteralErrorMessageGridView.Text = "User does not exist.";
            }
            else if(UserManager.Users.Count() > 1)
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    if (user.Id == User.Identity.GetUserId())
                    {
                        Context.GetOwinContext().Authentication.SignOut();
                    }

                    Response.Redirect((Page.Request.Url.ToString()), false);
                }
                else
                {
                    LiteralErrorMessageGridView.Text = result.Errors.FirstOrDefault();
                }
            }
            else
            {
                LiteralErrorMessageGridView.Text = "Cannot delete the last user.";
            }
        }

    }
}