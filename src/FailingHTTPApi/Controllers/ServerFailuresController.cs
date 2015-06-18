using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FailingHTTPApi.Tooling;
using Tavis;

namespace FailingHTTPApi.Controllers
{
    public class ServerFailuresController : ApiController
    {
        [Route("ServerFail/500")]
        [HttpGet()]
        public IHttpActionResult GetInternalServerError()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var incident =Guid.NewGuid().ToString();
            response.Headers.Add("incident-reference-id",incident);
            response.Content = new ProblemContent(ProblemFactory.CreateInternalServerErrorProblem(incident));
            return new ResponseMessageResult(response);
        }

        [Route("ServerFail/503")]
        [HttpGet()]
        public IHttpActionResult GetUnavailable()
        {
            var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(new TimeSpan(0, 5, 0));
            response.Content = new ProblemContent(ProblemFactory.CreateServiceUnavailableProblem());
            return new ResponseMessageResult(response);
        }
    }
}
