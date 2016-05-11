using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameVote.Startup))]
namespace GameVote
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
