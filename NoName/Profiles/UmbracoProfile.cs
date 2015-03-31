using AutoMapper;
using AutoMapper.Impl;
using NoName.Resolvers;
using Umbraco.Core.Models;

namespace NoName.Profiles
{
    public class UmbracoProfile<T> : Profile
    {
        protected override void Configure()
        {
            var map = Mapper.FindTypeMapFor<IPublishedContent, T>();

            if (null != map)
            {
                foreach (var property in map.GetUnmappedPropertyNames())
                {
                    var m = typeof(T).GetProperty(property);
                    if (null != m)
                    {
                        IMemberAccessor memberAccessor = m.ToMemberAccessor();

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
        public override string ProfileName
        {
            get { return GetType().Name; }
        }
    }
}
