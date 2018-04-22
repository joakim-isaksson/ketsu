using Ketsu.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
	public class LoadSceneOnEscape : MonoBehaviour
	{
		public string SceneName;
		
		private void Update()
		{
			if (Input.GetKey("escape"))
			{
				ScreenFaider.Instance.FadeIn(1.0f, () => SceneManager.LoadScene(SceneName));
			}
		}
	}
}