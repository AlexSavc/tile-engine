using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatBar : MonoBehaviour
{
    public Image background;
    public Image fill;
    public TextMeshProUGUI text;

    public Vector3 Pos0;
    public Vector3 Pos100;

    [SerializeField]
    private float percentFull;

    public int maxFull;
    public int currentFull;

    public bool setLocalPos0;
    public bool setLocalPos100;


    void OnValidate()
    {
        if(setLocalPos0)
        {
            Pos0 = fill.transform.localPosition;
            setLocalPos0 = false;
        }
        if(setLocalPos100)
        {
            Pos100 = fill.transform.localPosition;
            setLocalPos100 = false;
        }

        if (percentFull > 100) percentFull = 100;
        if (percentFull < 0) percentFull = 0;
        SetBarDisplay();
    }

    void SetBarDisplay()
    {
        if(maxFull != 0)
        fill.transform.localPosition = new Vector3(((Pos0.x * (100 - percentFull)/ 100) + Pos100.x), 0);
        SetTextMesh();
    }

    /*public void SetPercentFull(float percent)
    {
        percentFull = percent;
        SetBarDisplay();
    }*/

    public void SetPercentFull(float current, float max)
    {
        percentFull = (current / max) * 100f;
        maxFull = (int)max;
        currentFull = (int)current;
        SetBarDisplay();
    }

    public void SetTextMesh()
    {
        
        if(maxFull == 0)
        {
            if (currentFull == 0)
            {
                text.text = "";
            }
            else text.text = currentFull.ToString();
        }
        else
        {
            text.text = currentFull + "/" + maxFull;
        }
    }
}

[System.Serializable]
public class Materium
{
    public string Name;
    public float currentPressure;
    public Texture label;
    public Material material;
    public List<Tile> composition = new List<Tile>();
}
