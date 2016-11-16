using SH.Site.ExtensionMethods;
using SH.Site.Models;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace SH.Site.Controllers
{
    public class BlogController : RenderMvcController
    {
        public ActionResult Blog()
        {
            var model = new BlogViewModel(CurrentPage as Blog);

            model.Posts = from p in CurrentPage.Website().DescendantsOrSelf<Post>()
                          orderby p.Published descending
                          select p;

            return CurrentTemplate(model);
        }
    }
}