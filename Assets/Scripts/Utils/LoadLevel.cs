using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Ketsu.Utils;

public class LoadLevel : MonoBehaviour
{
    public string SfxMapEnv;

    [Serializable]
    public struct SceneToMusicEvent
    {
        public string SceneName;
        public string EventName;
    }
    public List<SceneToMusicEvent> MusicEvents;

	private void Start()
	{
		ScreenFaider.Instance.FadeOut(1.0f, null);
	}

	public void LoadOn(int level)
	{
		ScreenFaider.Instance.FadeIn(1.0f, () => SceneManager.LoadScene(level));
	}

	public void NextLevel()
	{
	    foreach (SceneToMusicEvent musicEvent in MusicEvents)
	    {
	        if (musicEvent.SceneName.Equals(SceneManager.GetActiveScene().name))
	        {
	            AkSoundEngine.PostEvent(musicEvent.EventName, gameObject);
	            break;
	        }
	    }
	    AkSoundEngine.PostEvent(SfxMapEnv, gameObject);

        int level = SceneManager.GetActiveScene().buildIndex;
		ScreenFaider.Instance.FadeIn(1.0f, () => SceneManager.LoadScene(level + 1));
	}

	public void ReloadLevel()
	{
		int level = SceneManager.GetActiveScene().buildIndex;
		ScreenFaider.Instance.FadeIn(1.0f, () => SceneManager.LoadScene(level));
	}
}