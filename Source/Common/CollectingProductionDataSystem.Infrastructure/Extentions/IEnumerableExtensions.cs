namespace CollectingProductionDataSystem.Infrastructure.Extentions
{
    using System;
    using System.Collections.Generic;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }

            return collection;
        }

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> collectionToRemove)
        {
            collectionToRemove.ForEach((item) => { collection.Remove(item); });
        }
    }
}
