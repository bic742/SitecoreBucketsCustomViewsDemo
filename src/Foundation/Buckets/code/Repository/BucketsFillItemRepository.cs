using Foundation.Buckets.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Buckets.Repository
{
    public class BucketsFillItemRepository : IBucketsFillItemRepository
    {
        private const string FillItemRootFolderSettingName = "Foundation.Buckets.FillItemFolder";

        public Dictionary<ID, List<string>> GetFillItems()
        {
            var items = new Dictionary<ID, List<string>>();
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var fillItemFolderId = Sitecore.Configuration.Settings.GetSetting(FillItemRootFolderSettingName);
            var fillItemFolder = database.GetItem(fillItemFolderId);

            if (fillItemFolder == null)
            {
                Log.Warn($"BucketsFillItemRepository - Unable to located root folder: {fillItemFolderId}", this);
                return items;
            }

            var fillItems = GetFillItems(fillItemFolder);

            if (fillItems.Any() == false)
            {
                return items;
            }

            items = fillItems
                .Select(doc => new
                {
                    TemplateId = doc.ResultTemplate,
                    doc.SolrFieldName
                })
                .GroupBy(result => result.TemplateId)
                .ToDictionary(result => result.Key, result => result.Select(r => r.SolrFieldName).ToList());

            return items;
        }

        protected IEnumerable<FillItemSearchResultItem> GetFillItems(Item root)
        {
            using (var searchContext = ContentSearchManager.GetIndex((SitecoreIndexableItem)root).CreateSearchContext())
            {
                var queryable = searchContext.GetQueryable<FillItemSearchResultItem>();

                queryable = queryable
                    .Where(result => result.Paths.Contains(root.ID))
                    .Where(result => result.TemplateId == new ID(Templates.FillItem.TemplateId));

                var results = queryable.GetResults().Select(data => data.Document);

                return results;
            }
        }
    }
}