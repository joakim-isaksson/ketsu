using UnityEngine;

namespace Game
{
    public class Gas : MonoBehaviour
    {
        public static int GasToCollect;
        public static int GasCollected;

        void Awake()
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

            Destroy(gameObject);
        }
    }
}