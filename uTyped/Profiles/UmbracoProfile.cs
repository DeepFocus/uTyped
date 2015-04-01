using AutoMapper;
using AutoMapper.Impl;
using Umbraco.Core.Models;
using uTyped.Resolvers;

namespace uTyped.Profiles
{
    public class UmbracoProfile<T> : Profile
    {
        /// <summary>
        /// Setup the PropertyResolver as ValueResolver by default on all unmapped properties
        /// </summary>
        protected override void Configure()
        {
            var map = Mapper.FindTypeMapFor<IPublishedContent, T>();
            if (null != map)
            {
                foreach (var property in map.GetUnmappedPropertyNames())
                {
                    var member = typeof(T).GetProperty(property);
                    if (null != member)
                    {
                        var memberAccessor = member.ToMemberAccessor();
                        var propertyMap = new PropertyMap(memberAccessor);
                        propertyMap.AssignCustomValueResolver(new PropertyResolver());
                        map.AddPropertyMap(propertyMap);
                    }
                }
            }
            else
            {
                CreateMap<IPublishedContent, T>()
                    .ForAllMembers(c => c.ResolveUsing<PropertyResolver>());
            }
        }

        /// <summary>
        /// Set the profile name to the current type name
        /// </summary>
        public override string ProfileName
        {
            get { return GetType().Name; }
        }
    }
}
