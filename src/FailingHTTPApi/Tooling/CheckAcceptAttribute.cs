using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FailingHTTPApi
{
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