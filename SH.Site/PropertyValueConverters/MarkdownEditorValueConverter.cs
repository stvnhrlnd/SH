using CommonMark;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Templates;

namespace SH.Site.PropertyValueConverters
{
    [PropertyValueType(typeof(IHtmlString))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Request)]
    public class MarkdownEditorValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return Constants.PropertyEditors.MarkdownEditorAlias.Equals(propertyType.PropertyEditorAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null)
            {
                return null;
            }

            var sourceString = source.ToString();
            sourceString = TemplateUtilities.ParseInternalLinks(sourceString);
            sourceString = TemplateUtilities.ResolveUrlsFromTextString(sourceString);
            return sourceString;
        }

        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            return new HtmlString(source == null ? string.Empty : CommonMarkConverter.Convert(source.ToString()));
        }

        public override object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview)
        {
            return source;
        }
    }
}