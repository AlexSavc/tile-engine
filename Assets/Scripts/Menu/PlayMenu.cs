using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayMenu : MonoBehaviour
{
    public MapSlot[] mapSlots;

    public string savePath;
    public string[] files;

    void OnEnable()
    {
        if (Directory.Exists(Application.streamingAssetsPath + "/Saves/Play"))
        {
            savePath = Application.streamingAssetsPath + "/Saves/Play";
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Saves/Play");
        }

        Refresh();
    }

    public void Refresh()
    {
        DirectoryInfo directory = new DirectoryInfo(savePath);
        files = Directory.GetFiles(savePath, "*.json");
        FileInfo[] fileInfo = directory.GetFiles("*.json");
        mapSlots = transform.GetComponentsInChildren<MapSlot>();

        foreach (MapSlot slot in mapSlots)
        {
            slot.SetSave(false, null, "...");
        }

        for (int i = 0; i < files.Length; i++)
        {
            mapSlots[i].SetSave(true, files[i], Path.GetFileNameWithoutExtension(files[i]));
        }
    }
}
