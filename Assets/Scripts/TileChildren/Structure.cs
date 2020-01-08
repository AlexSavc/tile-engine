using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Furniture, IContextMenu
{
    [Header("Context Menu Stats")]
    [SerializeField]
    private Sprite displaySprite;
    [SerializeField]
    private string description = "Barracks to train troops";
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private List<StatInfo> stats;

    public string Description()
    {
        return description;
    }

    public Sprite DisplaySprite()
    {
        return tileSprite;
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
    }

    public bool HasStats()
    {
        if (stats != null && stats.Count > 0) return true;
        else return false;
    }
}
