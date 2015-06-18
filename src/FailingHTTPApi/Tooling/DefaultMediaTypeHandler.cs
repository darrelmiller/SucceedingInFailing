using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FailingHTTPApi
{
    public class DefaultMediaTypeHandler : DelegatingHandler
    {

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content != null && request.Content.Headers.ContentType == null)
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}