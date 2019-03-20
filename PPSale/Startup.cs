using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PPSale.Startup))]
namespace PPSale
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
