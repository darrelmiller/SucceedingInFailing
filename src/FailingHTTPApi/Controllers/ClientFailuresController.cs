using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using FailingHTTPApi.Tooling;
using Tavis;

namespace FailingHTTPApi.Controllers
{
    public class ClientFailuresController : ApiController
    {
        [Route("ClientFail/429")]
        [HttpGet()]
        public IHttpActionResult GetInternalServerError()
        {
            var response = new HttpResponseMessage((HttpStatusCode)429)
            {
                RequestMessage = Request,
                Content = new ProblemContent(ProblemFactory.CreateTooManyRequests())
            };
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(new TimeSpan(0, 1, 0));
            return new ResponseMessageResult(response);
        }
    }
}