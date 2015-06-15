using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace FailingHTTPApi.Controllers
{
    public class ServerFailuresController : ApiController
    {
        [Route("ServerFail/500")]
        [HttpGet()]
        public IHttpActionResult GetInternalServerError()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.Headers.Add("incident-reference-id",Guid.NewGuid().ToString());
            return new ResponseMessageResult(response);
        }

        [Route("ServerFail/503")]
        [HttpGet()]
        public IHttpActionResult GetUnavailable()
        {
            var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(new TimeSpan(0, 5, 0));
            return new ResponseMessageResult(response);
        }
    }

    public class ClientFailuresController : ApiController
    {
        [Route("ClientFail/429")]
        [HttpGet()]
        public IHttpActionResult GetInternalServerError()
        {
            var response = new HttpResponseMessage((HttpStatusCode)429);
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(new TimeSpan(0, 1, 0));
            return new ResponseMessageResult(response);
        }
    }


}
