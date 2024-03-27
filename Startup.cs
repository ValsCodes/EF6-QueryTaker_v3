using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EF6_QueryTaker.Startup))]
namespace EF6_QueryTaker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
