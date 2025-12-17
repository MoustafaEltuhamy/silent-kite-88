using System;
using System.Collections.Generic;


namespace AGAPI.Foundation
{
    public static class ListExtensions
    {
        private static readonly Random Random = new Random();

        // Shuffles the elements of the list in place.
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                // Pick a random index from the remaining elements
                int randomIndex = i + Random.Next(n - i);

                // Swap the elements at the current index and the random index
                (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
            }
        }
    }
}
