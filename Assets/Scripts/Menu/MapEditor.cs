using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public Interaction interaction;
    public GameObject selected;
    public Map map;

    public bool canPaint = false;
    public bool isErasing = false;

    void Awake()
    {
        if (map == null)
        {
            map = FindObjectOfType<Map>();
        }

        if (interaction == null)
        {
            interaction = FindObjectOfType<Interaction>();
        }

        interaction.selectionEvent += OnClick;
        interaction.dragEvent += OnClick;
    }

    public void OnSelection(GameObject tile)
    {
        selected = tile;
    }

    public void OnClick(GameObject obj)
    {
        // this prevents you from painting while you click on nothing or are moving the canvass (space)
        if (obj == null || Input.GetKey(KeyCode.Space)) return; 
        
        if (obj.GetComponent<Tile>())
        {
            Tile clicked = map.GetTileFromVector(obj.transform.position);

            if (isErasing)
            {
                map.map[clicked.XCoord, clicked.YCoord].GetComponent<Tile>().DestroyOccupied();
            }

            if (!canPaint ) return;

            if(selected != null)
            {
                if (selected.GetComponent<Floor>())
                {
                    map.SpawnTile(selected, clicked.XCoord, clicked.YCoord);
                }
                if (selected.GetComponent<Furniture>())
                {
                    map.SpawnTile(selected, clicked.XCoord, clicked.YCoord);
                }
            }
            
        }
    }

    public void SetCanPaint(bool can)
    {
        canPaint = can;
    }

    public void SetErase(bool erase)
    {
        isErasing = erase;
    }
}
