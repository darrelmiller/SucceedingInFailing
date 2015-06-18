using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using FailingHTTPApi.Tooling;
using Tavis;

namespace FailingHTTPApi
{
    public static class RequestMessageExtensions
    {
        public static void CheckQueryString(this HttpRequestMessage request, string[] expectedParameters)
        {
            var queryParams = request.GetQueryNameValuePairs().Select(p => p.Key);
            if (!queryParams.SequenceEqual(expectedParameters))
            {
                var badParams = queryParams.Where(q => !expectedParameters.Contains(q)).ToArray();
                throw new ProblemException( ProblemFactory.CreateQueryParameterNotFoundProblem(badParams));
            }
        }

        public static void CheckAcceptHeader(this HttpRequestMessage request, string[] produces)
        {
            var mediatypes = request.Headers.Accept.Select(h => h.MediaType);
            if (!mediatypes.Any()) return; // Use default

            var matches = produces.Where(p => mediatypes.Any(m => m == p));
            if (!matches.Any())
            {
                throw new ProblemException(ProblemFactory.CreateNotAcceptableProblem(produces));
            }
        }

        
        public static void CheckAuthorization(this HttpRequestMessage request)
        {
            
            if (request.Headers.Authorization == null)
            {
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                httpResponseMessage.Headers.WwwAuthenticate.Add(new System.Net.Http.Headers.AuthenticationHeaderValue("basic"));
                httpResponseMessage.Content = new ProblemContent(ProblemFactory.CreateNotAuthorizedProblem());
                throw new HttpResponseException(httpResponseMessage);
            }
            if (!IsAuthorized(request.Headers.Authorization))
            {
                throw new ProblemException(ProblemFactory.CreateForbiddenProblem("Credentials provided do not have sufficient permissions"));
            }

        }

        private static bool IsAuthorized(System.Net.Http.Headers.AuthenticationHeaderValue authenticationHeaderValue)
        {
            // Dummy for now
            return false;
        }
    }
}