using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContextMenu : MonoBehaviour
{

    public Interaction interaction;
    public Image display;
    public Sprite empty;
    public StatBar health;

    public TextMeshProUGUI text;

    public List<Stat> statSlots;

    void Start()
    {
        if (interaction == null) interaction = FindObjectOfType<Interaction>();
        interaction.selectionEvent += SetContext;
        SetContext(null);
    }

    public void SetContext(GameObject toSet)
    {
        Debug.Log("Tried to setContext: " +toSet );
        try
        {
            IContextMenu context = toSet.GetComponent<IContextMenu>();
            //Debug.Log("1");
            display.sprite = context.DisplaySprite();
            //Debug.Log("2");
            SetHealth(context.CurrentHealth(), context.TotalHeath());
            //Debug.Log("3");
            text.text = context.Description();
            Debug.Log(4);
            if(context.HasStats())
            SetStats(context.Stats());
            else ClearStats();

        }
        catch(System.NullReferenceException)
        {
            ClearContext();
            
        }

        if(toSet == null)
        {
            ClearContext();
        }
    }

    public void ClearContext()
    {
        ClearStats();
        display.sprite = empty;
        health.gameObject.SetActive(false);
        text.text = "";
        Debug.Log("Cleared Context");
    }

    public void ClearStats()
    {
        foreach (Stat stat in statSlots)
        {
            stat.ClearStat();
        }
    }

    public void SetHealth(int current, int max)
    {
        if (!health.isActiveAndEnabled) health.gameObject.SetActive(true);
        health.SetPercentFull(current, max);
    }

    public void SetStats(List<StatInfo>toSet)
    {
        for (int i = 0; i < toSet.Count; i++)
        {
            if(i > statSlots.Count) { Debug.Log("Too many stats m8"); return; }

            statSlots[i].SetStat(toSet[i]);
        }
    }
}
