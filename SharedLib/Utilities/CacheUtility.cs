using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Utilities
{
    public static class CacheUtility<Tidentifier>
    {
        public static TItem UpdateCache<TItem>(ConcurrentDictionary<Tidentifier, TItem> cache, Tidentifier id, TItem item)
        {
            if (cache.TryGetValue(id, out TItem oldItem))
            {
                if (cache.TryUpdate(id, item, oldItem))
                {
                    return item;
                }
            }
            return default;
        }
    }
}
