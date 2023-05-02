using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OPBids.Web.Startup))]
namespace OPBids.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
