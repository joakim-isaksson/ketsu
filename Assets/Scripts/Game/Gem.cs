using UnityEngine;

namespace Game
{
    public class Gem : MonoBehaviour
    {
        public GameObject ExplosionPrefab;

        CharacterHandler characterHandler;

        void Start()
        {
            characterHandler = FindObjectOfType<CharacterHandler>();
        }

        void OnTriggerEnter(Collider other)
        {
            var otherType = other.GetComponent<MapObject>().Type;
            if (otherType != MapObjectType.Fox && otherType != MapObjectType.Wolf &&
                otherType != MapObjectType.Ketsu) return;

            Instantiate(ExplosionPrefab, transform.position, transform.rotation);

            characterHandler.FillKetsuPower();
            Destroy(gameObject);
        }
    }
}