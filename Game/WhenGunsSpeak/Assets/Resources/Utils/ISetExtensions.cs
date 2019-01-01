using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class ISetExtensions
    {
        public static void SynchronizeWith<T>(this ISet<T> currentItems, ISet<T> newItems, Action<T> onAdd, Action<T> onDelete)
        {
            newItems.Except(currentItems)
                    .ToList()
                    .ForEach(itemToAdd =>
                    {
                        currentItems.Add(itemToAdd);
                        onAdd(itemToAdd);
                    });

            currentItems.Except(newItems)
                        .ToList()
                        .ForEach(itemToDelete =>
                        {
                            currentItems.Remove(itemToDelete);
                            onDelete(itemToDelete);
                        });
        }
    }
}
