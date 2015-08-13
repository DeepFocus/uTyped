using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class ContentPickerResolver : IValueResolver
    {
        private readonly UmbracoHelper _umbracoHelper;
        private string _propertyName;

        public ContentPickerResolver(UmbracoHelper UmbracoHelper, string PropertyName = null)
        {
            _umbracoHelper = UmbracoHelper;
            _propertyName = PropertyName;
        }

        /// <summary>
        /// Returns the typed content linked to the source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ResolutionResult Resolve(ResolutionResult source)
        {
            _propertyName = _propertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(_propertyName))
            {
                var id = content.GetPropertyValue<string>(_propertyName);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    source = source.New(_umbracoHelper.TypedContent(id));
                }
            }

            return source.New(null);
        }
    }
}
