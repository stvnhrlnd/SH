using AutoMapper;
using SH.Site.Models;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace SH.Site.EventHandlers
{
    public class JsonCacheEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            Mapper.CreateMap<IPublishedContent, JsonCacheContentModel>()
                .ForMember(m => m.Content,
                    o => o.ResolveUsing(c => c.Properties.ToDictionary(p => p.PropertyTypeAlias, p => p.DataValue)))
                .ForMember(m => m.Template, o => o.ResolveUsing(c => c.GetTemplateAlias()));
        }
    }
}