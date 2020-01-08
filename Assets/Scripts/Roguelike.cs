using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurnManager))]
public class Roguelike : MonoBehaviour
{
    Map map;
    TurnManager turnManager;
    
    public bool nextRoom = false;

    public GameObject roomDisplay;

    [Header("Characters")]
    public List<GameObject> enemies;
    public List<GameObject> player;

    [Header("Enemy Party")]
    public Party enemyParty;

    void Update()
    {
        if(nextRoom)
        {
            nextRoom = false;
            NextRoom();
        }
    }

    public int room = 0;
    
    void Awake()
    {
        map = FindObjectOfType<Map>();
        turnManager = GetComponent<TurnManager>();
        SetDisplay();
    }

    public void NextRoom()
    {
        room++;
        //map.GenerateMap();
        newEnemies();
        turnManager.Reset();
        turnManager.NextTurn();
        SetDisplay();
    }

    void newEnemies()
    {
        if (enemyParty == null) Debug.Log("You need to set the EnemyParty manually");
        enemyParty.ClearParty();
        for (int i = 0; i < room; i++)
        {
            int e = Random.Range(0, enemies.Count);
            enemyParty.AddMember(enemies[e]);
        }
    }

    void SetDisplay()
    {
        if (roomDisplay != null && roomDisplay.GetComponent<TextMesh>())
        {
            roomDisplay.GetComponent<TextMesh>().text = "Room " + room;
        }
    }
}
