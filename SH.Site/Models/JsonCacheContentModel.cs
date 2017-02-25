using System;
using System.Collections.Generic;

namespace SH.Site.Models
{
    public class JsonCacheContentModel
    {
        public IEnumerable<JsonCacheContentModel> Children { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreatorName { get; set; }

        public string DocumentTypeAlias { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public IDictionary<string, object> Content { get; set; }

        public int SortOrder { get; set; }

        public string Template { get; set; }

        public DateTime UpdateDate { get; set; }

        public string Url { get; set; }

        public string UrlName { get; set; }

        public string WriterName { get; set; }
    }
}