using UnityEngine;

namespace Ketsu.Game
{
    public class Bumber : MonoBehaviour
    {
        EnemyController controller;

        public void Awake()
        {
            controller = GetComponentInParent<EnemyController>();
        }

        void OnTriggerEnter(Collider other)
        {
            controller.OnBumb(other);
        }
    }
}

