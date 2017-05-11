using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game
{
    public class Gas : MonoBehaviour
    {
        public string SfxCollectDefault;
        public string SfxCollectLast;

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

            if (GasToCollect > 0) AkSoundEngine.PostEvent(SfxCollectDefault, gameObject);
                else AkSoundEngine.PostEvent(SfxCollectLast, gameObject);

            Instantiate(ExplosionPrefab, transform.position, transform.rotation);

            if(SceneManager.GetActiveScene().buildIndex - 1 < 4)
                StartCoroutine(lerpPos(transform.position, new Vector3(9f,23f,8f), 1f));
            else{
                GasCollected++;
                GasToCollect--;

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
            GasCollected++;
            GasToCollect--;
            Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);

        }
    }
}