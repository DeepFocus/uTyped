using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class ContentPickerResolver : BaseResolver
    {
        public ContentPickerResolver(UmbracoHelper umbracoHelper, string propertyName = null)
            : base(umbracoHelper, propertyName) { }

        /// <summary>
        /// Returns the typed content linked to the source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override ResolutionResult Resolve(ResolutionResult source)
        {
            PropertyName = PropertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(PropertyName))
            {
                var id = content.GetPropertyValue<string>(PropertyName);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    source = source.New(UmbracoHelper.TypedContent(id));
                }
            }

            return source.New(null);
        }
    }

    public class ContentPickerResolver<T> : BaseResolver
    {
        public ContentPickerResolver(UmbracoHelper umbracoHelper, string propertyName = null)
            : base(umbracoHelper, propertyName) { }

        /// <summary>
        /// Returns the typed content depending on the type parameter linked to the source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override ResolutionResult Resolve(ResolutionResult source)
        {
            PropertyName = PropertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(PropertyName))
            {
                var id = content.GetPropertyValue<string>(PropertyName);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    source = source.New(Mapper.Map<T>(UmbracoHelper.TypedContent(id)));
                }
            }

            return source.New(null);
        }
    }
}
