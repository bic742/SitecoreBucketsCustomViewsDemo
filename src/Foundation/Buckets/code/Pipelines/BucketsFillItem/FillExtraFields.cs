using Foundation.Buckets.Repository;
using Sitecore.Buckets.Pipelines.UI.FillItem;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundation.Buckets.Pipelines.BucketsFillItem
{
    public class FillExtraFields : FillItemProcessor
    {
        protected readonly IBucketsFillItemRepository BucketsFillItemRepository;

        public FillExtraFields(IBucketsFillItemRepository bucketsFillItemRepository)
        {
            BucketsFillItemRepository = bucketsFillItemRepository;
        }

        public override void Process(FillItemArgs args)
        {
            var fillItems = BucketsFillItemRepository.GetFillItems();

            if (fillItems.Any() == false)
            {
                return;
            }

            foreach (var searchResultItem in args.ResultItems.OfType<SitecoreUISearchResultItem>())
            {
                var templateId = new ID(searchResultItem.TemplateId);

                if (!fillItems.ContainsKey(templateId))
                {
                    continue;
                }

                var fields = fillItems[templateId];
                var dynamicFields = new List<KeyValuePair<string, string>>();

                foreach (var field in fields)
                {
                    var fieldValue = searchResultItem.Fields.FirstOrDefault(resultField => resultField.Key == field).Value;
                    var dynamicFieldKey = GetFieldKey(field);

                    dynamicFields.Add(new KeyValuePair<string, string>(dynamicFieldKey, fieldValue?.ToString() ?? string.Empty));
                }

                searchResultItem.DynamicFields.AddRange(dynamicFields);
            }
        }

        /// <summary>
        /// Converts a solr field name to a usable DynamicPlaceholder name for rendering in a buckets view.
        /// </summary>
        /// <param name="field">Solr field name (e.g. category_facet)</param>
        /// <returns>Title case version of name, with no underscores (e.g. CategoryFacet)</returns>
        protected virtual string GetFieldKey(string field)
        {
            var fieldParts = field.Split('_');
            var builder = new StringBuilder();

            foreach (var fieldPart in fieldParts)
            {
                builder.Append(char.ToUpper(fieldPart[0]));
                builder.Append(fieldPart.Substring(1));
            }

            return builder.ToString();
        }
    }
}