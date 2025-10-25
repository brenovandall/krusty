using System.Collections.Immutable;

namespace Krusty.Api.Infrastructure
{
    internal static class MemoryContext<T>
    {
        private static readonly object _lock = new();
        private static ImmutableList<T> _bucket = [];

        public static void AddElement(T value)
        {
            lock (_lock)
            {
                _bucket = _bucket.Add(value);
            }
        }

        public static void ClearBucket()
        {
            lock (_lock)
            {
                _bucket = _bucket.Clear();
            }
        }

        public static IEnumerable<T> GetElements(Func<T, bool>? predicate = null)
        {
            var lazyData = _bucket;

            if (predicate == null)
                return lazyData;

            return lazyData.Where(predicate);
        }

        public static void RemoveElement(T element)
        {
            lock (_lock)
            {
                _bucket = _bucket.Remove(element);
            }
        }
    }
}
