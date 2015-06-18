using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;

namespace FailingHTTPApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Info;

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new DosProtectionHandler());
            config.MessageHandlers.Add(new MethodNotAllowedHandler());
            config.MessageHandlers.Add(new DefaultMediaTypeHandler());
            
        }
    }
}