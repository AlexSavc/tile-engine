using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Inventory))]
public class Attack : MonoBehaviour, IAttack
{
    //[SerializeField]
    //private GameObject defaultWeapon;
    //public GameObject equipped;

    [SerializeField]
    private Inventory myInventory;

    [SerializeField]
    private bool canAttack;

    void Awake()
    {
        if (GetComponent<Inventory>())
        {
            myInventory = GetComponent<Inventory>();
        }
        else myInventory = gameObject.AddComponent<Inventory>();
        /*if (equipped != null)
        {
            Equip(equipped);
        }*/
        //else Equip(defaultWeapon);
    }

    /*public void Equip(GameObject Weapon)
    {
        if (Weapon != null && Weapon.GetComponent<Weapon>())
        {
            GameObject tempEquipped = Instantiate(Weapon, transform);
            equipped = tempEquipped;
        }
        //else equipped = defaultWeapon;
    }

    /*public void UnEquip()
    {
        equipped = null;
    }*/

    public void OnAttack(Tile toAttack)
    {
        Debug.Log("attacking");

        if (toAttack == null || !canAttack) return;

        if (GetEquipped() != null)
        {
            if (GetEquipped().GetComponent<Weapon>() != null)
            {
                GetEquipped().GetComponent<Weapon>().OnWeaponUse(toAttack);
                try
                {
                    IThrowable throwable = GetEquipped().GetComponent<IThrowable>();
                    if (throwable != null) myInventory.RemoveEquipped();
                }
                catch (NullReferenceException) { }
            }
        }
    }

    //USE DELEGATES HERE

    public int AttackDamage()
    {
        if (!canAttack) return 0;
        if (GetEquipped() != null && GetEquipped().GetComponent<Weapon>() != null)
        {
            return GetEquipped().GetComponent<Weapon>().Damage();
        }

        /*else if (defaultWeapon != null)
        {
            return defaultWeapon.GetComponent<Weapon>().Damage();
        }
        */
        else return 0;
    }

    public int GetRange()
    {
        if (GetEquipped() != null)
        {
            if(GetEquipped().GetComponent<Weapon>() != null)
            {
                return GetEquipped().GetComponent<Weapon>().Range();
            }
            
        }
        /*else if (defaultWeapon != null)
        {
            return defaultWeapon.GetComponent<Weapon>().Range();
        }
        */
        return 0;
    }

    public int GetMinRange()
    {
        if (GetEquipped() != null && GetEquipped().GetComponent<Weapon>() != null)
        {
            return GetEquipped().GetComponent<Weapon>().MinRange();
        }
        /*else if (defaultWeapon != null)
        {
            return defaultWeapon.GetComponent<Weapon>().MinRange();
        }
        */
        else return 0;
    }

    public bool GetLockPerpendicular()
    {
        if (GetEquipped() != null && GetEquipped().GetComponent<Weapon>() != null)
        {
            return GetEquipped().GetComponent<Weapon>().LockPerpendicular();
        }

        /*else if (defaultWeapon != null)
        {
            return defaultWeapon.GetComponent<Weapon>().LockPerpendicular();
        }
        */
        else return false;
    }

    public bool GetLockDiagonal()
    {
        if (GetEquipped() != null && GetEquipped().GetComponent<Weapon>() != null)
        {
            return GetEquipped().GetComponent<Weapon>().LockDiagonal();
        }

        /*else if (defaultWeapon != null)
        {
            return defaultWeapon.GetComponent<Weapon>().LockDiagonal();
        }
        */
        else return false;
    }

    public bool InRange(Vector3 toAttack)
    {
        if (Vector3.Distance(transform.position, toAttack) < GetRange() * 1.5)
        {
            return true;
        }
        else return false;
    }

    public void SetAttack(bool CanAttack)
    {
        canAttack = CanAttack;
    }

    public GameObject GetEquipped()
    {
        return myInventory.GetEquipped();
    }
}
