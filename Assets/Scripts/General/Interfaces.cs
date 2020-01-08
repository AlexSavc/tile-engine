using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWalkable
{

}

public interface IMoveable
{
    void OnMovement(Vector3 MoveTo);
}

public interface IInteractable
{
    void OnInteraction(GameObject toInteract);
}

public interface IHealth
{
    void TakeDamage(int damage);
    bool FriendlyFire(Party attacker);
    int CurrentHealth();
}

public interface IAttack
{
    int AttackDamage();
    int GetRange();
    int GetMinRange();
    bool InRange(Vector3 toAttack);
    void OnAttack(Tile toAttack);
    bool GetLockPerpendicular();
    bool GetLockDiagonal();
}

public interface IWeapon
{
    int Range();
    int Damage();
    void OnWeaponUse(Tile toAttack);
}

public interface IThrowable
{

}

public interface ILimitPlacement
{
    //These types can be in the occupied of another tile
    List<Tile> placeableOn();
    //this is the actual tile you need to check
    bool CanBePlacedOn(Tile tile);
}

public interface IDestroyOccupied
{

}

public interface IContextMenu
{
    Sprite DisplaySprite();
    string Description();
    int TotalHeath();
    int CurrentHealth();
    List<StatInfo> Stats();
    bool HasStats();

    //for setup:

    /*[SerializeField]
    private Sprite displaySprite;
    [SerializeField]
    private string description;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private List<Stat> stats;

    public string Description()
    {
        return description;
    }

    public Sprite DisplaySprite()
    {
        return displaySprite;
    }

    public int TotalHeath()
    {
        return maxHealth;
    }

    public int CurrentHealth()
    {
        return currentHealth;
    }

    public List<StatInfo> Stats()
    {
        return stats;
    }*/
}
