using AutoMapper;
using Umbraco.Web;

namespace uTyped.Resolvers
{
    public abstract class BaseResolver : IValueResolver
    {
        protected readonly UmbracoHelper UmbracoHelper;
        protected string PropertyName;

        protected BaseResolver(UmbracoHelper umbracoHelper, string propertyName = null)
        {
            UmbracoHelper = umbracoHelper;
            PropertyName = propertyName;
        }

        public abstract ResolutionResult Resolve(ResolutionResult source);
    }
}
