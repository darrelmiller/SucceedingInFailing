using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace FailingHTTPApi.Controllers
{
    
    public class NoteController : ApiController
    {
        [Route("note/{id}")]
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            if (id < 0 || id >= NotesService.Notes.Count) throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            var note = NotesService.Notes[id];
            return new ResponseMessageResult(new HttpResponseMessage() {Content = new StringContent(note) });            
        }

        [Route("note/{id}")]
        [HttpPut()]
        public IHttpActionResult Put(int id, string note)
        {
            NotesService.Notes[id] = note;
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK));
        }


        [Route("note/{id}")]
        [HttpDelete()]
        public IHttpActionResult Put(int id)
        {
            if (id >= NotesService.Notes.Count) throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            NotesService.DeleteNote(id);
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK));
        }

        [Route("note/latest")]
        [HttpGet()]
        public IHttpActionResult GetLatest(int id)
        {
            var note = NotesService.Notes[id];
            return new ResponseMessageResult(new HttpResponseMessage() { Content = new StringContent(note) });
        }
    }
}
