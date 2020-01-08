using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Tile
{
    [Header("Item Equip Transform")]
    //public Vector3 equippedScale;
    public Quaternion equippedRotation;
    public Vector3 equippedposition;

    public bool setEquippedTransform;
    
    void OnValidate()
    {
        if (setEquippedTransform)
        {
            //equippedScale = transform.localScale;
            equippedRotation = transform.localRotation;
            equippedposition = transform.localPosition;
            setEquippedTransform = false;
        }
    }
}
