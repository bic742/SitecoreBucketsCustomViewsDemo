using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Linq;

namespace Foundation.Buckets.Indexing.Computed
{
    public class ComputedResultTemplate : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item currentItem = indexable as SitecoreIndexableItem;

            if (currentItem.TemplateID.Equals(new ID(Templates.FillItem.TemplateId)) == false)
            {
                return null;
            }

            var template = ((MultilistField)currentItem.Fields[Templates.FillItem.Fields.ResultTemplate]).GetItems().FirstOrDefault();

            return template?.ID;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}