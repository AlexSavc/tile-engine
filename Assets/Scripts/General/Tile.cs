using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Tile Settings")]
    public string type;
    public Sprite tileSprite;
    public List<Tile> occupied;
    //public Tile occupied;

    public int XCoord;
    public int YCoord;

    public delegate void OccupiedChange(Tile tileObj);
    public event OccupiedChange occupyEvent;

    public void AddOccupy(Tile toOccupy)
    {
        occupied.Add(toOccupy);
        occupyEvent?.Invoke(toOccupy);
    }

    public bool IsOccupied()
    {
        if (occupied.Count == 0)
        {
            return false;
        }
        else return true;
    }

    public void UnOccupy(Tile toRemove)
    {
        if (occupied.Contains(toRemove))
        {
            occupied.Remove(toRemove);
        }
        
    }

    public void ClearOccupied()
    {
        occupied.Clear();
    }

    public void DestroyOccupied()
    {
        if (occupied != null && occupied.Count > 0)
        {
            foreach (Tile tile in occupied)
            {
                Destroy(tile.gameObject);
            }
            occupied.Clear();
        }
            
    }

    public void SetOccupied(List<Tile> toOccupy)
    {
        occupied = toOccupy;
    }

    public List<Tile> Occupied()
    {
        return occupied;
    }

    void OnDestroy()
    {
        ClearOccupied();
    }
}
