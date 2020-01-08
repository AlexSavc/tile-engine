using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Road, ILimitPlacement
{
    public List<Tile> required;
    
    public List<Tile> placeableOn()
    {
        return required;
    }

    public bool CanBePlacedOn(Tile tile)
    {
        if(required.Count == 0)
        {
            Debug.Log("The Bridge can't be placed on anything, required is null");
        }
        // check if tile is one of the possible types
        foreach(Tile p in required)
        {
            if(tile.GetType() == p.GetType())
            {
                return true;
            }
        }

        // check if tile's occupied has the requirements
        if (tile.IsOccupied())
        {
            foreach(Tile occ in tile.occupied)
            {
                foreach (Tile p in required)
                {
                    Tile t = Map.GetOccupiedMatch(p, occ);
                    if (t != null)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}