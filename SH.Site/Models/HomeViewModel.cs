using Umbraco.Web.Models;
using Umbraco.Web.PublishedContentModels;

namespace SH.Site.Models
{
    public class HomeViewModel : RenderModel<Home>
    {
        public HomeViewModel(Home content) : base(content) { }

        public Post LatestPost { get; set; }
    }
}