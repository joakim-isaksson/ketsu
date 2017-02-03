using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

/* Map parser executes once when you open the scene,
 * if the scene is new, i.e. has not been initialized before.
 * 
 * If the scene has been initialized before, the easiest way to run 
 * Map parser again is to duplicate the GameObject holding it.
 *
 * The Map file used must be in .json format,
 * and for now is only specified in code, will be changed later.
 */

[ExecuteInEditMode]
public class ParseMap : MonoBehaviour {
	public Transform Grass;
	public Transform Water;
	public Transform Fence;
	public Transform Tree;
	public Transform FoxSpawn;
	public Transform WolfSpawn;
	public Transform FoxCave;
	public Transform WolfCave;
	public Transform Flower;

	//map structure to hold information
	public class Map {
		public int height;
    	public int width;
    	public int amountOfTiles;
    	public List<int[,]> tiles;
    	public List<string> layers;
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
		map.layers = new List<string>();
		map.tiles = new List<int[,]>();
		string tileString = "";

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
			        	map.layers.Add(line.Substring(start+1, line.LastIndexOf(',')-2-start));
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
			    map.tiles.Add(parseTiles(tileString, map.width, map.height));
	        }
	        if(line.Contains("tilecount"))
	        	int.TryParse(line.Substring(start, end - start), out map.amountOfTiles);
	        
	    }
	    //instantiate GameObjects via prefabs
	    for(int i=0; i<map.tiles.Count; i++)
	    	createObjects(map.layers, map.tiles[i], map.width, map.height);

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

	GameObject[] loadPrefabsFromFolder(string directory){
		string[] filenames = Directory.GetFiles(directory);
		List<string> files = new List<string>();

		for(int i=0; i<filenames.Length; i++)
			if(!filenames[i].Contains(".meta"))
				files.Add(filenames[i]);

		GameObject[] prefabs = new GameObject[files.Count];

		for(int i=0; i<files.Count; i++){
			prefabs[i] = (GameObject)AssetDatabase.LoadAssetAtPath(files[i],typeof(Object));
		}

		return prefabs;
	}

	void createObjects(List<string> layers, int[,] tiles, int x, int y){
		//string pathGround = EditorUtility.OpenFolderPanel("Prefabs for Ground Tiles", "Assets/Maps", "");
		//string pathObstacles = EditorUtility.OpenFolderPanel("Prefabs for Obstacle Tiles", "Assets/Maps", "");
		//string pathObjects = EditorUtility.OpenFolderPanel("Prefabs for Object Tiles", "Assets/Maps", "");
		//pathGround = pathGround.Substring(pathGround.IndexOf("Assets"));
		//pathObstacles = pathObstacles.Substring(pathObstacles.IndexOf("Assets"));
		//pathObjects = pathObjects.Substring(pathObjects.IndexOf("Assets"));

		//Create correct objects
        for (int n=0; n<layers.Count; n++){
        	//todo:
        	//-prefabit nollaan, tarvittaessa muutetaan parenttia
        	//-tyhjät gameobjectit parenteiksi
        	//-muuta dictionaryksi

        	//Instantiate ground things
			if(layers[n]=="Ground"){
				//GameObject[] prefabs = loadPrefabsFromFolder(pathGround);
				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						if(tiles[i,j]==1)
							Instantiate(Grass, new Vector3(-j+x-1, 0, i), Quaternion.identity);
						if(tiles[i,j]==3)
							Instantiate(Flower, new Vector3(-j+x-1, 0, i), Quaternion.identity);
						if(tiles[i,j]==5)
							Instantiate(Water, new Vector3(-j+x-1, 0, i), Quaternion.identity);
					}
				}
			} 
			//Instantiate obstacles
			if(layers[n]=="Obstacles"){
				//GameObject[] prefabs = loadPrefabsFromFolder(pathObstacles);

				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						if(tiles[i,j]==41)
							Instantiate(Fence, new Vector3(-j+x-1, 0, i), Quaternion.identity);
						if(tiles[i,j]==42)
							Instantiate(Tree, new Vector3(-j+x-1, 0, i), Quaternion.identity);
					}
				}

			}
			//Instantiate objects = spawn points etc.
			if(layers[n]=="Objects"){
				//NOTE TO SELF: you cannot choose prefab folder by asking the user
				//must use relative paths! Just have to add the correct folders here.
				//GameObject[] prefabs = loadPrefabsFromFolder(pathObjects);

				for(int i=0; i<y; i++){
					for(int j=0; j<x; j++){
						if(tiles[i,j]==182)
							Instantiate(WolfCave, new Vector3(-j+x-1, 0, i), Quaternion.identity);
						if(tiles[i,j]==82)
							Instantiate(WolfSpawn, new Vector3(-j+x-1, 0, i), Quaternion.identity);
						if(tiles[i,j]==181)
							Instantiate(FoxCave, new Vector3(-j+x-1, 0, i), Quaternion.identity);
						if(tiles[i,j]==81)
							Instantiate(FoxSpawn, new Vector3(-j+x-1, 0, i), Quaternion.identity);
					}
				}
			}
		}
	}
	#endif

}

