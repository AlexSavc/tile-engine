using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tile))]
public class MultiPerspective : MonoBehaviour
{
    [Header("GameObject with SpriteRenderer")]
    [SerializeField]
    private GameObject display;
    SpriteRenderer rend;

    [Header("GameObject with SpriteRenderer")]
    [SerializeField]
    private GameObject moveObj;
    [SerializeField]
    private Movement move;

    [Header("All Perspectives")]
    public Sprite north;
    public Sprite northEast;
    public Sprite east;
    public Sprite southEast;
    public Sprite south;
    public Sprite southWest;
    public Sprite west;
    public Sprite northWest;

    [SerializeField]
    private Sprite[] views;
    
    [SerializeField]
    private int currentView;
    [SerializeField]
    private Tile myTile;

    Tile previousTile;
    Tile currentTile;


    void OnDisable()
    {
        enabled = true;
    }

    void Awake()
    {
        views = new Sprite[8] { north, northEast, east, southEast, south, southWest, west, northWest };
    }

    void Start()
    {
        GetFirstTile();
        
        ///////////////currentTile = myTile.occupied;
        previousTile = currentTile;

        GetMoveObj();
        GetFirstMovement();
        
        rend = display.GetComponent<SpriteRenderer>();
    }

    void CheckDisplay()
    {
        if (rend == null)
        {
            return;
        }
        SetTiles();
        SetDisplay(CheckDirection());
    }

    void SetDisplay(int view)
    {
        if (view >= views.Length) view = view % views.Length;

       

        if (views[view] != null)
        {
            rend.sprite = views[view];
        }

        currentView = view;
    }

    int CheckDirection()
    {
        int X = currentTile.XCoord;
        int Y = currentTile.YCoord;

        int Xx = previousTile.XCoord;
        int Yy = previousTile.YCoord;

        int view = currentView;
        
        if(X > Xx)
        {
            if (Y == Yy) view = 2;
            else if (Y < Yy) view = 3;
            else view = 1;
        }

        if (X < Xx)
        {
            if (Y == Yy) view = 6;
            else if (Y < Yy) view = 5;
            else view = 7;
        }

        if (X == Xx)
        {
            if (Y > Yy) view = 0;
            else if (Y < Yy) view = 4;
        }
        return view;
    }

    void SetTiles()
    {
        previousTile = currentTile;
        ////////////////////currentTile = myTile.occupied;
    }

    void GetFirstTile()
    {
        if (GetComponent<Tile>())
        {
            myTile = GetComponent<Tile>();
        }

        else if(transform.parent.gameObject.GetComponent<Tile>())
        {
            myTile = transform.parent.gameObject.GetComponent<Tile>();
        }
    }

    void GetMoveObj()
    {
        if(GetComponent<Movement>())
        {
            moveObj = gameObject;
            return;
        }
        /*
        else if(transform.parent.gameObject.GetComponent<Movement>())
        {
            moveObj = transform.parent.gameObject;
        }*/
    }

    void GetFirstMovement()
    {
        if (moveObj == null) return;
        if (moveObj.GetComponent<Movement>())
        {
            move = moveObj.GetComponent<Movement>();
        }

        if (move != null) move.moveEvent += CheckDisplay;
        else Debug.Log(gameObject.name + " move null");
    }

    void OnDestroy()
    {
        if(move != null)
        move.moveEvent -= CheckDisplay;
    }
}
