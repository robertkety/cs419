using System;
using System.Web.UI;

namespace CRRD_Web_Interface
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!User.Identity.IsAuthenticated || DataAccess.token == "")
            {
                Response.Redirect("~/Account/Login");
            }
        }
    }
}