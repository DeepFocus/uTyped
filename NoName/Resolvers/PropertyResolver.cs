﻿using AutoMapper;
using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace NoName.Resolvers
{
    public class PropertyResolver : IValueResolver
    {
        private string _propertyName;

        public PropertyResolver()
        {
        }

        /// <summary>
        /// When the property name in the destination class doesn't match the property name in the Umbraco property, use this constructor
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public PropertyResolver(string propertyName = null)
        {
            _propertyName = propertyName;
        }

        /// <summary>
        /// Resolve values from Umbraco content properties based on the name of the destination property
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ResolutionResult Resolve(ResolutionResult source)
        {
            _propertyName = _propertyName ?? source.Context.MemberName;

            var content = source.Context.SourceValue as IPublishedContent;
            if (null != content && content.HasProperty(_propertyName))
            {
                return source.New(content.GetPropertyValue(_propertyName));
            }

            var prop = typeof (IPublishedContent).GetProperty(_propertyName);
            if (null != prop) //Last chance, if the property is part of the IPublishedContent object
            {
                return source.New(prop.GetValue(content));
            }

            var destType = source.Context.DestinationType;
            return source.New(destType.IsValueType ? Activator.CreateInstance(destType) : null);
        }
    }
}
