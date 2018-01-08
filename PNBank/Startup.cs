using Microsoft.Owin;
using Owin;
using PNBank;

[assembly: OwinStartup(typeof(Startup))]
namespace PNBank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
