using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    [Header("Button")]
    [SerializeField]
    private Button myButton;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private bool open;
    [Header("Contents")]
    [SerializeField]
    private string menuName;
    [SerializeField]
    private List<Tile> tiles;
    [Space]
    public SubMenuDisplay display;


    void Awake()
    {
        if (tiles == null) tiles = new List<Tile>();

        if(display == null) display = FindObjectOfType<SubMenuDisplay>();
        if (myButton == null) myButton = GetComponent<Button>();
        if (image == null) image = GetComponent<Image>();
        
        if(tiles.Count > 0)
        {
            
            if (sprite == null) 
            {
                sprite = tiles[0].tileSprite;
                image.sprite = tiles[0].tileSprite;
            }
            else
            {
                image.sprite = sprite;
            }
        }

        myButton.onClick.AddListener(() => ToggleOpen());
    }

    void Open()
    {
        display.SetDisplay(tiles);
        display.currentSubmenu = menuName;
        //open = true;
    }

    void Close()
    {
        display.Close();
        //open = false;
    }

    void ToggleOpen()
    {
        if (display.currentSubmenu == menuName)
        {
            Close();
        }
        else Open();
    }
}
