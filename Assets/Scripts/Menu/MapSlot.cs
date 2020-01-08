using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class MapSlot : MonoBehaviour
{
    public string mapName = "...";
    public string savePath;

    public GameObject textObj;
    TextMeshProUGUI text;

    public EditorMenu editorMenu;
    public MapCreator mapCreator;
    public SceneChanger sceneManager;

    public bool hasSave = false;
    public bool editor;

    void OnEnable()
    {
        if (sceneManager == null) sceneManager = FindObjectOfType<SceneChanger>();
        if (editorMenu == null) editorMenu = FindObjectOfType<EditorMenu>();
        //ALWAYS SET MAPCREATOR IN EDITOR

        if (transform.GetChild(0).GetComponent<TextMeshProUGUI>())
        {
            textObj = transform.GetChild(0).gameObject;
        }

        RefreshText();
    }

    public void RefreshText()
    {
        if (textObj == null) return;
        text = textObj.GetComponent<TextMeshProUGUI>();
        text.SetText(mapName);
    }

    public void SetSave(bool has, string path, string name)
    {
        if(has)
        {
            hasSave = true;
            savePath = path;
            mapName = name;
        }
        else
        {
            hasSave = false;
            mapName = "...";
        }

        RefreshText();
    }

    public void Open()
    {
        if(!hasSave)
        {
            mapCreator.gameObject.SetActive(true);
            mapCreator.OnOpen(this, editor);
            transform.parent.gameObject.SetActive(false);
            //editorMenu.gameObject.SetActive(false);
        }
        else
        {
            sceneManager.LoadMap(this);
        }
    }

    public void DeleteMap()
    {
        if (hasSave)
        {
            if (File.Exists(savePath + ".meta"))
            {
                File.Delete(savePath + ".meta");
            }
            File.Delete(savePath);
            SetSave(false, null, "...");
            editorMenu.Refresh();
        }
    }
}
