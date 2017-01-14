using AutoMapper;
using Newtonsoft.Json;
using SH.Site.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;

namespace SH.Site.Controllers
{
    public class JsonCacheController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public HttpResponseMessage Refresh()
        {
            try
            {
                // Serialise Umbraco content tree to JSON
                var content = Umbraco.TypedContentAtRoot();
                var contentModels = Mapper.Map<IEnumerable<JsonCacheContentModel>>(content);
                var json = JsonConvert.SerializeObject(contentModels);

                // Write JSON to file
                var physicalPath = HttpContext.Current.Server.MapPath(Constants.JsonCachePath);
                File.WriteAllText(physicalPath, json, Encoding.UTF8);

                // Compute and store MD5 hash of JSON file
                using (var stream = File.OpenRead(physicalPath))
                {
                    using (var md5 = MD5.Create())
                    {
                        var hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                        File.WriteAllText(HttpContext.Current.Server.MapPath(Constants.JsonCacheMd5Path), hash);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                Logger.Error<JsonCacheController>("Failed to refresh JSON cache.", exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}