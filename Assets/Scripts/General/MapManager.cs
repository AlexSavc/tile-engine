using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(Map))]
public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Map map;
    public Vector3 offset;

    public Dictionary<string, GameObject> Tiles;
    public string prefabPath;

    [Header("Markers")]
    public GameObject selectionMarker;
    public GameObject MoveMarker;
    public GameObject attackMarker;
    public float markerOffset;
    [Header("Map Generation")]
    public float noise, scale;

    void Awake()
    {
        map = FindObjectOfType<Map>();
        SetDictionnary();
    }

    public GameObject SpawnAlgorithm(int x, int X, int y, int Y)
    {
        return GetTileObj("plains");
    }

    public void SetDictionnary()
    {
        Tiles = new Dictionary<string, GameObject>();

        //AddPrefabsFrom(prefabPath);
        LogSubDirectories(prefabPath);
    }
    
    public GameObject GetTileObj(string name)
    {
        string n = name.ToLower();

        if(Tiles.ContainsKey(n))
        {
            return Tiles[n];
        }
        else
        return null;
    }
    
    void LogSubDirectories(string directoryPath)
    {
        string[] directories = Directory.GetDirectories(directoryPath);
        AddPrefabsFrom(directoryPath);
        foreach (string s in directories)
        {
            AddPrefabsFrom(s);
            string[] dir = Directory.GetDirectories(s);
            if (dir.Length == 0) continue;
            else LogSubDirectories(s);
        }
    }

    void AddPrefabsFrom(string dirPath)
    {
        string[] paths = Directory.GetFiles(dirPath, "*.prefab");
        foreach(string s in paths)
        {
            // remove "Assets/Resources\" from the path
            string baseFolder = s.Remove(0, 17);

            string str = RemoveExtension(baseFolder, ".prefab");
            GameObject g = Resources.Load(str) as GameObject;
           
            if (g != null && g.GetComponent<Tile>())
            {
                string type = g.GetComponent<Tile>().type;
                Tiles.Add(type.ToLower(), g);
            }
        }
    }

    string RemoveExtension(string path, string extensionWithDot)
    {
        int len = path.Length;
        int extLen = extensionWithDot.Length;

        string s = path.Remove(len - extLen);
        return s;
    }

    void AddPrefab(string path)
    {

    }

    void Spawn()
    {
        
    }
}

/*[Header("Floors")]
   public GameObject smallTile;
   public GameObject bigTile;
   public GameObject euroCarpet;
   public GameObject grass;
   [Space]
   public GameObject plains;
   public GameObject dirt;
   public GameObject sand;
   public GameObject rock;
   public GameObject snow;
   [Header("Walls")]
   public GameObject plasterWall;
   public GameObject glassWall;
   [Header("Stairs")]
   public GameObject stairsDown;
   public GameObject stairsDownCarpet;
   public GameObject stairsUp;
   public GameObject stairsUpCarpet;
   [Header("Buildings")]
   public GameObject villageHouse;
   public GameObject stoneHouse;
   public GameObject townHouse;
   public GameObject lumberHut;
   public GameObject barracks;
   public GameObject mine;
   [Header("Ramparts")]
   public GameObject stoneWall;
   public GameObject pallissade;
   [Header("Infrastructure")]
   public GameObject stoneRoad;
   [Header("Resources")]
   public GameObject wheat;
   public GameObject forest;
   public GameObject goldOre;
   public GameObject fruits;
   [Header("Others")]
   public GameObject river;
   [Header("Furniture")]
   public GameObject table;
   public GameObject pottedPlant;
   [Header("Characters")]
   public GameObject giletJaune;
   public GameObject gendarme;
   public GameObject gendarme_flashball;
   public GameObject gendarme_grenadier;
   [Header("Items")]
   public GameObject grenade;
   [Header("Markers")]*/

/*Tiles.Add("plasterwall", plasterWall);
Tiles.Add("glasswall", glassWall);

Tiles.Add("smalltile", smallTile);
Tiles.Add("bigtile", bigTile);
Tiles.Add("eurocarpet", euroCarpet);
Tiles.Add("grass", grass);

Tiles.Add("stairsdown", stairsDown);
Tiles.Add("stairsdowncarpet", stairsDownCarpet);
Tiles.Add("stairup", stairsUp);
Tiles.Add("stairsupcarpet", stairsDownCarpet);

Tiles.Add("table", table);

Tiles.Add("villagehouse", villageHouse);
Tiles.Add("stonehouse", stoneHouse);
Tiles.Add("townhouse", townHouse);
Tiles.Add("lumberhut", lumberHut);
Tiles.Add("barracks", barracks);
Tiles.Add("mine", mine);

Tiles.Add("stonewall", stoneWall);
Tiles.Add("pallissade", pallissade);

Tiles.Add("stoneroad", stoneRoad);

Tiles.Add("wheat", wheat);
Tiles.Add("forest", forest);
Tiles.Add("goldore", goldOre);
Tiles.Add("fruits", fruits);

Tiles.Add("river", river);

Tiles.Add("snow", snow);
Tiles.Add("plains", plains);
Tiles.Add("rock", rock);
Tiles.Add("dirt", dirt);
Tiles.Add("sand", sand);*/

