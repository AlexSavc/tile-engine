using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item, IWeapon
{
    [Header("Weapon Stats")]
    [SerializeField]
    private int range;
    [SerializeField]
    private int minRange;
    [SerializeField]
    private int damage;
    [SerializeField]
    private bool lockPerpendicular = false;
    [SerializeField]
    private bool lockDiagonal = false;

    protected bool friendlyFire;
    

    public int Range()
    {
        return range;
    }

    public int MinRange()
    {
        return minRange;
    }

    public int Damage()
    {
        return damage;
    }

    public bool LockPerpendicular()
    {
        return lockPerpendicular;
    }

    public bool LockDiagonal()
    {
        return lockDiagonal;
    }

    public virtual void OnWeaponUse(Tile toAttack)
    {
        try
        {
            Character character = GetComponentInParent<Character>();
            if (character != null)
            {
                if (!toAttack.GetComponent<IHealth>().FriendlyFire(character.GetParty()))
                {
                    toAttack.GetComponent<IHealth>().TakeDamage(Damage());
                    character.UseAction();
                    friendlyFire = false;
                }
                else
                {
                    friendlyFire = true;
                }
            }
        }
        catch (System.NullReferenceException) { }
    }
}
