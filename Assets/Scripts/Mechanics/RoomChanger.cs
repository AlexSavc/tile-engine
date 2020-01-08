using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChanger : MonoBehaviour, IInteractable
{
    public void OnInteraction(GameObject toInteract)
    {
        if(toInteract.GetComponent<Character>() && InRange(toInteract.transform.position))
        {
            FindObjectOfType<Roguelike>().NextRoom();
        }
    }

    public bool InRange(Vector3 toAttack)
    {
        if (Vector3.Distance(transform.position, toAttack) <  1.5)
        {
            return true;
        }
        else return false;
    }
}
