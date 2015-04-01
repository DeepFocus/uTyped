using AutoMapper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public class CsvPropertyResolver : IValueResolver
    {
        private string _propertyName;

        /// <summary>
        /// Default empty contructor because AutoMapper uses Activator.CreateInstance()
        /// </summary>
        public CsvPropertyResolver() { }

        /// <summary>
        /// When the property name in the destination class doesn't match the property name in the Umbraco property, use this constructor
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public CsvPropertyResolver(string propertyName = null)
        {
            _propertyName = propertyName;
        }

        /// <summary>
        /// Split a CSV stored string (usually Tags) and return a string[]
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ResolutionResult Resolve(ResolutionResult source)
        {
            _propertyName = _propertyName ?? source.Context.MemberName;
            var content = source.Context.SourceValue as IPublishedContent;

            if (null != content && content.HasProperty(_propertyName))
            {
                var values = content.GetPropertyValue<string>(_propertyName);
                if (!string.IsNullOrEmpty(values))
                {
                    return source.New(values.Split(','));
                }
            }

            return source.New(null);
        }
    }
}
