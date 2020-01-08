using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Attack))]
public class Lunge : Ability
{
    public bool analyseMove;
    public int lungeDepht;

    void OnDisable()
    {
        enabled = true;
    }

    public override void AnalyseMove()
    {
        
        base.AnalyseMove();
        Debug.Log(transform.name + "lunges at");
        Tile[] toAttack = ToLunge().ToArray();
        Debug.Log(toAttack.Length+ "enemies");
        foreach(Tile toLunge in toAttack)
        {
            Debug.Log("lunging...");
            if (toLunge.occupied == null) { Debug.Log("toLunge.Occupied == null"); break; }
            ///////////////////////////////////////////////////////////////////////////////////////attack.OnAttack(toLunge.occupied);
            Debug.Log("Lunged");
        }
    }

    public List<Tile> ToLunge()
    {
        List<Tile> lunge = new List<Tile>();
        int lastX = LastTile().XCoord;
        int lastY = LastTile().YCoord;

        int X = currentTile.XCoord;
        int Y = currentTile.YCoord;

        int lungeX = X;
        int lungeY = Y;

        int coeffX = 0;
        int coeffY = 0;

        if( lastX - X < 0) // means you moved away from the origin on the X axis
        {
            lungeX = X + lungeDepht;
            coeffX = 1;
        }

        else if(lastX - X > 0) // means you moved closer to the origin on the X axis
        {
            lungeX = X - lungeDepht;
            coeffX = -1;
        }

        if (lastY - Y < 0) // means you moved away from the origin on the Y axis
        {
            lungeY = Y + lungeDepht;
            coeffY = 1;
        }
        else if (lastY - Y > 0) // means you moved closer to the origin on the Y axis
        {
            lungeY = Y - lungeDepht;
            coeffY = -1;
        }

        int x = X;
        int y = Y;

        int tried = 0;

        while(x != lungeX || y != lungeY)
        {
            x += coeffX;
            y += coeffY;

            try
            {
                Tile toLunge = map.map[x, y].GetComponent<Tile>();
                lunge.Add(toLunge);
            }
            catch (System.IndexOutOfRangeException) { }
            
            tried += 1;
            if (tried >= 200) break;
        }

        return lunge;
    }

    void Die()
    {
        if (move != null)
        {
            move.moveEvent -= AnalyseMove;
        }
    }

    void OnDestroy()
    {
        Die();
    }
}
