using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Party : MonoBehaviour
{
    Map map;

    [SerializeField]
    private List<GameObject> members;

    public delegate void TurnDelegate(bool ourTurn);
    public event TurnDelegate turnEvent;

    void Start()
    {
        OnStart();
    }

    public void OnStart()
    {
        map = FindObjectOfType<Map>();
        TurnManager turnManager = FindObjectOfType<TurnManager>();
        turnManager.turnChangeEvent += OnTurnChange;
        turnManager.parties.Add(this);

        List<GameObject> membersUpdated = new List<GameObject>();
        foreach (GameObject member in members)
        {
            if (member != null)
            {
                //if (character == null) continue;
                GameObject obj = null;
                //map.SpawnTileRandom(member, out obj);
                membersUpdated.Add(obj);
                Character character = obj.GetComponent<Character>();
                character.SetParty(this);
                if (character == null) continue;
                turnEvent += character.OnTurnChange;
            }
        }
        members = membersUpdated;
    }

    void OnTurnChange(Party party)
    {
        if (party == this)
        {
            OnTurnStart();
        }

        else if(party != this)
        {
            OnTurnEnd();
        }

        void OnTurnStart()
        {
            turnEvent?.Invoke(true);
        }

        void OnTurnEnd()
        {
            turnEvent?.Invoke(false);

        }
    }

    public void AddMember(GameObject member)
    {
        if (members == null) members = new List<GameObject>();
        members.Add(member);
    }

    public void ClearParty()
    {
        if (members == null) return;
        /*foreach(GameObject member in members)
        {
            if(member.GetComponent<Character>())
            {
                member.GetComponent<Character>().Die();
            }
        }*/
        members.Clear();
    }
}
