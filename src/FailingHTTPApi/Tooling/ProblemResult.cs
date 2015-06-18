using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Tavis;

namespace FailingHTTPApi
{
    public class ProblemResult : IHttpActionResult
    {
        private readonly ProblemDocument _document;

        public ProblemResult(ProblemDocument document)
        {
            _document = document;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return new HttpResponseMessage((HttpStatusCode)_document.StatusCode)
            {
                Content = new ProblemContent(_document)
            };
        }
    }
}