using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    [Header("Markers")]
    [SerializeField]
    private GameObject selectionMarker;
    [SerializeField]
    private GameObject moveMarker;
    [SerializeField]
    private GameObject attackMarker;

    [Header("Movement")]
    [SerializeField]
    private float speed;
    [SerializeField]
    public bool isMoveable;
    public bool diagonalMovement;
    public bool perpendicularMovement;

    [SerializeField]
    private Tile myTile;
    private Map map;
    private MapManager manager;

    public int range;
    [Header("Selection")]
    public bool showingSelection;
    
    public Tile[] selected;
    [SerializeField]
    private GameObject selectionParent;

    public delegate void MoveDelegate();
    public event MoveDelegate moveEvent;

    public bool discriminateAttack = true;

    void Awake()
    {

        manager = FindObjectOfType<MapManager>();
        map = FindObjectOfType<Map>();
        myTile = GetComponent<Tile>();

        if(selectionParent == null)
        {
            selectionParent = new GameObject("SelectionParent");
            selectionParent.transform.parent = transform;
            selectionParent.transform.localPosition = Vector3.zero;
        }
        try
        {
            moveMarker = manager.MoveMarker;
            attackMarker = manager.attackMarker;
            selectionMarker = manager.selectionMarker;

            FindObjectOfType<Interaction>().selectionEvent += OnSelection;

            Health health = gameObject.GetComponent<Health>();
            if (health != null) health.deathEvent += OnPlayerDeath;
        }
        catch(NullReferenceException)
        {

        }

        CheckComponents();
        CheckDiscriminate();
    }

    public void OnSelection(GameObject selected)
    {
        if (isMoveable && selected == gameObject)
        {
            CheckDiscriminate();
            ShowSelection(Moveable(), Attackable(discriminateAttack));
        }
        else if (showingSelection)
        {
            ClearSelection();
        }
    }

    public void Move(Tile toMove)
    {
        if (!isMoveable) return;
        ///////////////////////////////////////////////////////////////////////////////myTile.occupied.UnOccupy(); // is no longer occupied

        if (!CanMove(toMove)) return;

        transform.position = new Vector3(transform.position.x,
                                             transform.position.y,
                                             /*toMove.transform.position.z - GetComponent<Tile>().zOffset - 1*/0);
        
        Vector3 pos = new Vector3(toMove.transform.position.x,
                                         toMove.transform.position.y,
                                         /*toMove.transform.position.z - GetComponent<Tile>().zOffset*/0);
        StartCoroutine(translate(pos));

        toMove.AddOccupy(myTile);
        myTile.AddOccupy(toMove);

        moveEvent?.Invoke();
    }

    public bool CanMove(Tile toCheck)
    {
        if (Vector3.Distance(transform.position, toCheck.transform.position) < range + 0.5 && !toCheck.IsOccupied())
        {
            return true; // if not too far
        }
        else return false;
    }

    public List<Tile> GetTileRange(int minRange, int Range)
    {
        int[] coords = map.GetCoordFromVector(transform.position);
        int Xcoord = coords[0];
        int Ycoord = coords[1];
        List<Tile> tiles = new List<Tile>();

        for (int x = Xcoord - Range; x <= Xcoord + Range; x++)
        {
            for (int y = Ycoord - Range; y <= Ycoord + Range; y++)
            {
                try
                {
                    try
                    {
                        Tile toMove = map.map[x, y].GetComponent<Tile>();

                        if (!IsInside(x, y, Xcoord - minRange, Xcoord + minRange, Ycoord - minRange, Ycoord + minRange, false))
                        {
                            if(!perpendicularMovement && !diagonalMovement)
                            {
                                if (toMove != null) tiles.Add(toMove);
                            }

                            if(perpendicularMovement)
                            {
                                if(x == Xcoord || y == Ycoord)
                                {
                                    if (toMove != null) tiles.Add(toMove);
                                }
                            }

                            if(diagonalMovement)
                            {
                                int Xx = x - Xcoord;
                                int Yy = y - Ycoord;

                                if(Mathf.Sqrt(Xx * Xx) == Mathf.Sqrt(Yy * Yy))
                                {
                                    if (toMove != null) tiles.Add(toMove);
                                }
                            }
                            
                        }
                    }
                    catch (NullReferenceException) { }
                    
                }
                catch(IndexOutOfRangeException ) { }
            }
        }
        return tiles;
    }

    public List<Tile> GetTileRange(int minRange, int Range, bool lockDiagonal, bool lockPerpendicular)
    {
        int[] coords = map.GetCoordFromVector(transform.position);
        int Xcoord = coords[0];
        int Ycoord = coords[1];
        List<Tile> tiles = new List<Tile>();

        for (int x = Xcoord - Range; x <= Xcoord + Range; x++)
        {
            for (int y = Ycoord - Range; y <= Ycoord + Range; y++)
            {
                try
                {
                    try
                    {
                        Tile toMove = map.map[x, y].GetComponent<Tile>();

                        if (!IsInside(x, y, Xcoord - minRange, Xcoord + minRange, Ycoord - minRange, Ycoord + minRange, false))
                        {
                            if (!lockPerpendicular && !lockDiagonal)
                            {
                                if (toMove != null) tiles.Add(toMove);
                            }

                            else
                            {
                                if (lockPerpendicular && IsPerpendicular(x, Xcoord, y, Ycoord))
                                {
                                    if (toMove != null) tiles.Add(toMove);
                                }

                                if (lockDiagonal && IsDiagonal(x, Xcoord, y, Ycoord))
                                {
                                    if (toMove != null) tiles.Add(toMove);
                                }
                            }
                        }
                    }
                    catch (NullReferenceException) { }

                }
                catch (IndexOutOfRangeException) { }
            }
        }
        return tiles;
    }

    public Tile[] Moveable()
    {
        List<Tile> moveable = new List<Tile>();
        List<Tile> toCheck = GetTileRange(0 , range);

        foreach (Tile tile in toCheck)
        {
            if (tile.GetComponent<IWalkable>() != null)
            {
                if (tile.IsOccupied() == false)
                {
                    moveable.Add(tile);
                }
            }
        }

        return moveable.ToArray();
    }

    public Tile[] Attackable(bool discriminate)
    {
        List<Tile> attackable = new List<Tile>();
        
        IAttack attack;
        try
        {
            attack = GetComponent<IAttack>();
        }
        catch (NullReferenceException) { return attackable.ToArray(); }

        int attackRange = attack.GetRange();
        int minAttackRange = attack.GetMinRange();
        List<Tile> toCheck = GetTileRange(minAttackRange, attackRange, attack.GetLockDiagonal(), attack.GetLockPerpendicular());
        
        foreach(Tile toAttack in toCheck)
        {
            if (discriminate == false && toAttack.occupied == null)
            {
                attackable.Add(toAttack);
            }
            /*else if (discriminate == true && toAttack.IsOccupied() == true && toAttack.occupied != myTile)
            {
                
                ////////if (toAttack.occupied.GetComponent<IHealth>() != null)                 THIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIs
                {
                    attackable.Add(toAttack);
                }
            }*/
        }

        return attackable.ToArray();

    }


    public void ShowSelection(Tile[] toSelect, Tile[] toAttack)
    {
        selected = toSelect;
        Tile occupied = null; ////////////// myTile.occupied;
        Vector3 pos = new Vector3(occupied.transform.position.x, occupied.transform.position.y, occupied.transform.position.z - manager.markerOffset);
        GameObject selectionObj = Instantiate(selectionMarker, pos, Quaternion.identity, selectionParent.transform);

        foreach (Tile tile in selected)
        {
            pos = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - manager.markerOffset);
            GameObject moveObj = Instantiate(moveMarker, pos, Quaternion.identity, selectionParent.transform);
        }
        foreach (Tile tile in toAttack)
        {
            pos = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - manager.markerOffset);
            GameObject attackObj = Instantiate(attackMarker, pos, Quaternion.identity, selectionParent.transform);
        }
        showingSelection = true;
    }

    public void ClearSelection()
    {
        Transform parent = selectionParent.transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        showingSelection = false;
    }

    IEnumerator translate(Vector3 tragetPosition)
    {
        while (Vector3.Distance(transform.localPosition, tragetPosition) > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, tragetPosition, Time.deltaTime * speed);
            yield return null;
        }

        yield return null;
        transform.localPosition = tragetPosition;
        //moveEvent?.Invoke();
    }

    void OnPlayerDeath()
    {
        FindObjectOfType<Interaction>().selectionEvent -= OnSelection;
        ClearSelection();
    }

    public void SetMoveable(bool moveable)
    {
        isMoveable = moveable;
    }

    public static bool IsBetween(float i, float min, float max)
    {
        if (i > min && i < max) return true;
        else return false;
    }

    public static bool IsBetween(float i, float min, float max, bool includeMinMax)
    {
        if (i >= min && i <= max) return true;
        else return false;
    }

    public bool IsInside(int x, int y, int minX, int maxX, int minY, int maxY, bool IncudeMinMax)
    {
        if(IncudeMinMax)
        {
            if (x >= minX && x <= maxX && y >= minY && y <= maxY)
            {
                return true;
            }
            else return false;
        }

        else
        {
            if (x > minX && x < maxX && y > minY && y < maxY)
            {
                return true;
            }
            else return false;
        }
    }

    public void CheckDiscriminate()
    {
        GameObject obj = GetWeapon();

        if (obj != null && obj.GetComponent<Grenade>())
        {
            discriminateAttack = false;
        }
        
        else discriminateAttack = true;
    }

    public GameObject GetWeapon()
    {
        GameObject obj = null;

        try
        {
            Attack a = GetComponent<Attack>();
            GameObject ob = a.GetEquipped();
            if (ob != null)
            {
                obj = GetComponent<Attack>().GetEquipped();
            }
        }
        catch (NullReferenceException) {/* Debug.Log("Nullreference in getWeapon on " + gameObject); */}

        return obj;
    }

    public Tile CurrentTile()
    {
        return null; /////////////////////////////////////////////// myTile.occupied;
    }

    public bool IsDiagonal(int X, int x, int Y, int y)
    {
        int Xx = x - X;
        int Yy = y - Y;

        if (Mathf.Sqrt(Xx * Xx) == Mathf.Sqrt(Yy * Yy))
        {
            return true;
        }

        else return false;
    }

    public bool IsPerpendicular(int X, int x, int Y, int y)
    {
        if (x == X || y == Y) return true;
        else return false;
    }

    void CheckComponents()
    {
        Component[] comps = GetComponents<Component>();
        if(GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    void OnDestroy()
    {
        if(FindObjectOfType<Interaction>() != null)
        FindObjectOfType<Interaction>().selectionEvent -= OnSelection;
    }
}
