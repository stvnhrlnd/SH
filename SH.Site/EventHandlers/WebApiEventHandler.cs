using System.Web.Http;
using Umbraco.Core;

namespace SH.Site.EventHandlers
{
    public class WebApiEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            GlobalConfiguration.Configuration.EnableCors();
        }
    }
}