using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FailingHTTPApi
{
    public static class RequestMessageExtensions
    {
        public static void CheckQueryString(this HttpRequestMessage request, string[] expectedParameters)
        {
            var queryParams = request.GetQueryNameValuePairs().Select(p => p.Key);
            if (!queryParams.SequenceEqual(expectedParameters))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        public static void CheckAcceptHeader(this HttpRequestMessage request, string[] produces)
        {
            var mediatypes = request.Headers.Accept.Select(h => h.MediaType);
            if (!mediatypes.Any()) return; // Use default

            var matches = produces.Where(p => mediatypes.Any(m => m == p));
            if (!matches.Any())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable));
            }
        }

        public static void CheckAuthorization(this HttpRequestMessage request)
        {
            
            if (request.Headers.Authorization == null)
            {
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                httpResponseMessage.Headers.WwwAuthenticate.Add(new System.Net.Http.Headers.AuthenticationHeaderValue("basic"));
                throw new HttpResponseException(httpResponseMessage);
            }
            if (!IsAuthorized(request.Headers.Authorization))
            {
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden);
                throw new HttpResponseException(httpResponseMessage);
            }

        }

        private static bool IsAuthorized(System.Net.Http.Headers.AuthenticationHeaderValue authenticationHeaderValue)
        {
            // Dummy for now
            return false;
        }
    }

    public class CheckAcceptAttribute: ActionFilterAttribute
    {
        public string Produces { get; set; }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var produces = Produces.Split(',').Select(s=> s.Trim()).ToArray();

            var mediatypes = actionContext.Request.Headers.Accept.Select(h => h.MediaType);
            if (!mediatypes.Any()) return; // Use default

            var matches = produces.Where(p => mediatypes.Any(m => m == p));
            if (!matches.Any())
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    RequestMessage = actionContext.Request
                };
                
            }
        }  
    }
}