using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        public float Seconds;

        void Awake()
        {
            StartCoroutine(DestroyAfterDelay());
        }

        IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(Seconds);
            Destroy(gameObject);
        }
    }
}