using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToDo.Startup))]
namespace ToDo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
