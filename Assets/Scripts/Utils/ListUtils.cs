using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Utils
{
    public class ListUtils
    {
        /// <summary>
        /// Shuffle given list using Fisher–Yates algorithm
        /// </summary>
        /// <param name="list">List to be shuffled</param>
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}