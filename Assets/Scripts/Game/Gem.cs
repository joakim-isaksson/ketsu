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

		private bool Collected;

		void Start ()
		{
			characterHandler = FindObjectOfType<CharacterHandler> ();
		}

		void OnTriggerEnter (Collider other)
		{
			if (Collected)
				return;
			Collected = true;

			var character = other.GetComponent<Character> ();
			if (character == null) return;
			characterHandler.FillKetsuPower(character);
			
			AkSoundEngine.PostEvent (SfxCollect, gameObject);

			Instantiate (ExplosionPrefab, transform.position, transform.rotation);
			
			StartCoroutine (lerpPos (transform.position, new Vector3 (8f, 23f, 8f), 0.6f));
		}

		IEnumerator lerpPos (Vector3 start, Vector3 end, float t)
		{
			float StartTime = Time.time;
			while (Time.time < StartTime + t) {
				float timeProgressed = (Time.time - StartTime) / t;
				float x = Mathf.Lerp (start.x, end.x, timeProgressed);
				float y = Mathf.Lerp (start.y, end.y, timeProgressed);
				float z = Mathf.Lerp (start.z, end.z, timeProgressed);
				transform.position = new Vector3 (x, y, z);
				yield return new WaitForFixedUpdate ();
			}
			Instantiate (ExplosionPrefab, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}