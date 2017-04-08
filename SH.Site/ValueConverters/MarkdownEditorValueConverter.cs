using CommonMark;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace SH.Site.ValueConverters
{
    [PropertyValueType(typeof(IHtmlString))]
    public class MarkdownEditorValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return Umbraco.Core.Constants.PropertyEditors.MarkdownEditorAlias.Equals(propertyType.PropertyEditorAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object data, bool preview)
        {
            var markdown = data as string;
            if (string.IsNullOrWhiteSpace(markdown))
                return null;

            var html = CommonMarkConverter.Convert(markdown);
            return new HtmlString(html);
        }
    }
}