using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tavis;

namespace FailingHTTPApi.Tooling
{
    public static class ProblemFactory
    {
        internal static Tavis.ProblemDocument CreatePayloadTooLargeProblem(int maxsize, HttpContent httpContent)
        {
            return new ProblemDocument
                    {
                        StatusCode = HttpStatusCode.RequestEntityTooLarge,
                        ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.11"),
                        Title = String.Format("Payload larger than the allowed {0} bytes",maxsize),
                        Detail = String.Format("Payload size : {0}",httpContent.Headers.ContentLength)
                    };
        }

        internal static ProblemDocument CreateQueryParameterNotFoundProblem(IEnumerable<string> badParams)
        {
            return new ProblemDocument()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Title = "Query string parameter not found",
                        Detail = string.Format("Parameters not found: " + String.Join(", ", badParams)),
                        ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.4")
                    };
        }

        public static ProblemDocument CreateNoteNotFoundProblem(int id)
        {
            return new ProblemDocument()
            {
                StatusCode = HttpStatusCode.NotFound,
                Title = "Note not found",
                Detail = string.Format("The id {0} does not have a corresponding note", id),
                ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.4")
            };
        }
        public static ProblemDocument CreateInvalidJSONProblem(string detail)
        {
            return new ProblemDocument
            {
                StatusCode = HttpStatusCode.BadRequest,
                Title = "Invalid JSON in request body",
                Detail = detail,
                ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.1")
            };
        }

        internal static ProblemDocument CreateNotAcceptableProblem(string[] produces)
        {
            return new ProblemDocument()
                    {
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Title = "Accept header does not declare any supported media types",
                        Detail = string.Format("Supported media types: " + String.Join(", ", produces)),
                        ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.6")
                    };
        }

        internal static ProblemDocument CreateNotAuthorizedProblem()
        {
            return new ProblemDocument()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Title = "This resource is protected",
                Detail = "Please provide an authorization header using one of the schemes defined in the www-authenticate header",
                ProblemType = new Uri("https://tools.ietf.org/html/rfc7235#section-3.1")
            };
        }

        internal static ProblemDocument CreateForbiddenProblem(string reason)
        {
            return new ProblemDocument()
            {
                StatusCode = HttpStatusCode.Forbidden,
                Title = reason,
                ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.3")
            };
        }

        internal static ProblemDocument CreateMethodNotAllowedProblem(HttpMethod method)
        {
           
            
            return new ProblemDocument()
            {
                StatusCode = HttpStatusCode.Forbidden,
                Title = String.Format("Method {0} not allowed on this resource",method),
                ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.5"),
                
            };
        }

        internal static ProblemDocument CreateRequestHeadersTooLargeProblem(int maxHeaderValueBytes)
        {
            return new ProblemDocument()
            {
                StatusCode = (HttpStatusCode)431,
                Title = String.Format("Request headers cannot exceed {0} bytes",maxHeaderValueBytes),
                ProblemType = new Uri("http://tools.ietf.org/html/rfc6585#section-5")
            };
        }

        internal static ProblemDocument CreatePayloadTooLargeProblem(int maxPayloadBytes)
        {
            return new ProblemDocument()
                    {
                        StatusCode = HttpStatusCode.RequestEntityTooLarge,
                        ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.11"),
                        Title = "Payload is larger than " + maxPayloadBytes
                    };
        }

        internal static ProblemDocument CreateRequestUriTooLargeProblem(int MaxURILength)
        {
            return new ProblemDocument()
            {
                StatusCode = HttpStatusCode.RequestUriTooLong,
                ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.12"),
                Title = "Url is larger than " + MaxURILength
            };
        }

        internal static ProblemDocument CreateTooManyRequests()
        {
            return new ProblemDocument()
            {
                StatusCode = (HttpStatusCode)429,
                ProblemType = new Uri("http://tools.ietf.org/html/rfc6585#section-4"),
                Title = "Exceeded 1000 requests with a 1 minute period"
            };
        }

        internal static ProblemDocument CreateUnprocessableEntity(string title)
        {
            return new ProblemDocument()
            {
                StatusCode = (HttpStatusCode)422,
                ProblemType = new Uri("http://tools.ietf.org/html/rfc4918#section-11.2"),
                Title = title
            };
        }

        internal static ProblemDocument CreateInternalServerErrorProblem(string incidentId)
        {
            var prob = new ProblemDocument()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.6.1"),
                Title = "Server failed to successful handle the request"
            };
            prob.Extensions["incident-reference-id"] = incidentId;
            return prob;
        }

        internal static ProblemDocument CreateServiceUnavailableProblem()
        {
            return new ProblemDocument()
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.6.4"),
                Title = "Service Unavailable"
            };
        }
    }
}
