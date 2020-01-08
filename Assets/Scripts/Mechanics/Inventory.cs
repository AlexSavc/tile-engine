using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Equipped Item")]
    [SerializeField]
    private GameObject equipped;

    [Header("Inventory")]
    [SerializeField]
    private List<GameObject> inventory;

    [Space]

    [SerializeField]
    private int capacity = 5;
    [SerializeField]
    private GameObject inventoryParent;
    
    void OnValidate()
    {
        CheckInventory();
    }

    void Start()
    {
        if(equipped != null)
        {
            equipped.SetActive(true);
        }
        
        if(inventoryParent == null)
        {
            inventoryParent = Instantiate(new GameObject(), transform);
            inventoryParent.name = "InventoryParent";
        }

        CreateInventory();

        CheckInventory();

        foreach(GameObject obj in inventory)
        {
            if (obj != null)
            {
                int index = inventory.IndexOf(obj);
                
                //obj.SetActive(false);
                //inventory[index] = Instantiate(obj, inventoryParent.transform);
            }
        }

        if(equipped != null)
        {
            equipped = Instantiate(equipped, inventoryParent.transform);
        }
    }

    public void DisplayEquipped()
    {

    }

    public bool HasItemComponent(GameObject toCheck)
    {
        if (toCheck.GetComponent<Item>()) return true;
        else return false;
    }

    public GameObject[] GetInventory()
    { 
        return inventory.ToArray();
    }

    public GameObject GetEquipped()
    {
        return equipped;
    }
    
    public void Equip(GameObject toEquip, out bool success)
    {
        success = false;

        if (HasItemComponent(toEquip))
        {
            if (equipped == null && inventory.Contains(toEquip) == false)
            {
                equipped = toEquip; success = true; return;
            }
            else SwapItems(toEquip, equipped, out success);
            if (success) return;

            else AddItem(toEquip, out success);
            return;
        }
    }

    public void UnEquip(out bool success)
    {
        success = false;
        SwapItems(equipped, null, out success);
    }

    public void Add(GameObject toAdd, out bool success)
    {
        if (HasItemComponent(toAdd))
        {
            AddItem(toAdd, out success);
            return;
        }
        else
        {
            Debug.LogError("No Item Component in GameObject toAdd to inventory");
            success = false;
            return;
        }
    }

    public void Remove(GameObject toRemove, out bool success)
    {
        if (HasItemComponent(toRemove))
        {
            RemoveItem(toRemove, out success);
            return;
        }
        else
        {
            Debug.LogError("No Item Component in GameObject toAdd to inventory");
            success = false;
            return;
        }
    }
    
    void AddItem(GameObject toAdd, out bool success)
    {
        success = false;

        if (HasItemComponent(toAdd) == false || inventory.Contains(toAdd))
        {
            return;
        }

        else if (equipped == null)
        {
            Equip(toAdd, out success);
            return;
        }

        else if (inventory.Contains(null))
        {
            int i = inventory.IndexOf(null);
            inventory[i] = toAdd;
            success = true;
            return;
        }
    }

    public void RemoveItem(GameObject toRemove, out bool success)
    {
        success = false;

        if (HasItemComponent(toRemove) == false)
        {
            return;
        }

        if (equipped == toRemove)
        {
            RemoveEquipped();
            success = true;
        }

        else if (inventory.Contains(toRemove))
        {
            int i = inventory.IndexOf(toRemove);
            inventory[i] = null;
            success = true;
        }
    }

    void SwapItems(GameObject swapper, GameObject swappee, out bool success)
    {
        success = false;

        if (HasItemComponent(swapper) == false || HasItemComponent(swappee) == false)
        {
            return;
        }

        if (inventory.Contains(swapper) && inventory.Contains(swappee))
        {
            inventory[inventory.IndexOf(swappee)] = swapper;
            inventory[inventory.IndexOf(swapper)] = swappee;
            success = true;
        }
    }

    public void RemoveEquipped()
    {
        equipped = null;
    }

    void CheckInventory()
    {
        if (inventory == null) return;
        if (capacity > 200) capacity = 200;
        if (inventory.Count > capacity)
        {
            while (inventory.Count > capacity)
            {
                if (inventory.Contains(null))
                {
                    inventory.RemoveAt(inventory.IndexOf(null));
                }
                else inventory.RemoveAt(0);
            }
        }
            
        if(inventory.Count < capacity)
        {
            while (inventory.Count < capacity)
            {
                inventory.Add(null);
            }
        }
    }

    void CreateInventory()
    {
        if (inventory == null)
        {
            inventory = new List<GameObject>(capacity);
        }
    }
}
