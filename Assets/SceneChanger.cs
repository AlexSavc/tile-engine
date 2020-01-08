using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    static SceneChanger Instance;
    public Map map;

    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void LoadMap(MapSlot slot)
    {
        Application.LoadLevel("Editor");

        StartCoroutine(LoadEnumerator(slot.savePath));
    }

    public void CreateMap(string path)
    {
        MapCreateInfo info = JsonUtility.FromJson<MapCreateInfo>(System.IO.File.ReadAllText(path));

        Application.LoadLevel("Editor");
        
        StartCoroutine(enumerator(info));
    }

    IEnumerator enumerator(MapCreateInfo info)
    {
        yield return new WaitForSeconds(0.1f);
        map = FindObjectOfType<Map>();
        if (map != null)
            map.GenerateMap(info);
        else Debug.Log("Map null");

    }

    IEnumerator LoadEnumerator(string path)
    {
        yield return new WaitForSeconds(0.1f);
        map = FindObjectOfType<Map>();
        if (map != null)
        {
            map.SetSaveData(path);
            map.LoadMap();
        }
        else Debug.Log("Map null");

    }

    void ReturnMenu()
    {

    }
}
