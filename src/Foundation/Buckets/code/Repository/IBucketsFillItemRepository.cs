using Sitecore.Data;
using System.Collections.Generic;

namespace Foundation.Buckets.Repository
{
    public interface IBucketsFillItemRepository
    {
        Dictionary<ID, List<string>> GetFillItems();
    }
}