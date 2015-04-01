using AutoMapper;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class MultiContentPickerResolver<T> : IValueResolver
    {
        private readonly UmbracoHelper _umbracoHelper;
        private string _propertyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="umbracoHelper"></param>
        /// <param name="propertyName"></param>
        public MultiContentPickerResolver(UmbracoHelper umbracoHelper, string propertyName = null)
        {
            _umbracoHelper = umbracoHelper;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Returns the list of typed content linked to the source.
        /// </summary>
        /// <param name="source"></param>
        public ResolutionResult Resolve(ResolutionResult source)
        {
            _propertyName = _propertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(_propertyName))
            {
                var ids = content.GetPropertyValue<string>(_propertyName);
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    return source.New(Mapper.Map<IEnumerable<T>>(_umbracoHelper.TypedContent(ids.Split(','))));
                }
            }

            return source.New(null);
        }
    }
}
