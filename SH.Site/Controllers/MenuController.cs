﻿using SH.Site.ExtensionMethods;
using SH.Site.Models;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace SH.Site.Controllers
{
    public class MenuController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var website = CurrentPage.Website();

            var model = new MenuViewModel(CurrentPage);
            model.SiteName = website.SiteName;
            model.MenuItems = website.Children<IMaster>();
            model.ContactEmail = website.ContactEmail;
            model.LinkedInUrl = website.LinkedInUrl;
            model.TwitterUrl = website.TwitterUrl;

            return PartialView("_Menu", model);
        }
    }
}