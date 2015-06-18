using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using FailingHTTPApi.Tooling;
using Newtonsoft.Json.Linq;
using System.Linq;
using Tavis;

namespace FailingHTTPApi.Controllers
{
    
    public class NotesController : ApiController
    {
        // Search Thing
        [Route("Notes")]
        public IHttpActionResult Get(string q)
        {
            if (string.IsNullOrEmpty(q)) return new ProblemResult(ProblemFactory.CreateQueryParameterNotFoundProblem(new [] {"q"}));

            Request.CheckQueryString(new string[] { "q" });

            var results = NotesService.SearchNotes(q);

            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Join(Environment.NewLine,results))
            });
        }

        // List Thing
        // Don't really want this, but if I remove it, then an invalid query parameter gets
        // mapped to the POST action and then the framework returns 405!
        [Route("Notes")]
        public IHttpActionResult Get()
        {
            Request.CheckQueryString(new string[] { "q"});

            // This should never get executed
            return new ProblemResult(new ProblemDocument
            {
                   StatusCode = HttpStatusCode.InternalServerError,
                   ProblemType = new Uri("https://tools.ietf.org/html/rfc7231#section-6.6.1")
            });

        }

        // List Thing
        [Route("Notes/secret")]
        public IHttpActionResult GetSecretNotes()
        {
            Request.CheckQueryString(new string[] { });
            Request.CheckAuthorization();

            var secretNotes = NotesService.SecretNotes();

            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Join(Environment.NewLine, secretNotes))
            });
            
        }

        // List Thing
        [Route("Notes")]
        public IHttpActionResult Post(JObject body)
        {
            // Web API will match unknown query parameters to actions without parameters
            Request.CheckQueryString(new string[] { });

            if (Request.Content.Headers.ContentLength > 500)
            {
                return new ProblemResult(ProblemFactory.CreatePayloadTooLargeProblem(500, Request.Content));
            }

            // The JSON formatter passes in a null object when receiving a malformed JSON document.
            if (body == null) return new ProblemResult(ProblemFactory.CreateInvalidJSONProblem(String.Join(", ",ModelState.Values.SelectMany(m=>m.Errors.Select(e => e.ErrorMessage)))));

            var noteprop = body.Property("note");
            if (noteprop == null) return new ProblemResult(ProblemFactory.CreateUnprocessableEntity("Required property 'note' is missing"));
            var newNote = (string)((JProperty)noteprop).Value;

            NotesService.AddNote(newNote);

            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(newNote)
            });
        }

        
    }
}