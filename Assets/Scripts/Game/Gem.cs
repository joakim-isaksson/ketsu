using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game
{
    public class Gem : MonoBehaviour
    {
        public GameObject ExplosionPrefab;
        public string SfxCollect;

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

            AkSoundEngine.PostEvent(SfxCollect, gameObject);

            Instantiate(ExplosionPrefab, transform.position, transform.rotation);

            if(SceneManager.GetActiveScene().buildIndex - 1 < 7)
                StartCoroutine(lerpPos(transform.position, new Vector3(8f,23f,8f), 0.7f));
            else{
                characterHandler.FillKetsuPower();
                Destroy(gameObject);
            }
        }

        IEnumerator lerpPos(Vector3 start, Vector3 end, float t){
            float StartTime = Time.time;
            while(Time.time < StartTime+t){
                float timeProgressed = (Time.time - StartTime) / t;
                float x = Mathf.Lerp(start.x, end.x, timeProgressed);
                float y = Mathf.Lerp(start.y, end.y, timeProgressed);
                float z = Mathf.Lerp(start.z, end.z, timeProgressed);
                transform.position = new Vector3(x,y,z);
                yield return new WaitForFixedUpdate();
            }
            characterHandler.FillKetsuPower();
            Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);

        }
    }
}