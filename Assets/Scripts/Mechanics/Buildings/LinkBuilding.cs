using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Tile))]
public class LinkBuilding : MonoBehaviour
{
    public Map map;
    public Movement move;
    public Tile myTile;

    //for the bridge
    public bool twoMax = false;

    [Header("If adjacent tiles are linkable")]
    // open to addibng diagonal adjacents
    public bool topAdjacent;
    public bool bottomAdjacent;
    public bool rightAdjacent;
    public bool leftAdjacent;

    [Header("Different states")]

    [Space]
    //no adjecents
    public Sprite zero;
    [Space]
    //one adjacent, on top
    public Sprite oneTop;
    public Sprite oneBottom;
    public Sprite oneRight;
    public Sprite oneLeft;
    [Space]
    //two adjacents, one right and one left
    public Sprite twoHorizontal;
    public Sprite twoVertical;
    [Space]
    //two adjacents, one top one right
    public Sprite twoTopRight;
    public Sprite twoTopLeft;
    public Sprite twoBottomRight;
    public Sprite twoBottomLeft;
    [Space]
    //three adjacents, none on top
    public Sprite threeTop;
    public Sprite threeBottom;
    public Sprite threeRight;
    public Sprite threeLeft;
    [Space]
    //Top, bottom, fet, right are adjacent;
    public Sprite fourSides;

    void Start()
    {
        map = FindObjectOfType<Map>();
        move = GetComponent<Movement>();
        myTile = GetComponent<Tile>();

        CheckAdjacents(false);
    }

    public void CheckAdjacents(bool dontCheckNeighbours)
    {
        ClearBools();

        if (map == null) return;

        List<Tile> range = GetAdjacentTiles();//move.GetTileRange(0, 1,transform.position);


        foreach(Tile tile in range)
        {
            
            Tile t = Map.GetOccupiedMatch(myTile, tile);
            // GetOccupiedMatch checks for assignability, still need to check if they are linkable 
            //(road/wall are not, for example, but would pass the CheckOccupiedMatch)

            if (t != null && myTile.GetType() == t.GetType())
            {
                if(t.GetComponent<LinkBuilding>())
                {
                    SetBools(myTile, t);
                    if (!dontCheckNeighbours) { t.GetComponent<LinkBuilding>().CheckAdjacents(true); }
                }
            }
        }

        SetDisplay();
    }

    public List<Tile> GetAdjacentTiles()
    {
        List<Tile> list = move.GetTileRange(0, 1);
        /*List<Tile> list = new List<Tile>();
        int x = myTile.XCoord;
        int y = myTile.YCoord;
        GameObject[,] m = map.map;
        try { if (m[x - 1, y] != null) list.Add(m[x - 1, y].GetComponent<Tile>()); } catch (System.Exception) { }
        try { if (m[x + 1, y] != null) list.Add(m[x + 1, y].GetComponent<Tile>()); } catch (System.Exception) { }
        try { if (m[x, y - 1] != null) list.Add(m[x, y - 1].GetComponent<Tile>()); } catch (System.Exception) { }
        try { if (m[x, y + 1] != null) list.Add(m[x, y + 1].GetComponent<Tile>()); } catch (System.Exception) { }*/
        return list;
    }

    public void SetBools(Tile me, Tile toGet)
    {
        int X = me.XCoord;
        int Y = me.YCoord;

        int Xx = toGet.XCoord;
        int Yy = toGet.YCoord;

        if (X > Xx) // i'm to the right
        {
            if (Y == Yy) leftAdjacent = true; // no vertical motion

            // set these up when diagonals are added
            //else if (Y < Yy) ; // i'm below
            //else ; // i'm above
        }

        if (X < Xx) // i'm to the left
        {
            if (Y == Yy) rightAdjacent = true;

            // set these up when diagonals are added
            //else if (Y < Yy) view = 5;
            //else view = 7;
        }

        if (X == Xx)
        {
            if (Y > Yy) bottomAdjacent = true;
            else if (Y < Yy) topAdjacent = true;
        }
    }

    public void SetDisplay()
    {
        Sprite sp = null;

        int i = 0;
        if (topAdjacent) i++;
        if (bottomAdjacent) i++;
        if (leftAdjacent) i++;
        if (rightAdjacent) i++;

        if(i == 0)
        {
            sp = zero;
        }

        else if(twoMax)
        {
            if(i == 2)
            {
                if (topAdjacent) sp = twoVertical;
                if (rightAdjacent) sp = twoHorizontal;
            }
            if(i == 3)
            {
                if (topAdjacent && bottomAdjacent) sp = twoVertical;
                else sp = twoHorizontal;
            }
            if (i == 4) sp = zero;
        }

        else if(i == 1)
        {
            if (topAdjacent) sp = oneTop;
            if (bottomAdjacent) sp = oneBottom;
            if (leftAdjacent) sp = oneLeft;
            if (rightAdjacent) sp = oneRight;
        }

        else if(i == 2)
        {
            if (topAdjacent)
            {
                if (rightAdjacent) sp = twoTopRight;
                else if (leftAdjacent) sp = twoTopLeft;
                else sp = twoVertical;
            }
            else if (bottomAdjacent)
            {
                if (rightAdjacent) sp = twoBottomRight;
                else if (leftAdjacent) sp = twoBottomLeft;
                else sp = twoVertical;
            }
            else sp = twoHorizontal;
        }

        else if( i == 3)
        {
            if (!topAdjacent) sp = threeTop;
            else if (!bottomAdjacent) sp = threeBottom;
            else if (!leftAdjacent) sp = threeLeft;
            else if (!rightAdjacent) sp = threeRight;
        }

        else if(i == 4)
        {
            sp = fourSides;
        }

        if(sp != null) SetTileSprite(sp);
    }

    public void SetTileSprite(Sprite sprite)
    {
        SpriteRenderer rend = myTile.gameObject.GetComponent<SpriteRenderer>();
        rend.sprite = sprite;
    }

    void ClearBools()
    {
        topAdjacent = false;
        bottomAdjacent = false;
        leftAdjacent = false;
        rightAdjacent = false;
    }

    void OnDestroy()
    {
        CheckAdjacents(false);
    }
}
