using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FailingHTTPApi.Tooling;
using Tavis;

namespace FailingHTTPApi
{
    public class DosProtectionHandler : DelegatingHandler
    {
        private const int MaxURILength = 1000;
        private const int MaxHeaderValueBytes = 1000;
        private const int MaxPayloadBytes = 1000;

  
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsoluteUri.Length > MaxURILength)
                return new HttpResponseMessage(HttpStatusCode.RequestUriTooLong)
                {
                    RequestMessage = request,
                    Content = new ProblemContent(ProblemFactory.CreateRequestUriTooLargeProblem(MaxURILength))
                };

            foreach (var header in request.Headers)
            {
                if (header.Key.Length > MaxHeaderValueBytes )
                    return new HttpResponseMessage((HttpStatusCode)431)
                    {
                        RequestMessage = request,
                        Content = new ProblemContent(ProblemFactory.CreateRequestHeadersTooLargeProblem(MaxHeaderValueBytes))
                    };
                if (String.Join(",", header.Value).Length > MaxHeaderValueBytes)
                    return new HttpResponseMessage((HttpStatusCode)431)
                    {
                        RequestMessage = request,
                        Content = new ProblemContent(ProblemFactory.CreateRequestHeadersTooLargeProblem(MaxHeaderValueBytes))
                    };
            }

            if (request.Content != null && request.Content.Headers.ContentLength > MaxPayloadBytes)
            {
                return new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge)
                {
                    RequestMessage = request,
                    Content = new ProblemContent(ProblemFactory.CreatePayloadTooLargeProblem(MaxPayloadBytes))
                }; 
            }

            var response = await base.SendAsync(request, cancellationToken);
           
            return response;
        }
    }
}