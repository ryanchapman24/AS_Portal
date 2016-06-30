using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AS_TestProject.Startup))]
namespace AS_TestProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
