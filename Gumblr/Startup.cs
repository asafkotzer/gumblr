using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gumblr.Startup))]
namespace Gumblr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
