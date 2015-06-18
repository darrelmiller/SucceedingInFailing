using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tavis;

namespace FailingHTTPApi
{
    public class ProblemException : HttpResponseException
    {
        public ProblemException(ProblemDocument problem) : base(new HttpResponseMessage())
        {
            Response.StatusCode = (HttpStatusCode)problem.StatusCode;
            Response.Content = new ProblemContent(problem);
        }
    }
}