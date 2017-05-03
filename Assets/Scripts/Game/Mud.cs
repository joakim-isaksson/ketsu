using UnityEngine;

namespace Game
{
    public class Mud : MonoBehaviour
    {
        public GameObject SplatterPrefab;

        void OnTriggerEnter(Collider other)
        {
            MapObject obj = other.GetComponent<MapObject>();
            if (obj != null && (obj.Type == MapObjectType.Fox || obj.Type == MapObjectType.Wolf || obj.Type == MapObjectType.Ketsu))
            {
                Instantiate(SplatterPrefab, transform.position, transform.rotation);
            }
        }
    }
}