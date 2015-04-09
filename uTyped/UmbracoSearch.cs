using AutoMapper;
using Examine;
using Examine.LuceneEngine.SearchCriteria;
using Examine.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uTyped
{
    public class UmbracoSearch
    {
        private readonly ExamineManager _examine;
        private readonly UmbracoHelper _umbracoHelper;

        public UmbracoSearch(UmbracoHelper umbracoHelper)
            : this(umbracoHelper, ExamineManager.Instance) { }

        public UmbracoSearch(UmbracoHelper umbracoHelper, ExamineManager examine)
        {
            _examine = examine;
            _umbracoHelper = umbracoHelper;
        }

        /// <summary>
        /// Search for the term and returns an IEnumerable of T
        /// </summary>
        /// <param name="term">Term to search</param>
        /// <param name="fuzzy">The fuzzieness level</param>
        /// <param name="xPath">Path to the node. If not provided, the output class name is used</param>
        /// <returns></returns>
        public IEnumerable<T> Search<T>(string term, float fuzzy = 0.5f, string xPath = null)
        {
            return Search<T>(term, fuzzy, Mapper.Map<IEnumerable<T>>, xPath);
        }

        /// <summary>
        /// Search for the term and returns an IEnumerable of T
        /// </summary>
        /// <param name="term">Term to search</param>
        /// <param name="fuzzy">The fuzzieness level</param>
        /// <param name="mapper">Function mapping the IPublishedContent to the expected out Type T</param>
        /// <param name="xPath">Path to the node. If not provided, the output class name is used</param>
        /// <returns></returns>
        public IEnumerable<T> Search<T>(string term, float fuzzy, Func<IEnumerable<IPublishedContent>, IEnumerable<T>> mapper, string xPath = null)
        {
            //First getting a sample to retrieve all document type properties
            var content = _umbracoHelper.TypedContentSingleAtXPath(xPath ?? string.Format("//{0}", typeof(T).Name));
            if (null != content)
            {
                var properties = content.ContentType.PropertyTypes.Select(p => p.PropertyTypeAlias);
                return Search<T>(term, properties, mapper, fuzzy);
            }

            return mapper(null);
        }

        /// <summary>
        /// Search for the term and returns an IEnumerable of T
        /// </summary>
        /// <param name="term">Term to search</param>
        /// <param name="properties">List of properties to apply the fuzziness on and use for the search</param>
        /// <param name="fuzzy">The fuzzieness level</param>
        /// <returns></returns>
        public IEnumerable<T> Search<T>(string term, IEnumerable<string> properties, float fuzzy = 0.5f)
        {
            return Search<T>(term, properties, Mapper.Map<IEnumerable<T>>, fuzzy);
        }

        /// <summary>
        /// Search for the term and returns an IEnumerable of T
        /// </summary>
        /// <param name="term">Term to search</param>
        /// <param name="properties">List of properties to apply the fuzziness on and use for the search</param>
        /// <param name="mapper">Function mapping the IPublishedContent to the expected out Type T</param>
        /// <param name="fuzzy">The fuzzieness level</param>
        /// <returns></returns>
        public IEnumerable<T> Search<T>(string term, IEnumerable<string> properties, Func<IEnumerable<IPublishedContent>, IEnumerable<T>> mapper, float fuzzy = 0.5f)
        {
            var search = _examine.CreateSearchCriteria();
            var q = search.NodeTypeAlias(typeof(T).Name).And().GroupedOr(properties, term.Fuzzy(fuzzy));

            return Search<T>(q.Compile(), mapper);
        }

        /// <summary>
        /// Search for the cirteria and returns an IEnumerable of T
        /// </summary>
        /// <param name="criteria">Criteria to search for</param>
        /// <returns></returns>
        public IEnumerable<T> Search<T>(ISearchCriteria criteria)
        {
            return Search<T>(criteria, Mapper.Map<IEnumerable<T>>);
        }

        /// <summary>
        /// Search for the cirteria and returns an IEnumerable of T
        /// </summary>
        /// <param name="criteria">Criteria to search for</param>
        /// <param name="mapper">Function mapping the IPublishedContent to the expected out Type T</param>
        /// <returns></returns>
        public IEnumerable<T> Search<T>(ISearchCriteria criteria, Func<IEnumerable<IPublishedContent>, IEnumerable<T>> mapper)
        {
            return mapper(_umbracoHelper.TypedSearch(criteria));
        }

        /// <summary>
        /// Search for the term and returns an IEnumerable of IPublishedContent
        /// </summary>
        /// <param name="term">Term to search</param>
        /// <returns></returns>
        public IEnumerable<IPublishedContent> Search(string term)
        {
            return _umbracoHelper.TypedSearch(term);
        }
    }
}
