using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using FailingHTTPApi.Tooling;
using Tavis;

namespace FailingHTTPApi.Controllers
{
    
    public class NoteController : ApiController
    {
        [Route("note/{id}")]
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            CheckId(id);

            var note = NotesService.GetNote(id);
            return new ResponseMessageResult(new HttpResponseMessage() {Content = new StringContent(note) });            
        }


        [Route("note/{id}")]
        [HttpPut()]
        public IHttpActionResult Put(int id, string note)
        {
            CheckId(id);

            NotesService.SetNotes(id,note);
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK));
        }


        [Route("note/{id}")]
        [HttpDelete()]
        public IHttpActionResult Put(int id)
        {
            CheckId(id);

            NotesService.DeleteNote(id);
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.OK));
        }

        [Route("note/latest")]
        [HttpGet()]
        public IHttpActionResult GetLatest(int id)
        {
            CheckId(id);

            var note = NotesService.GetNote(id);
            return new ResponseMessageResult(new HttpResponseMessage() { Content = new StringContent(note) });
        }


        private void CheckId(int id)
        {
            if (!NotesService.NoteExists(id))
            {
                var problemContent = new ProblemContent(ProblemFactory.CreateNoteNotFoundProblem(id));
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = problemContent
                });
            }

        }


    }
}
