using AutoMapper;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class MultiMediaResolver : IValueResolver
    {
        private readonly UmbracoHelper _umbracoHelper;
        private string _propertyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="umbracoHelper"></param>
        /// <param name="propertyName"></param>
        public MultiMediaResolver(UmbracoHelper umbracoHelper, string propertyName = null)
        {
            _umbracoHelper = umbracoHelper;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Resolve multiple media contents from Umbraco and returns their urls
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ResolutionResult Resolve(ResolutionResult source)
        {
            _propertyName = _propertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(_propertyName))
            {
                var ids = content.GetPropertyValue<string>(_propertyName);
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    return source.New(_umbracoHelper.TypedMedia(ids.Split(',')).Select(m => m.Url));
                }
            }
            return source.New(null);
        }
    }
}
