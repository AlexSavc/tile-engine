using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Furniture : Tile , IInteractable, IMoveable
{
    public Vector3 itemPosition;
    public GameObject item;

    public void OnInteraction(GameObject toInteract)
    {
        try
        {

        }
        catch (NullReferenceException) { }
    }

    public void OnMovement(Vector3 MoveTo)
    {

    }
}
