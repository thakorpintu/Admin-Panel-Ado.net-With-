using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECommerce1.Startup))]
namespace ECommerce1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
