using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExamGenerator.Startup))]
namespace ExamGenerator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
