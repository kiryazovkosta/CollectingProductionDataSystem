namespace CollectingProductionDataSystem.Contracts.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> collectionToRemove)
        {
            collectionToRemove.ForEach((item) => { collection.Remove(item); });
        }
    }
}
