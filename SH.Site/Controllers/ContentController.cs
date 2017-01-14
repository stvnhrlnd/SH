﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;

namespace SH.Site.Controllers
{
    public class ContentController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Json()
        {
            try
            {
                // Ensure JSON cache file exists
                var physicalPath = HttpContext.Current.Server.MapPath(Constants.JsonCachePath);
                if (!File.Exists(physicalPath))
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                // Check if the JSON cache has been updated since the last request from the client
                var md5Hash = File.ReadAllText(HttpContext.Current.Server.MapPath(Constants.JsonCacheMd5Path));
                var responseETag = EntityTagHeaderValue.Parse($"\"{md5Hash}\"");
                var requestETag = Request.Headers.IfNoneMatch.FirstOrDefault();
                if (requestETag != null && requestETag.Tag == responseETag.Tag)
                    return Request.CreateResponse(HttpStatusCode.NotModified);

                // Create and return JSON response from file
                var stream = new FileStream(physicalPath, FileMode.Open);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response.Headers.ETag = responseETag;
                return response;
            }
            catch (Exception exception)
            {
                Logger.Error<JsonCacheController>("Could not serve JSON cache.", exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}