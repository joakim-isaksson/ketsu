using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using System.Linq;

/* Map parser executes once when you open the scene,
 * if the scene is new, i.e. has not been initialized before.
 * 
 * If the scene has been initialized before, the easiest way to run 
 * Map parser again is to uncheck Initialized and duplicate the 
 * GameObject holding the Map parser.
 *
 * The Map file used must be in .json format,
 * and is loaded based on user's selection.
 */

[ExecuteInEditMode]
public class ParseMap : MonoBehaviour {

	//map structure to hold information
	public class Map {
		public int height;
    	public int width;
    	public int amountOfTiles;
    	//dictionary of <layer name, layer data>
    	public Dictionary<string, int[,]> tiles;
    	//public List<int[,]> tiles;
    	//public List<string> layers;
	}

	#if UNITY_EDITOR
    [SerializeField]
    private bool initialized = false;
 
    void Awake() { Initialize(); }
 
    void Initialize()
    {
        if (initialized)
        	return;
 
		//init new map
		Map map = new Map();
		//map.layers = new List<string>();
		//map.tiles = new List<int[,]>();
		map.tiles = new Dictionary<string, int[,]>();
		string tileString = "";
		string layername = "";
		

		//read json file
		string line = null;

		//which map to read, ask user
		string path = EditorUtility.OpenFilePanel("Map file to load", "Assets/Maps/", "json");
		StreamReader reader = new StreamReader(path);
		while ((line = reader.ReadLine()) != null) {	        	
			int start = line.IndexOf(":", 0)+1;
        	int end = line.IndexOf(",", start);

	        if(line.Contains("data")){
	        	tileString = line.Substring(start+1, line.LastIndexOf(',')-2-start);

				while ((line = reader.ReadLine()) != null) {
			        //parse the variables
					start = line.IndexOf(":", 0)+1;
		        	end = line.IndexOf(",", start);

		        	//layer name
					if(line.Contains("name"))
			        	layername = line.Substring(start+1, line.LastIndexOf(',')-2-start);
			   		//height & width for the map
			        if(line.Contains("\"height") && map.height == 0)
			        	int.TryParse(line.Substring(start, end - start), out map.height);
			        if(line.Contains("\"width")){
			        	if(map.width == 0){
			        		int.TryParse(line.Substring(start, end - start), out map.width);
			        		break;
			        	}
			        	else
			        		break;
			        }
			        
				}

				//parse the string of tiles to an actual array
			    int[,] layerdata = parseTiles(tileString, map.width, map.height);
				map.tiles.Add(layername, layerdata);
	        }
	        if(line.Contains("tilecount"))
	        	int.TryParse(line.Substring(start, end - start), out map.amountOfTiles);
	        
	    }
	    //instantiate GameObjects via prefabs
	    createObjects(map.tiles, map.width, map.height);

        initialized = true;
	}

	int[,] parseTiles(string tileString, int x, int y){
		tileString = Regex.Replace(tileString, @"\s+", "");
		string[] substrings = tileString.Split(',');

		int[,] tiles = new int[y, x];
		for(int i=0; i<y; i++)
			for(int j=0; j<x; j++)
				int.TryParse(substrings[i*x+j], out tiles[i,j]);
		
		return tiles;
	}

	Dictionary<int, GameObject> loadPrefabsFromFile(){
		Dictionary<int, GameObject> prefabs = new Dictionary<int, GameObject>();

		StreamReader reader = new StreamReader("Assets/Maps/tiles.txt");
		string line = null;

		while ((line = reader.ReadLine()) != null) {
			int n = 0;
			int.TryParse(line.Substring(0, line.IndexOf(' ')), out n);
			string name = line.Substring(line.LastIndexOf(' ')+1);

			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Maps/Prefabs/"+name+".prefab", typeof(Object));
			prefabs.Add(n, prefab);
		}	

		return prefabs;
	}

	void createObjects(Dictionary<string, int[,]> tiles, int x, int y){

		Dictionary<int, GameObject> prefabs = loadPrefabsFromFile();
		//Create correct objects
        foreach(KeyValuePair<string, int[,]> entry in tiles){
        	//todo:
        	//-prefabit nollaan, tarvittaessa muutetaan parenttia
        	//-tyhjät gameobjectit parenteiksi
        	//-muuta dictionaryksi

        	//Instantiate ground things
			if(entry.Key=="Ground"){
				GameObject GroundParent = new GameObject();
				GroundParent.name = "Ground";
				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						for(int k=1; k<=30; k++)
							if(prefabs.ContainsKey(k) && entry.Value[i,j]==k){
								GameObject item = Instantiate(prefabs[k], new Vector3(-j+x-1, 0, i), Quaternion.identity);
								item.transform.parent = GroundParent.transform;
							}	
					}
				}
			}
			 
			//Instantiate obstacles
			if(entry.Key=="Obstacles"){
				GameObject ObstacleParent = new GameObject();
				ObstacleParent.name = "Obstacles";
				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						for(int k=31; k<=80; k++)
							if(prefabs.ContainsKey(k) && entry.Value[i,j]==k){
								GameObject item = Instantiate(prefabs[k], new Vector3(-j+x-1, 0, i), Quaternion.identity);
								item.transform.parent = ObstacleParent.transform;
							}
					}
				}

			}
			//Instantiate objects = spawn points etc.
			if(entry.Key=="Objects"){
				GameObject ObjectParent = new GameObject();
				ObjectParent.name = "Objects";
				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						if(prefabs.ContainsKey(81) && entry.Value[i,j] == 81){
							GameObject item = Instantiate(prefabs[81], new Vector3(-j+x-1, 0, i), Quaternion.identity);
							item.transform.parent = ObjectParent.transform;
						}
						if(prefabs.ContainsKey(82) && entry.Value[i,j] == 82){
							GameObject item = Instantiate(prefabs[82], new Vector3(-j+x-1, 0, i), Quaternion.identity);
							item.transform.parent = ObjectParent.transform;
						}
						if(prefabs.ContainsKey(181) && entry.Value[i,j] == 181){
							GameObject item = Instantiate(prefabs[181], new Vector3(-j+x-1, 0, i), Quaternion.identity);
							item.transform.parent = ObjectParent.transform;
						}
						if(prefabs.ContainsKey(182) && entry.Value[i,j] == 182){
							GameObject item = Instantiate(prefabs[182], new Vector3(-j+x-1, 0, i), Quaternion.identity);
							item.transform.parent = ObjectParent.transform;
						}
					}
				}
			}
		}
	}
	#endif

}

