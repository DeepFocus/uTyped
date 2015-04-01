using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class MediaResolver : IValueResolver
    {
        private string _propertyName;
        private readonly UmbracoHelper _umbracoHelper;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="propertyName"></param>
        public MediaResolver(UmbracoHelper helper, string propertyName = null)
        {
            _umbracoHelper = helper;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Resolve media content from Umbraco and returns its url
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
                    return source.New(_umbracoHelper.TypedMedia(id).Url);
                }
            }
            return source.New(null);
        }
    }
}
