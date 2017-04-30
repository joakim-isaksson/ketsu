using UnityEngine;

namespace Game
{
    public class Gas : MonoBehaviour
    {
        public static int GasToCollect;
        public static int GasCollected;

        public GameObject ExplosionPrefab;

        void Awake()
        {
            GasToCollect = 0;
            GasCollected = 0;
        }

        void Start()
        {
            GasToCollect++;
        }

        void OnTriggerEnter(Collider other)
        {
            var otherType = other.GetComponent<MapObject>().Type;
            if (otherType != MapObjectType.Fox && otherType != MapObjectType.Wolf &&
                otherType != MapObjectType.Ketsu) return;

            GasCollected++;
            GasToCollect--;

            Instantiate(ExplosionPrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}