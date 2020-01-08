using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Weapon, IAttack
{
    Movement move;

    [Header("Explosion Stats")]

    [SerializeField]
    private int ExplosionDamage;

    [SerializeField]
    private int explosionRange;

    [SerializeField]
    private float explosionDelay;

    [SerializeField]
    private int explosionTurnWait;
    int turnWait;

    [Header("Explosion Effects")]

    public GameObject explosionMarker;
    public float explosionSpeed;
    public float explosionTime;

    GameObject explosionParent;
    [Space]
    public float throwSpeed = 10f;

    public bool canExplode = false;

    bool thrown = false;

    void Start()
    {
        turnWait = explosionTurnWait;
        move = transform.GetComponent<Movement>();
        move.discriminateAttack = false;
        FindObjectOfType<Interaction>().selectionEvent += OnSelection;
        FindObjectOfType<Interaction>().selectionEvent -= move.OnSelection;

        //occupied = transform.parent.gameObject.GetComponent<Tile>().occupied;
    }

    public void Explode()
    {
        Tile[] explode = move.GetTileRange(0, explosionRange).ToArray();

        foreach (Tile tile in explode)
        {
            if (tile.occupied != null)
            {
                /*Health health = tile.occupied.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(ExplosionDamage); /////////////////////////////////////////////////////////////////////////////////////////////////
                }*/
            }
        }

        if (explosionMarker != null)
        {
            explosionParent = new GameObject();
            explosionParent.transform.parent = transform;

            foreach (Tile tile in explode)
            {
                Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - 0.001f);
                Instantiate(explosionMarker, pos, Quaternion.identity, explosionParent.transform);
            }

            explosionParent.SetActive(false);

            FindObjectOfType<Interaction>().selectionEvent -= OnSelection;

            StartCoroutine(Explosion());
        }

    }

    public void OnAttack(Tile tile)
    {
        //Explode();
    }

    public bool InRange(Vector3 toAttack)
    {
        if (Vector3.Distance(transform.position, toAttack) < GetRange() * 1.5)
        {
            return true;
        }
        else return false;
    }

    public int GetRange()
    {
        return move.range;
    }

    public int GetMinRange()
    {
        return MinRange();
    }

    public int AttackDamage()
    {
        return Damage();
    }

    public override void OnWeaponUse(Tile toAttack)
    {
        if (GetComponent<Grenade>() && toAttack.IsOccupied() == false)
        {
            /////////////////////////////////////////////////////////////////if (occupied != null) occupied.UnOccupy();
            toAttack.AddOccupy(this);
            Vector3 pos = new Vector3(toAttack.transform.position.x, toAttack.transform.position.y, toAttack.transform.position.z - 0.01f);
            StartCoroutine(translate(pos));
            FindObjectOfType<TurnManager>().turnChangeEvent += OnTurnChange;
            thrown = true;

        }
    }

    public void OnSelection(GameObject selected)
    {
        Tile[] mes = GetComponentsInParent<Tile>();
        if (mes.Length < 1) goto end;
        Tile me = mes[mes.Length - 1]; // get last parent with Tile Component == Character
        GameObject meObj = me.transform.gameObject;

        if (meObj.GetComponent<Character>() == null) goto end;
        Character meChar = meObj.GetComponent<Character>();
        

        if (selected != null && !thrown) // can't throw at nothing and has to not have been thrown
        {
            if (selected == meObj) // parent Character must be selected to throw
            {
                move.OnSelection(gameObject);
                canExplode = true;
            }
            else if (move.showingSelection) { move.ClearSelection(); canExplode = false; }

            if (meChar.IsMoveable() == false) canExplode = false;

            if (canExplode &&
                selected.GetComponent<Tile>() &&
                selected.GetComponent<Tile>().IsOccupied() == false &&
                InRange(selected.transform.position))
            {
                Tile toTry = selected.GetComponent<Tile>();
                OnWeaponUse(toTry);
            }
        }

        end:
        return;
    }

    IEnumerator Explosion()
    {
        float time = explosionTime;
        explosionParent.SetActive(true);
        GameObject[] objs = new GameObject[explosionParent.transform.childCount];
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i] = explosionParent.transform.GetChild(i).gameObject;
        }

        while (time >= 0)
        {
            foreach (GameObject obj in objs)
            {
                Transform t = obj.transform;
                t.localRotation = new Quaternion(t.localRotation.x, t.localRotation.y, t.localRotation.z + explosionSpeed * Time.deltaTime, t.localRotation.w);
            }

            time -= Time.deltaTime;
            yield return null;
        }
        FindObjectOfType<Interaction>().selectionEvent -= move.OnSelection;
        Destroy(gameObject);
        yield return null;
    }

    IEnumerator translate(Vector3 tragetPosition)
    {
        while (Vector3.Distance(transform.position, tragetPosition) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, tragetPosition, Time.deltaTime * throwSpeed);
            yield return null;
        }

        //yield return new WaitForSeconds(explosionDelay);

        yield return null;

        //Explode();
    }

    public bool GetLockPerpendicular()
    {
        return LockPerpendicular();
    }

    public bool GetLockDiagonal()
    {
        return LockDiagonal();
    }

    public void OnTurnChange(Party party)
    {
        turnWait -= 1;
        if(turnWait <=0)
        {
            Explode();
        }
    }

    void OnDestroy()
    {
        if (FindObjectOfType<Interaction>()) 
            FindObjectOfType<Interaction>().selectionEvent -= OnSelection;
        if (FindObjectOfType<Interaction>())
            FindObjectOfType<TurnManager>().turnChangeEvent -= OnTurnChange;
    }
}
