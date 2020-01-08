using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Tile))]
public abstract class Ability : MonoBehaviour
{
    [SerializeField]
    protected Tile lastTile;
    [SerializeField]
    protected Tile currentTile;
    [SerializeField]
    protected Movement move;
    [SerializeField]
    protected Map map;
    [SerializeField]
    protected Tile myTile;

    public Tile[] oldAdjacents;
    public Tile[] adjacents;

    protected Attack attack;

    void Awake()
    {
        attack = GetComponent<Attack>();
    }

    void Start()
    {
        Tile[] mes = GetComponentsInParent<Tile>();
        Tile me = mes[mes.Length - 1];

        move = me.GetComponent<Movement>();
        map = FindObjectOfType<Map>();
        myTile = me;
        move.moveEvent += AnalyseMove;

        SetTiles();

        adjacents = move.GetTileRange(1, 1).ToArray();
    }

    void SetTiles()
    {
        if(currentTile != null)
        {
            lastTile = currentTile;
            currentTile = GetCurrentTile();
        }
        else
        {
            currentTile = GetCurrentTile();
            lastTile = currentTile;
        }
    }

    public virtual void AnalyseMove()
    {
        SetTiles();
        SetAdjacents();
    }

    public Tile LastTile()
    {
        return lastTile;
    }

    public Tile GetCurrentTile()
    {
        return null;
        /////////////////////////////////////////////////////////////return myTile.occupied;
    }

    public void SetAdjacents()
    {
        oldAdjacents = adjacents;
        adjacents = move.GetTileRange(1, 1).ToArray();
    }
}

