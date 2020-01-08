using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Floor
{
    [SerializeField]
    private bool shallow;

    public bool IsShallow()
    {
        return shallow;
    }

    void Start()
    {
        DestroyThoseInTheWayTatArentMeantToBe();
    }

    public void DestroyThoseInTheWayTatArentMeantToBe()
    {
        if (IsOccupied())
        {
            foreach (Tile occupant in occupied)
            {
                if (occupant.IsOccupied())
                {
                    foreach (Tile occ in occupant.occupied)
                    {
                        if (occ == this || occ == null) continue;
                        try
                        {
                            ILimitPlacement limit = occ.GetComponent<ILimitPlacement>();
                            if (limit.CanBePlacedOn(this))
                            {
                                return;
                            }
                            else
                            {
                                Destroy(occ.gameObject);
                            }
                        }
                        catch (System.NullReferenceException) { Destroy(occ.gameObject);}
                    }
                }
            }
        }
    }
}
