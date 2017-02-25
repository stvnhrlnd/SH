using AutoMapper;
using SH.Site.Models;
using System.Linq;
using System.Web.Hosting;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace SH.Site.EventHandlers
{
    public class JsonCacheEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            ContentService.Published += ContentService_Published;

            Mapper.CreateMap<IPublishedContent, JsonCacheContentModel>()
                .ForMember(m => m.Content,
                    o => o.ResolveUsing(c => c.Properties.ToDictionary(p => p.PropertyTypeAlias, p => p.DataValue)))
                .ForMember(m => m.Template, o => o.ResolveUsing(c => c.GetTemplateAlias()));
        }

        private void ContentService_Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            // Invalidate JSON cache
            var jsonCachePath = HostingEnvironment.MapPath(Constants.JsonCachePath);
            var jsonCacheMd5Path = HostingEnvironment.MapPath(Constants.JsonCacheMd5Path);
            System.IO.File.Delete(jsonCachePath);
            System.IO.File.Delete(jsonCacheMd5Path);
        }
    }
}