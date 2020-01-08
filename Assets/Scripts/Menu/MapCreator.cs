using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MapCreator : MonoBehaviour
{
    public InputField sizeX;
    [SerializeField]
    private int sizex;

    public InputField sizeY;
    [SerializeField]
    private int sizey;
    
    public InputField mapName;
    [SerializeField]
    private string mapname;

    [SerializeField]
    private MapSlot temp;
    [SerializeField]
    private EditorMenu editorMenu; // SET THIS IN EDITOR, IT IS DISABLED IN RUNTIME
    [SerializeField]
    private PlayMenu playMenu; // SET THIS IN EDITOR, IT IS DISABLED IN RUNTIME
    public SceneChanger sceneManager;

    private string EditorPath;
    private string PlayPath;
    private string savePath;

    void Awake()
    {
        if (sceneManager == null) sceneManager = FindObjectOfType<SceneChanger>();
    }

    void OnEnable()
    {
        EditorPath = editorMenu.savePath;
    }

    public void OnOpen(MapSlot toCreate, bool editor)
    {
        temp = toCreate;
        savePath = EditorPath;
    }

    public void OnCreate()
    {
        

        int.TryParse(sizeX.text, out sizex);
        int.TryParse(sizeY.text, out sizey);
        mapname = mapName.text;

        if(mapname == null)
        {
            Debug.Log("You must enter a valid map name");
        }

        MapCreateInfo info = new MapCreateInfo
        {
            savePath = /*temp.*/savePath,
            mapName = mapname,
            sizeX = sizex,
            sizeY = sizey
        };

        //string newPath = /*info.*/savePath + "/" + mapname + ".json";
        info.savePath = /*info.*/savePath + "/" + mapname + ".json";
        string save = JsonUtility.ToJson(info);
        File.WriteAllText(info.savePath, save);

        temp.mapName = mapname;

        sceneManager.CreateMap(info.savePath);
        //gameObject.SetActive(false);
    }
}

public class MapCreateInfo
{
    public string savePath;
    public string mapName;
    public int sizeX;
    public int sizeY;
}
