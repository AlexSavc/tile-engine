using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour
{
    public int sizeX;
    public int sizeY;
    public GameObject tileParent;
    public GameObject[,] map;
    public MapManager manager;

    string jsonSave;
    string savePath;


    public delegate void MapGenerateEvent();
    public event MapGenerateEvent OnGenerateMap;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            SaveMap();
        }
    }

    void Awake()
    {
        if (manager == null) manager = FindObjectOfType<MapManager>();
        //FindObjectOfType<SceneChanger>().map = this;
    }

    public void GenerateMap(MapCreateInfo info)
    {
        sizeX = info.sizeX;
        sizeY = info.sizeY;

        savePath = info.savePath;
        jsonSave = File.ReadAllText(savePath);

        if (tileParent != null) Destroy(tileParent);
        int X = sizeX;
        int Y = sizeY;

        tileParent = new GameObject();
        tileParent.transform.parent = transform;
        map = new GameObject[X, Y];
        for(int x = 0; x < X; x++)
        {
            for(int y = 0; y< Y; y++)
            {
                GameObject toSpawn = manager.SpawnAlgorithm(x, X, y, Y);
                
                Tile myTile = toSpawn.GetComponent<Tile>();
                myTile.XCoord = x;  
                myTile.YCoord = y;
                Vector3 Position = new Vector3(x*manager.offset.x, y * manager.offset.y, 0);
                toSpawn.GetComponent<SpriteRenderer>().sortingOrder = Y-y;
                GameObject tile = Instantiate(toSpawn, Position, Quaternion.identity, tileParent.transform);
                map[x, y] = tile;
            }
        }

        //SpawnTile(manager.stairsDown, (X/2), Y-2, true);
        //SpawnTile(manager.table, 2, 3, false);

        OnGenerateMap?.Invoke();
    }

    public Tile GetTileFromVector(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / manager.offset.x);
        int y = Mathf.RoundToInt(position.y / manager.offset.y);
        
        Tile tile = map[x, y].GetComponent<Tile>();
        return tile;
    }

    public int[] GetCoordFromVector(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / manager.offset.x);
        int y = Mathf.RoundToInt(position.y / manager.offset.y);
        if (x >= sizeX) x = sizeX - 1;
        if (y >= sizeY) y = sizeY - 1;
        if (x < 0) x = 0;
        if (y < 0) y = 0;
        int[] coords = new int[2] { x, y };
        return coords;
    }

    public void SpawnTile(GameObject toSpawn, int x, int y)
    {
        if (toSpawn == null)
        {
            Debug.Log("toSpawn == null");
            return;
        }
        Vector3 Pos = new Vector3(x * manager.offset.x, y * manager.offset.y, 0);

        GameObject mapObj = map[x, y];
        Tile mapTile = mapObj.GetComponent<Tile>();
        //if (mapTile.IsOccupied() && !replace) return; /////////////////////////////////////////////////

        try
        {
            ILimitPlacement l = toSpawn.GetComponent<ILimitPlacement>();
            if(!l.CanBePlacedOn(mapTile))
            {
                return;
            }
        }
        catch (System.NullReferenceException) { }

        GameObject obj = Instantiate(toSpawn, Pos, Quaternion.identity);

        obj.transform.parent = tileParent.transform;
        
        Tile newTile = obj.GetComponent<Tile>();

        newTile.XCoord = x;
        newTile.YCoord = y;

        SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();


        if(newTile.GetType() == (typeof(Floor)))
        {
            //if you're replacing a floor tile
            if (mapTile.IsOccupied())
            {
                foreach(Tile occupant in mapTile.occupied )
                {
                    //Foreach Tile that occupied mapTile
                    //Transfer occupant to new Tile
                    occupant.AddOccupy(newTile);
                    newTile.AddOccupy(occupant);
                }
            }
            //Set objTile in map[,], destroy previous tile, set rendering layer
            rend.sortingOrder = sizeY - y;
            Destroy(mapObj);
            map[x, y] = obj;
        }
        else
        {
            //In the case where you place something on a floor
            //You check if there is the same type of Tile occupying it
            Tile occupant;

            occupant = GetOccupiedMatch(newTile, mapTile);

            //And if there is
            if (occupant != null)
            {
                //*t a k e   c a r e   o f   i t*
                mapTile.UnOccupy(occupant);
                Destroy(occupant.gameObject);
            }

            occupant = GetOccupiedTypeOf(typeof(Floor), mapTile);

            if ( occupant != null)
            {
                mapTile.UnOccupy(occupant);
                Destroy(occupant.gameObject);
            }

            try
            {
                IDestroyOccupied destroy = newTile.GetComponent<IDestroyOccupied>();
                if(destroy != null)
                mapTile.DestroyOccupied();
            }
            catch (System.NullReferenceException) { }

            rend.sortingOrder = sizeY - y;
            mapTile.AddOccupy(newTile);
            newTile.AddOccupy(mapTile);
        }
    }

    /*public void SpawnTileRandom(GameObject toSpawn, out GameObject spawned)
    {
        if (map == null) { spawned = null; Debug.Log("no Map"); return; }

        int tried = 0;
        GameObject obj = null;
        retry:
        if (tried >= 100)
        {
            spawned = obj;
            return;
        }

        tried += 1;

        int x = Random.Range(0, sizeX - 1);
        int y = Random.Range(0, sizeY - 1);
        
        Vector3 Pos = new Vector3(x * manager.offset.x, y * manager.offset.y, 0);
        
        Tile mapTile = map[x, y].GetComponent<Tile>();
        if (mapTile.IsOccupied())
        {
            goto retry;
        }

        try
        {
            IWalkable walkable = mapTile.GetComponent<IWalkable>();
        }
        catch (System.NullReferenceException) { goto retry; }

        obj = Instantiate(toSpawn, Pos, Quaternion.identity);

        obj.transform.parent = tileParent.transform;

        Tile objTile = obj.GetComponent<Tile>();

        objTile.AddOccupy(mapTile);
        mapTile.AddOccupy(objTile);

        spawned = obj;
        return;
    }*/

    /*public void PlaceTileRandom(GameObject toPlace)
    {
        if (map == null) { Debug.Log("no Map"); return; }

        int tried = 0;
        GameObject obj = null;
        retry:

        if (tried >= 100)
        {
            return;
        }

        tried += 1;

        int x = Random.Range(0, sizeX - 1);
        int y = Random.Range(0, sizeY - 1);

        Vector3 Pos = new Vector3(x * manager.offset.x, y * manager.offset.y, 0);

        Tile mapTile = map[x, y].GetComponent<Tile>();
        if (mapTile.IsOccupied())
        {
            goto retry;
        }

        try
        {
            IWalkable walkable = mapTile.GetComponent<IWalkable>();
        }
        catch (System.NullReferenceException) { goto retry; }
        

        obj.transform.parent = tileParent.transform;

        Tile objTile = toPlace.GetComponent<Tile>();

        objTile.AddOccupy(mapTile);
        mapTile.AddOccupy(objTile);

        return;
    }*/


    public void SaveMap()
    {
        if(map == null)
        {
            return;
        }

        SaveData saveData = new SaveData();
        saveData.tileData = new List<TileData>();

        foreach(GameObject obj in map)
        {
            Tile tile = obj.GetComponent<Tile>();
            TileData data = new TileData
            {
                type = tile.type,
                XCoord = tile.XCoord,
                YCoord = tile.YCoord
            };

            if(tile.occupied != null && tile.occupied.Count > 0)
            {
                data.occupiedType = new List<string>();
                foreach(Tile t in tile.occupied)
                {
                    data.occupiedType.Add(t.type);
                }
            }

            saveData.tileData.Add(data);
            saveData.SizeX = sizeX;
            saveData.SizeY = sizeY;
        }
        jsonSave = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, jsonSave);
        Debug.Log("Saved");
    }

    public void LoadMap()
    {
        SaveData save = JsonUtility.FromJson<SaveData>(jsonSave);

        if (tileParent != null) Destroy(tileParent);

        sizeX = save.SizeX;
        sizeY = save.SizeY;

        int X = save.SizeX;
        int Y = save.SizeY;
        tileParent = new GameObject();
        tileParent.transform.parent = transform;
        map = new GameObject[X, Y];

        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y < Y; y++)
            {
                TileData tileData = save.tileData[x*Y+y];
                GameObject toSpawn = manager.GetTileObj(tileData.type);

                if (toSpawn == null) Debug.Log("toSpawn == null");

                Tile myTile = toSpawn.GetComponent<Tile>();
                myTile.XCoord = x;
                myTile.YCoord = y;
                Vector3 Position = new Vector3(x * manager.offset.x, y * manager.offset.y, 0);
                toSpawn.GetComponent<SpriteRenderer>().sortingOrder = Y - y;
                GameObject tile = Instantiate(toSpawn, Position, Quaternion.identity, tileParent.transform);
                map[x, y] = tile;

                if(tileData.occupiedType != null)
                {
                    if(tileData.occupiedType.Count > 0)
                    {
                        foreach(string type in tileData.occupiedType)
                        {
                            SpawnTile(manager.GetTileObj(type), x, y);
                        }
                    }
                }
            }
        }
        OnGenerateMap?.Invoke();
    }

    public void SetSaveData(string path)
    {
        savePath = path;
        jsonSave = File.ReadAllText(savePath); 
    }

    public static Tile GetOccupiedMatch(Tile input, Tile toCheck)
    {
        //to see if there is already a tile of the same type
        int a = toCheck.occupied.Count;

        if (a == 0)
        {
            //there isn't
            return null;
        }
        
        for (int i = 0; i < a; i++)
        {
            // foreach occupant
            Tile occupant = toCheck.occupied[i];
            if (input.GetType().IsSubclassOf(/*typeof(Furniture*/occupant.GetType().BaseType) || occupant.GetType().IsSubclassOf(input.GetType().BaseType))
            {
                return occupant;
            }
        }
        return null;
    }

    public static Tile GetOccupiedTypeOf(System.Type input, Tile toCheck)
    {
        //to see if there is already a tile of the same type
        int a = toCheck.occupied.Count;

        if (a == 0)
        {
            //there isn't
            return null;
        }

        for (int i = 0; i < a; i++)
        {
            // foreach occupant
            Tile occupant = toCheck.occupied[i];
            if (input.IsSubclassOf(occupant.GetType().BaseType) || occupant.GetType().IsSubclassOf(input))
            {
                return occupant;
            }
        }
        return null;
    }



    [System.Serializable]
    private class SaveData
    {
        public List<TileData> tileData;
        public int SizeX;
        public int SizeY;
    }

    [System.Serializable]
    private class TileData
    {
        public string type;
        public int XCoord;
        public int YCoord;
        public List<string> occupiedType;
    }
}


