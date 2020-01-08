using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class EditorMenu : MonoBehaviour
{
    public MapSlot[] mapSlots;
    
    public string savePath;
    public string[] files;

    void OnEnable()
    {
        if(Directory.Exists(Application.streamingAssetsPath + "/Saves/Editor"))
        {
            savePath = Application.streamingAssetsPath + "/Saves/Editor";
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Saves/Editor");
        }

        Refresh();
    }

    public void Refresh()
    {
        DirectoryInfo directory = new DirectoryInfo(savePath);
        files = Directory.GetFiles(savePath, "*.json");
        FileInfo[] fileInfo = directory.GetFiles("*.json");
        mapSlots = transform.GetComponentsInChildren<MapSlot>();

        foreach(MapSlot slot in mapSlots)
        {
            slot.SetSave(false, null, "...");
        }

        for (int i = 0; i < files.Length; i++)
        {
            mapSlots[i].SetSave(true, files[i], Path.GetFileNameWithoutExtension(files[i]));
        }
    }
}
