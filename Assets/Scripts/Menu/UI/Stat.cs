using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Stat : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Sprite empty;
    public Image image;

    public int maxStat = 0; // optionnal;
    public int currentStat = 0;

    [SerializeField]
    private string statText;

    void Start()
    {
        GetTheStuff();
        SetDisplay();
    }

    void SetDisplay()
    {
        try
        {
            if (maxStat == 0)
            {
                statText = currentStat.ToString();
            }
            else statText = currentStat.ToString() + "/" + maxStat;

            if (maxStat == 0 && currentStat == 0) statText = "";

            text.text = statText;
        }
        catch (System.NullReferenceException) {}
        
    }

    public void GetTheStuff()
    {
        if (text == null) text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        if (image == null) image = GetComponent<Image>();
    }

    public void SetStat(int current)
    {
        currentStat = current;
        SetDisplay();
    }
    public void SetStat(int current, int max)
    {
        currentStat = current;
        maxStat = max;
        SetDisplay();
    }
    public void SetImage(Image sprite)
    {
        image = sprite;
        SetDisplay();
    }

    public void SetStat(StatInfo template)
    {
        image.sprite = template.sprite;
        maxStat = template.maxStat;
        currentStat = template.currentStat;
        SetDisplay();
    }

    public void SetMaxStat(int max)
    {
        maxStat = max;
        SetDisplay();
    }

    public void ClearStat()
    {
        image.sprite = empty;
        SetStat(0, 0);
    }
}

[System.Serializable]
public class StatInfo
{
    public string type;
    public Sprite sprite;
    [Header("if there is no maximum, set to 0")]
    public int maxStat;
    public int currentStat;
}

