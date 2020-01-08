using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    /*[SerializeField]
    private bool dash;
    
    [SerializeField]
    public bool lunge;
    [SerializeField]
    private int lungeDepth;*/

    public int speed = 15;

    List<IEnumerator> enumerators;

    void Awake()
    {
        enumerators = new List<IEnumerator>();
    }

    public override void OnWeaponUse(Tile toAttack)
    {
        base.OnWeaponUse(toAttack);
        if (friendlyFire == true) return;
        if (enumerators.Count == 0)
        {
            enumerators.Add(translate(toAttack.transform.position));
            StartCoroutine(Play());
        }
    }
    
    public void AddCoroutine(IEnumerator enumerator)
    {
        enumerators.Add(enumerator);
    }

    IEnumerator Play()
    {
        while(enumerators.Count > 0)
        {
            StartCoroutine(enumerators[0]);
            yield return null;
        }
        yield return null;
    }

    IEnumerator translate(Vector3 tragetPosition)
    {
        Tile[] mes = GetComponentsInParent<Tile>();
        if (mes.Length < 1) ;///////////////////////////// goto end;
        Tile me = mes[mes.Length - 1]; // get last parent with Tile Component == Character
        /*Tile o = me.Occupied();
        Vector3 origin = o.transform.position;
        while (Vector3.Distance(transform.parent.position, tragetPosition) > 0.05f)
        {
            me.transform.position = Vector3.Lerp(transform.parent.position, tragetPosition, Time.deltaTime * speed);
            yield return null;
        }

        me.transform.position = origin;
        enumerators.RemoveAt(0);

        end:*/ //////////////////////////////////////////
        yield return null;
    }
}
