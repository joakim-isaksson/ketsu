using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Utils
{
    [System.Serializable]
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
            List<float> cumulativeWeights = new List<float>();
            float cumulativeWeight = 0;
            foreach (WeightedGameObject wObj in Replacements)
            {
                cumulativeWeight += wObj.Weight;
                cumulativeWeights.Add(cumulativeWeight);
            }
            float maxWeight = cumulativeWeight;

            int index = cumulativeWeights.BinarySearch(Random.value * maxWeight);
            if (index < 0) index = ~index;

            Instantiate(Replacements[index].Object, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}