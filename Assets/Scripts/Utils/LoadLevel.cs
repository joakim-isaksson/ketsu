using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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

    public void LoadOn(int level)
	{
		SceneManager.LoadScene(level);
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
		SceneManager.LoadScene(level + 1);
	}

	public void ReloadLevel()
	{
		int level = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(level);
	}
}