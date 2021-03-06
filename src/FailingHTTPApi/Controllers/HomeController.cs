﻿using System;
using System.Net.Http;

using System.Web.Http;
using System.Web.Http.Results;
using Tavis;
using Tavis.Home;
using Tavis.UriTemplates;

namespace FailingHTTPApi.Controllers
{
    public class HomeController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            Request.CheckAcceptHeader(new[] { "application/json-home" });

            
            var home = new HomeDocument();
            home.AddResource(new Link()
            {
                Relation = "urn:tavis:notes",
                Template = new UriTemplate(GetHost() + "/Notes{q}")
            });

            home.AddResource(new Link()
            {
                Relation = "urn:tavis:safenotes",
                Target = new Uri(GetHost() + "/Notes/secret")
            });
            home.AddResource(new Link()
            {
                Relation = "urn:tavis:note",
                Template = new UriTemplate(GetHost() + "/Note/{id}")
            });
            var homeContent = new HomeContent(home);
            homeContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-home");
            return new ResponseMessageResult(new HttpResponseMessage() { Content = homeContent });            
        }

        private string GetHost()
        {
            return Request.RequestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
        }

        //[Route("")]
        //public IHttpActionResult Head()
        //{
        //    Request.CheckAcceptHeader(new[] { "text/plain" });
        //    return new ResponseMessageResult(new HttpResponseMessage() );
        //}

        [Route("")]
        public IHttpActionResult Options()
        {
            return new ResponseMessageResult(new HttpResponseMessage());
        }

    }
}
