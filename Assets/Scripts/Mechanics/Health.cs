using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, IHealth, IInteractable
{
    [Header("Health Stats")]
    public int health;

    [SerializeField]
    private float offsetX = 0.1f;
    [SerializeField]
    private float offsetZ = 0.02f;

    Map map;


    [Header("Health Display")]
    public GameObject heart;
    public GameObject healthDisplayParent;
    List<GameObject> healthObjs;

    [Space]
    GameObject damageObj;
    public GameObject gore;
    public GameObject damageMarker;
    public float goreOffset = 0.002f;

    [SerializeField]
    private List<GameObject> drops;
    
    

    public delegate void DeathDelegate();
    public event DeathDelegate deathEvent;

    void Awake()
    {
        map = FindObjectOfType<Map>();
        CreateHealthDisplay();
    }

    public void OnInteraction(GameObject toInteract)
    {
        try
        {
            IAttack attack = toInteract.GetComponent<IAttack>();
            
            if(toInteract != gameObject && attack.InRange(transform.position) == true ) 
            {
                //TakeDamage(attack.AttackDamage());
                if (GetComponent<Tile>() != null)
                {
                    attack.OnAttack(GetComponent<Tile>());
                }
            }
        }
        catch (NullReferenceException) { }
    }

    public int CurrentHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        if (damage == 0) return;
        health -= damage;
        StartCoroutine(showDamage());
        if (health <= 0)
        {
            if (damageObj != null) Destroy(damageObj);
            Die();
        }
        RemoveHealthDisplay();
    }

    IEnumerator showDamage()
    {
        if (damageMarker != null)
        {
            if(damageObj != null) Destroy(damageObj);
            Vector3 pos = transform.position;
            pos.z += goreOffset;
            damageObj = Instantiate(damageMarker, pos, Quaternion.identity, map.tileParent.transform);
            yield return new WaitForSeconds(0.2f);
            Destroy(damageObj);
        }
        yield return null;
    }

    public bool FriendlyFire(Party attacker)
    {
        Character me = GetComponent<Character>();
        if (me.GetParty() != attacker) return false;
        else return true;
    }

    void CreateHealthDisplay()
    {
        if (healthDisplayParent == null)
        {
            healthDisplayParent = new GameObject() { name = "healthDisplay" };
            healthDisplayParent.transform.parent = transform;
            healthDisplayParent.transform.localPosition = new Vector3(0, 0, -offsetZ);
        }

        healthObjs = new List<GameObject>();

        ClearHealthDisplay();

        for(int i = 0; i < health; i++)
        {
            AddHealthDisplay();
        }
    }

    void SetHealthDisplay()
    {
        foreach(GameObject heart in healthObjs)
        {
            Vector3 pos = new Vector3(GetHealthDisplayXOffset(healthObjs.Count, healthObjs.IndexOf(heart)), 0, 0);
            heart.transform.localPosition = pos;
        }
    }

    void AddHealthDisplay()
    {
        if (heart == null) 
        {
            Debug.Log("No health obj indicator: Heart missing");
            return;
        }
        GameObject obj = Instantiate(heart, healthDisplayParent.transform);
        healthObjs.Add(obj);
        SetHealthDisplay();
    }

    void RemoveHealthDisplay()
    {
        if (healthObjs.Count <= 0) return;
        GameObject obj = healthObjs[healthObjs.Count-1];
        healthObjs.Remove(obj);
        Destroy(obj);
        SetHealthDisplay();
    }

    void ClearHealthDisplay()
    {
        int c = healthDisplayParent.transform.childCount;
        for(int i = 0; i< c; i++)
        {
            Destroy(healthDisplayParent.transform.GetChild(i).gameObject);
        }
    }

    float GetHealthDisplayXOffset(float total, float count)
    {
        float step = 1 / total;
        float i = count * step;
        return i;
    }

    void Die()
    {
        if(deathEvent != null)
        {
            deathEvent();
        }

        if(gore != null)
        {
            Vector3 GorePos = new Vector3(transform.position.x, transform.position.y, transform.position.z - goreOffset);
            GameObject Gore = Instantiate(gore, GorePos, Quaternion.identity, map.tileParent.transform);
        }
        Destroy(transform.gameObject);
    }
}