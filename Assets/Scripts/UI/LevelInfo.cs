using Game;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


namespace Ketsu.UI
{
	public class LevelInfo : MonoBehaviour {

		int level=0;
		int n=0;
		public Text LevelName;
		public Text LevelNumber;
		// Use this for initialization
		void Start () {
			level = SceneManager.GetActiveScene().buildIndex-1;
			TextAsset file = Resources.Load("levels") as TextAsset;
			string path = file.text;

			StringReader reader = new StringReader(path);
			string line = null;

			while ((line = reader.ReadLine()) != null)
			{
				int.TryParse(line.Substring(0, line.IndexOf(' ')), out n);
				if(n == level){
					string name = line.Substring(line.IndexOf(' ') + 1);
					LevelName.text = name;
					LevelNumber.text = "0"+(level-1)+"/30";
					break;
				}

			}
		}
	}
}