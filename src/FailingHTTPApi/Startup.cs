using System;
using System.Threading.Tasks;
using System.Web.Http;
using Owin;

namespace FailingHTTPApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
        }
    }
}
