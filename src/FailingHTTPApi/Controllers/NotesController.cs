using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;

namespace FailingHTTPApi.Controllers
{
    
    public class NotesController : ApiController
    {
        // Search Thing
        [Route("Notes")]
        public IHttpActionResult Get(string q)
        {
            if (string.IsNullOrEmpty(q)) return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.NotFound));

            Request.CheckQueryString(new string[] { "q" });
            
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Not implemented yet")
            });
        }

        // List Thing
        [Route("Notes")]
        public IHttpActionResult Get()
        {
            Request.CheckQueryString(new string[] { });

            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Not implemented yet")
            });

        }

        // List Thing
        [Route("Notes/secret")]
        public IHttpActionResult GetSecretNotes()
        {
            Request.CheckQueryString(new string[] { });
            Request.CheckAuthorization();

            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Not implemented yet")
            });
            
        }

        // List Thing
        [Route("Notes")]
        public IHttpActionResult Post(JObject body)
        {
            // Web API will match unknown query parameters to actions without parameters
            Request.CheckQueryString(new string[] { });

            if (Request.Content.Headers.ContentLength > 500) { return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge)); }

            // The JSON formatter passes in a null object when receiving a malformed JSON document.
            if (body == null) return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));

            var noteprop = body.Property("note");
            if (noteprop == null) return new ResponseMessageResult(new HttpResponseMessage((HttpStatusCode)422));
            var newNote = (string)((JProperty)noteprop).Value;

            NotesService.AddNote(newNote);

            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(body.ToString())
            });
        }
    }
}