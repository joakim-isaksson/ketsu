using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    [Serializable]
    public struct WeightedGameObject
    {
        public GameObject Object;
        public float Weight;
    }

    public class PlaceHolder : MonoBehaviour
    {
        public WeightedGameObject[] Replacements;

        void Awake()
        {
            var cumulativeWeights = new List<float>();
            float cumulativeWeight = 0;
            foreach (var wObj in Replacements)
            {
                cumulativeWeight += wObj.Weight;
                cumulativeWeights.Add(cumulativeWeight);
            }
            var maxWeight = cumulativeWeight;

            var index = cumulativeWeights.BinarySearch(Random.value * maxWeight);
            if (index < 0) index = ~index;

            Instantiate(Replacements[index].Object, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}