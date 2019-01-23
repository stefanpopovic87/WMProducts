using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WMProducts.Startup))]
namespace WMProducts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
