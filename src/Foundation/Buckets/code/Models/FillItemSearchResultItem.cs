using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System.Collections.Generic;

namespace Foundation.Buckets.Models
{
    public class FillItemSearchResultItem : SearchResultItem
    {
        [IndexField("computed_result_template")]
        public virtual ID ResultTemplate { get; set; }

        [IndexField("solr_field_name")]
        public virtual string SolrFieldName { get; set; }
    }
}