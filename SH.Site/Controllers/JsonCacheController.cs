using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SH.Site.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;

namespace SH.Site.Controllers
{
    [EnableCors("*", "*", "*", "ETag")]
    public class JsonCacheController : UmbracoApiController
    {
        [HttpGet]
        public HttpResponseMessage Download()
        {
            try
            {
                // Ensure JSON cache file exists
                var physicalPath = HostingEnvironment.MapPath(Constants.JsonCachePath);
                if (!File.Exists(physicalPath))
                    Refresh();

                // Check if the JSON cache has been updated since the last request from the client
                var md5Hash = File.ReadAllText(HostingEnvironment.MapPath(Constants.JsonCacheMd5Path));
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

        [HttpGet]
        public HttpResponseMessage Validate(string hash)
        {
            try
            {
                // Ensure JSON cache MD5 exists
                var physicalPath = HostingEnvironment.MapPath(Constants.JsonCacheMd5Path);
                if (!File.Exists(physicalPath))
                    Refresh();

                // Compare request hash with hash on file
                var fileHash = File.ReadAllText(physicalPath);
                return Request.CreateResponse(hash == fileHash);
            }
            catch (Exception exception)
            {
                Logger.Error<JsonCacheController>("Could not validate JSON cache MD5.", exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private void Refresh()
        {
            // Serialise Umbraco content tree to JSON
            var content = Umbraco.TypedContentAtRoot();
            var contentModels = Mapper.Map<IEnumerable<JsonCacheContentModel>>(content);
            var json = JsonConvert.SerializeObject(contentModels, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            // Write JSON to file
            var physicalPath = HostingEnvironment.MapPath(Constants.JsonCachePath);
            File.WriteAllText(physicalPath, json, Encoding.UTF8);

            // Compute and store MD5 hash of JSON file
            using (var stream = File.OpenRead(physicalPath))
            {
                using (var md5 = MD5.Create())
                {
                    var hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    File.WriteAllText(HostingEnvironment.MapPath(Constants.JsonCacheMd5Path), hash);
                }
            }
        }
    }
}