using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class MediaResolver : BaseResolver
    {
        public MediaResolver(UmbracoHelper umbracoHelper, string propertyName = null)
            : base(umbracoHelper, propertyName) { }

        /// <summary>
        /// Resolve media content from Umbraco and returns its url
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
                    return source.New(UmbracoHelper.TypedMedia(id).Url);
                }
            }
            return source.New(null);
        }
    }
}
