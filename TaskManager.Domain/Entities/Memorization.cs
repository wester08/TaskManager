using System.Collections.Concurrent;

namespace TaskManager.Domain.Entities
{
    public static class Memoization
    {
      
        private static readonly ConcurrentDictionary<string, object> _cache = new();

        public static TResult GetOrAdd<TResult>(string key, Func<TResult> valueFactory)
        {
            return (TResult)_cache.GetOrAdd(key, _ => valueFactory()!);
        }

        public static void Clear(string key)
        {
            _cache.TryRemove(key, out _);
        }

        public static void ClearAll()
        {
            _cache.Clear();
        }
    }
}
