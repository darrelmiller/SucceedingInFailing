using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;
using System.Net;

namespace FailingHTTPApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Info;

           // config.Services.Replace(typeof(IHttpActionSelector), new MyActionSelector());
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new DosProtectionHandler());
            config.MessageHandlers.Add(new DefaultHeadHandler());
            config.MessageHandlers.Add(new DefaultMediaTypeHandler());
            
        }
    }

    public class DefaultMediaTypeHandler : DelegatingHandler
    {

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content != null && request.Content.Headers.ContentType == null)
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }


    public class DefaultHeadHandler : DelegatingHandler
    {
        
        // Strip body from failed HEAD requests.
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.RequestMessage.Method == HttpMethod.Head 
                && response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }
            return response;
        }
    }

    public class DosProtectionHandler : DelegatingHandler
    {
        private const int MaxURILength = 1000;
        private const int MaxHeaderValueBytes = 1000;
        private const int MaxPayloadBytes = 1000;

        // Strip body from failed HEAD requests.
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsoluteUri.Length > MaxURILength)
                return new HttpResponseMessage(HttpStatusCode.RequestUriTooLong);

            foreach (var header in request.Headers)
            {
                if (header.Key.Length > MaxHeaderValueBytes )
                    return new HttpResponseMessage((HttpStatusCode)431);
                if (String.Join(",", header.Value).Length > MaxHeaderValueBytes)
                    return new HttpResponseMessage((HttpStatusCode)431);
            }

            if (request.Content != null && request.Content.Headers.ContentLength > MaxPayloadBytes)
            {
                return new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge);
            }

            var response = await base.SendAsync(request, cancellationToken);
            if (response.RequestMessage.Method == HttpMethod.Head
                && response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }
            return response;
        }
    }


    //public class MyActionSelector : ApiControllerActionSelector
    //{
    //    public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
    //    {
    //        var action = base.SelectAction(controllerContext);
    //        // Parameters must match exactly except for the controller param
            
    //        if (action != null) {
    //            var actionParameters = action.GetParameters();
    //            var queryparams = controllerContext.Request.GetQueryNameValuePairs();
    //            if (actionParameters.Count == (controllerContext.RouteData.Values.Count - 1) + queryparams.Count())
    //            {
    //                return action;
    //            }
    //            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
    //        }
    //        else
    //        {
    //            return null;
    //        }
            
    //    }
    //}
}