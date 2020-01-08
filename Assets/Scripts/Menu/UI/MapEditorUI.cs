using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour
{

    [Header("Editor")]
    
    public MapEditor editor;

    [Header("UI")]

    public GameObject paintButton;
    public bool painting = false;
    public GameObject erasorButton;
    public bool erasing = false;

    /*[Space]
    public GameObject buttonTemplate;
    public GameObject outButton;
    public RectTransform scrollContent;
    public int yOffset;

    public GameObject holder;
    private GameObject UIHolder;

    [Header("Tiles")]
    [Space]
    //All Lists must be HARDCODED in SetDisplayTiles() and In(int i);
    public List<Tile> floor;
    public List<Tile> furniture;
    public List<Tile> buildings;
    public List<Tile> resources;
    public List<Tile> walls;
    public List<Tile> stairs;
    */
    //[Header("Display")]

    //public Image selectDisplay;
    //public GameObject selectedUI;
    //public Sprite defaultDisplay;

    //private List<Tile> displayTiles;

    public delegate void SelectionDelegate(GameObject tile);
    public event SelectionDelegate selectionEvent;

    public delegate void PaintingDelegate(bool painting);
    public event PaintingDelegate paintEvent;

    public delegate void ErasingDelegate(bool erasing);
    public event ErasingDelegate eraseEvent;

    public void Start()
    {
        if (FindObjectOfType<PanZoom>() != null)
        {
            paintEvent += FindObjectOfType<PanZoom>().SetLimit;
            eraseEvent += FindObjectOfType<PanZoom>().SetLimit;
        }

        if (editor == null)
        {
            editor = FindObjectOfType<MapEditor>();
        }

        //SetDisplayTiles();

        //outButton.GetComponent<Button>().onClick.AddListener(delegate { SetOutUI(); });

        //SetOutUI();

        SetBrush(false);
        SetErasor(false);

        //selectDisplay.sprite = defaultDisplay;

        //selectionEvent += editor.OnSelection;
        paintEvent += editor.SetCanPaint;
        eraseEvent += editor.SetErase;
    }

    

    public void OnBrushSelection()
    {
        if (painting)
        {
            SetBrush(false);
        }
        else
        {
            SetBrush(true);
        }
    }

    public void SetBrush(bool select)
    {
        Image image = paintButton/*.transform.GetChild(0)*/.GetComponent<Image>();
        Color color = image.color;

        if (select == false)
        {
            Color c = new Color(color.r, color.g, color.b, 0.2f);
            image.color = c;
            painting = false;
        }
        else
        {
            Color c = new Color(color.r, color.g, color.b, 1f);
            image.color = c;
            painting = true;

            SetErasor(false);
        }

        paintEvent?.Invoke(painting);
    }

    public void OnErasorSelection()
    {
        if (erasing)
        {
            SetErasor(false);
        }
        else
        {
            SetErasor(true);
        }
    }

    public void SetErasor(bool select)
    {
        Image image = erasorButton/*.transform.GetChild(0)*/.GetComponent<Image>();
        Color color = image.color;

        if (select == false)
        {
            Color c = new Color(color.r, color.g, color.b, 0.2f);
            image.color = c;
            erasing = false;

        }
        else
        {
            Color c = new Color(color.r, color.g, color.b, 1f);
            image.color = c;
            erasing = true;

            SetBrush(false);

        }
        eraseEvent?.Invoke(erasing);
    }

    /*public void CreateButtons(List<Tile> list)
    {
        if (list.Count == 0) return;
        if (list == null) Debug.Log("List Null");

        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                Tile tile = list[i];

                GameObject button = Instantiate(buttonTemplate, UIHolder.transform);
                button.transform.localPosition = new Vector3(0,- (i * yOffset), 0);

                GameObject buttonChilde = button.transform.GetChild(0).gameObject;

                button.GetComponent<Button>().onClick.AddListener(delegate { OnPress(tile.gameObject); });

                buttonChilde.GetComponent<Image>().sprite = tile.tileSprite;

            }
            catch (System.Exception) { Debug.Log("THE BUTTON IS WRONG YOU RETARD"); }
        }

        scrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (yOffset * 0.45f) * list.Count);
    }

    public void SelectUITile(GameObject toSelect)
    {
        if(selectedUI != null && toSelect == selectedUI)
        {
            selectedUI = null;
            selectDisplay.sprite = defaultDisplay;
        }
        else
        {
            selectedUI = toSelect;
            selectDisplay.sprite = toSelect.GetComponent<Tile>().tileSprite;
            SetBrush(true);
        }

        selectionEvent?.Invoke(selectedUI);
    }

    void SetUI(List<Tile> list)
    {

        if (UIHolder != null)
        {
            Destroy(UIHolder);
        }

        UIHolder = new GameObject
        {
            name = "UIHolder",
        };

        UIHolder.transform.parent = holder.transform;
        UIHolder.transform.localPosition = Vector3.zero;

        CreateButtons(list);
    }

    public void SetOutUI()
    {
        outButton.SetActive(false);
        
        if (UIHolder != null)
        {
            Destroy(UIHolder);
        }

        UIHolder = new GameObject
        {
            name = "UIHolder",
        };

        UIHolder.transform.parent = holder.transform;
        UIHolder.transform.localPosition = Vector3.zero;

        if (displayTiles.Count < 1) return;

        for (int i = 0; i < displayTiles.Count; i++)
        {
            try
            {
                Tile tile = displayTiles[i];

                GameObject button = Instantiate(buttonTemplate, UIHolder.transform);
                button.transform.localPosition = new Vector3(0, -(i * yOffset), 0);

                GameObject buttonChilde = button.transform.GetChild(0).gameObject;

                int b = i;

                button.GetComponent<Button>().onClick.AddListener(delegate { In(b); });

                buttonChilde.GetComponent<Image>().sprite = tile.tileSprite;

            }
            catch (System.Exception) { Debug.Log("THE BUTTON IS WRONG YOU RETARD"); }
        }

        scrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (yOffset *0.45f)* displayTiles.Count);
    }

    public void In(int index)
    {
        List<Tile> list = null;
        if (index == 0) list = floor;
        if (index == 1) list = walls;
        if (index == 2) list = furniture;
        if (index == 3) list = stairs;
        if (index == 4) list = buildings;
        if (index == 5) list = resources;

        SetUI(list);

        outButton.SetActive(true);
    }

    public void OnPress(GameObject tile)
    {
        SelectUITile(tile);
    }

    void SetDisplayTiles()
    {
        displayTiles = new List<Tile>();

        displayTiles.Add(floor[0]);
        displayTiles.Add(walls[0]);
        displayTiles.Add(furniture[0]);
        displayTiles.Add(stairs[0]);
        displayTiles.Add(buildings[0]);
        displayTiles.Add(resources[0]);
    }*/


}
