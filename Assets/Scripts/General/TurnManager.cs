using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private int currentTurn = 0;

    public bool nextTurn; 

    [SerializeField]
    public List<Party> parties;

    [SerializeField]
    private Party partyTurn;

    public GameObject turnDisplay;

    public delegate void TurnChangeDelegate(Party party);
    public event TurnChangeDelegate turnChangeEvent;

    void Update()
    {
        if(nextTurn)
        {
            NextTurn();
            nextTurn = false;
        }
    }

    void Start()
    {
        NextTurn();
    }

    public void Reset()
    {
        currentTurn = 0;
        List<Party> oldParties = new List<Party>(parties);
        parties.Clear();
        foreach (Party party in oldParties)
        {
            party.OnStart();
        }
    }

    public void NextTurn()
    {
        if (parties.Count <= 0) return;
        currentTurn += 1;
        nextParty();
        turnChangeEvent?.Invoke(partyTurn);
        SetDisplay();
    }

    Party nextParty()
    {
        int i = parties.IndexOf(partyTurn);
        
        i++;

        if (i >= parties.Count)
        {
            partyTurn = parties[0];
            return parties[0];
        }
        else
        {
            partyTurn = parties[i];
            return parties[i];
        }
    }

    public void AddParty(Party party)
    {
        parties.Add(party);
    }

    public void ClearParties()
    {
        parties.Clear();
    }

    void SetDisplay()
    {
        if(turnDisplay != null && turnDisplay.GetComponent<TextMesh>())
        {
            turnDisplay.GetComponent<TextMesh>().text = partyTurn.name;
        }
    }
}
