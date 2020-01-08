using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : Tile, IInteractable, IMoveable
{
    private Movement movement;
    private Map map;

    [Header("Character")]
    [SerializeField]
    private bool moveable;
    [SerializeField]
    private Party myParty;
    [SerializeField]
    private int actionsPerTurn;
    int actionsLeft;

    private List<Movement> moves;
    private List<Attack> attacks;

    void Awake()
    {
        moves = new List<Movement>();
        attacks = new List<Attack>();

        foreach(Movement move in GetComponents<Movement>())
        {
            moves.Add(move);
        }
        foreach (Attack attacc in GetComponents<Attack>())
        {
            attacks.Add(attacc);
        }
        foreach (Movement move in GetComponentsInChildren<Movement>())
        {
            moves.Add(move);
        }
        foreach (Attack attacc in GetComponentsInChildren<Attack>())
        {
            attacks.Add(attacc);
        }

        if (movement == null)
        {
            movement = GetComponent<Movement>();
        }
        if (map == null)
        {
            map = FindObjectOfType<Map>();
        }
        if(GetComponent<Health>())
        {
            GetComponent<Health>().deathEvent += Die;
        }
        GetComponent<Tile>().occupyEvent += SetOccupied;
    }

    public void OnInteraction(GameObject toInteract)
    {
        
    }
    
    public void OnMovement(Vector3 MoveTo)
    {
        UseAction();


        Tile tile = map.GetTileFromVector(MoveTo);
        movement.Move(tile);
        occupied = base.occupied;

        SetActions();
    }

    public void SetOccupied(Tile newOccupied)
    {
        //////////////////////////////////////////////////////////////////////////////occupied = newOccupied;
    }

    public void OnTurnChange(bool ourTurn)
    {
        actionsLeft = actionsPerTurn;
        moveable = ourTurn;
        SetActions();
    }

    public Party GetParty()
    {
        return myParty;
    }

    public void SetParty(Party party)
    {
        myParty = party;
    }

    void SetActions()
    {
        if (moveable && actionsLeft > 0)
        {
            foreach (Attack attacc in attacks)
            {
                attacc.SetAttack(true);
            }
            foreach (Movement move in moves)
            {
                move.SetMoveable(true);
            }
        }
        else
        {
            foreach (Attack attacc in attacks)
            {
                attacc.SetAttack(false);
            }
            foreach (Movement move in moves)
            {
                move.SetMoveable(false);
            }
        }
    }

    public void UseAction()
    {
        actionsLeft--;
    }

    public bool IsMoveable()
    {
        return moveable;
    }

    public void Die()
    {
        myParty.turnEvent -= OnTurnChange;
        GetComponent<Health>().deathEvent -= Die;
        GetComponent<Tile>().occupyEvent -= SetOccupied;
    }
}
