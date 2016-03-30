using AutoMapper;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class MultiContentPickerResolver : BaseResolver
    {
        public MultiContentPickerResolver(UmbracoHelper umbracoHelper, string propertyName = null)
            : base(umbracoHelper, propertyName) { }

        /// <summary>
        /// Returns the list of typed content linked to the source.
        /// </summary>
        /// <param name="source"></param>
        public override ResolutionResult Resolve(ResolutionResult source)
        {
            PropertyName = PropertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(PropertyName))
            {
                var ids = content.GetPropertyValue<string>(PropertyName);
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    return source.New(UmbracoHelper.TypedContent(ids.Split(',')));
                }
            }

            return source.New(null);
        }
    }

    public class MultiContentPickerResolver<T> : BaseResolver
    {
        public MultiContentPickerResolver(UmbracoHelper umbracoHelper, string propertyName = null)
            : base(umbracoHelper, propertyName) { }

        /// <summary>
        /// Returns the list of typed content linked to the source.
        /// </summary>
        /// <param name="source"></param>
        public override ResolutionResult Resolve(ResolutionResult source)
        {
            PropertyName = PropertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(PropertyName))
            {
                var ids = content.GetPropertyValue<string>(PropertyName);
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    return source.New(Mapper.Map<IEnumerable<T>>(UmbracoHelper.TypedContent(ids.Split(','))));
                }
            }

            return source.New(null);
        }
    }
}
