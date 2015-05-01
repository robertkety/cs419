using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRRD_Web_Interface.Startup))]
namespace CRRD_Web_Interface
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
