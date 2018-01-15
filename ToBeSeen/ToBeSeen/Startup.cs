using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToBeSeen.Startup))]
namespace ToBeSeen
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
