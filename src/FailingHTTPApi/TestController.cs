using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace FailingHTTPApi
{
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            Request.CheckQueryString(new string[] {});

            return new ResponseMessageResult(new HttpResponseMessage()
            {
                Content = new StringContent("Hello World")
            });
        }

        //public IHttpActionResult Head()
        //{
        //    Request.CheckQueryString(new string[] { });

        //    return new ResponseMessageResult(new HttpResponseMessage()
        //    {
        //        //Content = new StringContent("Hello World")
        //    });
        //}
    }

    // Secured controller should return 401
    // Unsupported media type
}
