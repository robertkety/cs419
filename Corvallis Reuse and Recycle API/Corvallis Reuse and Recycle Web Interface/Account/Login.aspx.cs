using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CRRD_Web_Interface.Models;

namespace CRRD_Web_Interface.Account
{
    public partial class Login : Page
    {
        public static string returnUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            var url = HttpContext.Current.Request.Url;
            
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            
            /*if (returnUrl == null)
                returnUrl = url.Scheme + @"://" + url.Authority + @"/Default";*/
                
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);
                
                // This will send the same login info to the Web API for a parallel authentication (Ideally, we'll have both pointed to the same Azure SQL DB)
                Entities.Login newlogin = new Entities.Login(Email.Text, Password.Text);
                result &= DataAccess.PostLogin(newlogin);
                
                switch (result)
                {
                    case SignInStatus.Success:
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
                                                        Request.QueryString["ReturnUrl"],
                                                        RememberMe.Checked),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "Invalid login attempt";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }
    }
}