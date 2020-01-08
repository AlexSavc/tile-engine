using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Floor : Tile, IInteractable, IWalkable
{
    public void OnInteraction(GameObject toInteract)
    {
        try
        {
            IMoveable moveable = toInteract.GetComponent<IMoveable>();

            if (moveable != null)
            {
                moveable.OnMovement(transform.position);
            }
            
        }
        catch(NullReferenceException e) { }
    }
}
