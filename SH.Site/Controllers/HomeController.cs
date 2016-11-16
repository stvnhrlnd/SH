using SH.Site.ExtensionMethods;
using SH.Site.Models;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace SH.Site.Controllers
{
    public class HomeController : RenderMvcController
    {
        public ActionResult Home()
        {
            var model = new HomeViewModel(CurrentPage as Home);

            model.LatestPost = (
                from p in CurrentPage.Website().DescendantsOrSelf<Post>()
                orderby p.Published descending
                select p
            ).FirstOrDefault();

            return CurrentTemplate(model);
        }
    }
}