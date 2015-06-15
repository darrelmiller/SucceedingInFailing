using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace FailingHTTPApi.Controllers
{
    public class HomeController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            Request.CheckAcceptHeader(new[] { "text/plain" });
            return new ResponseMessageResult(new HttpResponseMessage() { Content = new StringContent("Discover doc goes here") });            
        }

        [Route("")]
        public IHttpActionResult Head()
        {
            Request.CheckAcceptHeader(new[] { "text/plain" });
            return new ResponseMessageResult(new HttpResponseMessage() );
        }

        [Route("")]
        public IHttpActionResult Options()
        {
            return new ResponseMessageResult(new HttpResponseMessage());
        }

    }
}
