using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped
{
    public class UmbracoRepository
    {
        private readonly UmbracoHelper _umbraco;

        public UmbracoRepository(UmbracoHelper umbracoHelper)
        {
            _umbraco = umbracoHelper;
        }

        /// <summary>
        /// Returns strongly typed elements for which the class name matches the type in Umbraco.
        /// xPath parameter can be used to specify a different XPath.
        /// Mapping IPublishedContent to Type T is done using AutoMapper.
        /// </summary>
        /// <param name="xPath">Path to the node. If not provided, the output class name is used</param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string xPath = null)
        {
            return GetAll<T>(Mapper.Map<T>, xPath);
        }

        /// <summary>
        /// Returns strongly typed elements for which the class name matches the type in Umbraco.
        /// xPath parameter can be used to specify a different XPath.
        /// </summary>
        /// <param name="mapper">Function mapping the IPublishedContent to the expected out Type T</param>
        /// <param name="xPath">Path to the node. If not provided, the output class name is used</param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(Func<IPublishedContent, T> mapper, string xPath = null)
        {
            var collection = _umbraco.TypedContentAtXPath(xPath ?? string.Format("//{0}", typeof(T).Name));
            return collection.Select(mapper);
        }

        /// <summary>
        /// Returns typed elements from Umbraco mapped to the requested class
        /// Mapping IPublishedContent to Type T is done using AutoMapper.
        /// </summary>
        /// <param name="ids">List of the elements Ids to retrieve</param>
        /// <returns></returns>
        public IEnumerable<T> GetById<T>(IEnumerable<int> ids)
        {
            return ids.Select(GetById<T>);
        }

        /// <summary>
        /// Returns typed elements from Umbraco mapped to the requested class
        /// </summary>
        /// <param name="ids">List of the elements Ids to retrieve</param>
        /// <param name="mapper">Function mapping the IPublishedContent to the expected out Type T</param>
        /// <returns></returns>
        public IEnumerable<T> GetById<T>(IEnumerable<int> ids, Func<IPublishedContent, T> mapper)
        {
            return ids.Select(id => GetById<T>(id, mapper));
        }

        /// <summary>
        /// Returns a typed element from Umbraco mapped to Type T.
        /// Mapping IPublishedContent to Type T is done using AutoMapper.
        /// </summary>
        /// <param name="id">Id of the element to retrieve</param>
        /// <returns></returns>
        public T GetById<T>(int id)
        {
            return GetById<T>(id, Mapper.Map<T>);
        }

        /// <summary>
        /// Returns a typed element from Umbraco mapped to Type T
        /// </summary>
        /// <param name="id">Id of the element to retrieve</param>
        /// <param name="mapper">Function mapping the IPublishedContent to the expected out Type T</param>
        /// <returns></returns>
        public T GetById<T>(int id, Func<IPublishedContent, T> mapper)
        {
            return mapper(_umbraco.TypedContent(id));
        }
    }
}
