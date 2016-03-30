using AutoMapper;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class MultiMediaResolver : BaseResolver
    {
        public MultiMediaResolver(UmbracoHelper umbracoHelper, string propertyName = null)
            : base(umbracoHelper, propertyName) { }

        /// <summary>
        /// Resolve multiple media contents from Umbraco and returns their urls
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override ResolutionResult Resolve(ResolutionResult source)
        {
            PropertyName = PropertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(PropertyName))
            {
                var ids = content.GetPropertyValue<string>(PropertyName);
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    return source.New(UmbracoHelper.TypedMedia(ids.Split(',')).Select(m => m.Url));
                }
            }
            return source.New(null);
        }
    }
}
