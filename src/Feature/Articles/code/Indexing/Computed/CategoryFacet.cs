using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Linq;

namespace Feature.Articles.Indexing.Computed
{
    public class CategoryFacet : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item currentItem = indexable as SitecoreIndexableItem;

            if (currentItem.TemplateID.Equals(new ID(Templates.Article.TemplateId)) == false)
            {
                return null;
            }

            var category = ((MultilistField) currentItem.Fields[Templates.Article.Fields.Category]).GetItems().FirstOrDefault();

            return category?.Fields[Templates.ArticleCategory.Fields.Title].Value;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}