using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace SH.Site.ExtensionMethods
{
    public static class PublishedContentExtensions
    {
        public static Website Website(this IPublishedContent content)
        {
            var website = HttpContext.Current.Items["Website"] as Website;

            if (website == null)
            {
                website = content.AncestorOrSelf<Website>();
                HttpContext.Current.Items["Website"] = website;
            }

            return website;
        }
    }
}