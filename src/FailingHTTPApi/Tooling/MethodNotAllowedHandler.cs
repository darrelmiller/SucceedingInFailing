using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FailingHTTPApi.Tooling;
using Tavis;

namespace FailingHTTPApi
{
    public class MethodNotAllowedHandler : DelegatingHandler
    {
        
        // Strip body from failed HEAD requests.
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                if (response.RequestMessage.Method == HttpMethod.Head)
                {
                    // Body is not allowed
                    var allow = response.Content.Headers.Allow;
                    response.Content = new StringContent("");  // Can't have body but Allow header is on Content!!!!
                    response.Content.Headers.ContentType = null;
                    response.Content.Headers.TryAddWithoutValidation("Allow", allow.ToString());
                    return response;
                }
                else
                {
                    var allow = response.Content.Headers.Allow;
                    response.Content = new ProblemContent(ProblemFactory.CreateMethodNotAllowedProblem(request.Method));
                    response.Content.Headers.TryAddWithoutValidation("Allow",allow.ToString());
                }
            }
            return response;
        }
    }
}