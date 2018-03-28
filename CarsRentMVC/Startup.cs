using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarsRentMVC.Startup))]
namespace CarsRentMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
