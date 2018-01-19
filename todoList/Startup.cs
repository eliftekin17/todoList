using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(todoList.Startup))]
namespace todoList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
