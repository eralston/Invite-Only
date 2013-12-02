using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Invite_Only.Startup))]
namespace Invite_Only
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
