using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    void OnDisable()
    {
        enabled = true;
    }

    public override void AnalyseMove()
    {
        base.AnalyseMove();

        Tile[] toAttack = ToDash().ToArray();
        
        foreach (Tile toLunge in toAttack)
        {
            if (toLunge == currentTile) continue;
            ////////////////////attack.OnAttack(toLunge.occupied);

            //Debug.Log("dashed");
        }
    }

    public List<Tile> ToDash()
    {
        List<Tile> dash = new List<Tile>();
        foreach (Tile t in adjacents)
        {
            int pos = System.Array.IndexOf(oldAdjacents, t);
            if (pos > -1) // that means it exists in oldAdjacent
            {
                dash.Add(t);
            }
        }
        return dash;
    }
}
