using System.Collections.Generic;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedContentModels;

namespace SH.Site.Models
{
    public class BlogViewModel : RenderModel<Blog>
    {
        public BlogViewModel(Blog content) : base(content) { }

        public IEnumerable<Post> Posts { get; set; }
    }
}