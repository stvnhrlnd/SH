using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace SH.Site.Models
{
    public class MenuViewModel : RenderModel
    {
        public MenuViewModel(IPublishedContent content) : base(content) { }

        public string SiteName { get; set; }

        public IEnumerable<IPublishedContent> MenuItems { get; set; }

        public string ContactEmail { get; set; }

        public string LinkedInUrl { get; set; }

        public string TwitterUrl { get; set; }
    }
}