using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuDisplay : MonoBehaviour
{
    //they have to be in order in the Editor
    public List<Image> images;
    public List<Button> buttons;

    public List<Tile> displayTiles;

    [SerializeField]
    private Sprite defaultSprite;

    public string currentSubmenu;

    public bool editingEnabled;
    public MapEditorUI editorUI;
    public MapEditor editor;

    public delegate void SelectionDelegate(GameObject tile);
    public event SelectionDelegate selectionEvent;

    void Start()
    {
        SetUp();
        gameObject.SetActive(false);
        FindAllObjects();
        SetAllEvents();
    }

    void SetUp()
    {
        images.Clear();
        buttons.Clear();
        int c = transform.childCount;

        for(int i = 0; i< c; i++)
        {
            if(transform.GetChild(i).GetComponent<Image>())
            {
                Image image = transform.GetChild(i).GetComponent<Image>();
                images.Add(image);
                image.sprite = defaultSprite;
            }
            if (transform.GetChild(i).GetComponent<Button>())
            {
                Button button = transform.GetChild(i).GetComponent<Button>();
                buttons.Add(button);
                button.onClick.RemoveAllListeners();
            }
        }
    }

    public void SetDisplay(List<Tile> toDisplay)
    {
        gameObject.SetActive(true);
        ClearImages();
        displayTiles = toDisplay;

        for(int i = 0; i < toDisplay.Count; i++)
        {
            if (i > images.Count) goto next;
            images[i].sprite = toDisplay[i].tileSprite;
            int c = i;
            buttons[i].onClick.AddListener(delegate  { OnClick(displayTiles[c].gameObject); });
        }

        next:

        return;
    }

    public void ClearImages()
    {
        foreach(Image img in images)
        {
            img.sprite = defaultSprite;
        }
    }

    public void Close()
    {
        SetUp();
        SetCurrentSubMenu("null");
        gameObject.SetActive(false);
    }

    public void OnClick(GameObject toSelect)
    {
        selectionEvent?.Invoke(toSelect);
    }

    public void SetCurrentSubMenu(string menuName)
    {
        currentSubmenu = menuName;
    }

    void FindAllObjects()
    {
        if (editorUI == null) editorUI = FindObjectOfType<MapEditorUI>();
        if (editor == null) editor = FindObjectOfType<MapEditor>();
    }

    void SetAllEvents()
    {
        selectionEvent += editor.OnSelection;
    }
}
