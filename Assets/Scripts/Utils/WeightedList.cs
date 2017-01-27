using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Utils.WeightedList
{
    public class WeightedList<T>
    {
        List<T> objects;
        List<float> cumulativeWeights;
        float maxWeight;

        public WeightedList(List<float> weights, List<T> objs)
        {
            objects = new List<T>();
            cumulativeWeights = new List<float>();

            float cumulativeWeight = 0;
            for (int i = 0; i < weights.Count; ++i)
            {
                cumulativeWeight += weights[i];
                cumulativeWeights.Add(cumulativeWeight);
                objects.Add(objs[i]);
            }
            maxWeight = cumulativeWeight;
        }

        /// <summary>
        /// Get a random object from the list using weighted probabilities
        /// </summary>
        /// <returns>random object from the list</returns>
        public T GetRandom()
        {
            int index = cumulativeWeights.BinarySearch(Random.value * maxWeight);
            if (index < 0) index = ~index;
            return objects[index];
        }
    }
}