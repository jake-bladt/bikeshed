using System;
using System.Collections.Generic;
using System.Linq;

namespace runoffs
{
    public static class ListExtensions
    {
        private static Random rng = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        //public static IList<T> GetRandom<T>(this IList<t> list, int pullCount)
        //{
        //    return list.OrderBy(x => 0);
        //}
    }
}
