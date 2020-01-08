using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Weapon
{
    [Header("Projectile Settings")]
    public GameObject projectile;
    public Vector3 projectileOrigin;
    
    private GameObject bullet;

    public int speed = 30;

    void Awake()
    {
        bullet = Instantiate(projectile, projectileOrigin, Quaternion.identity, transform);
        bullet.transform.localPosition = projectileOrigin;
        bullet.SetActive(false);
    }

    public override void OnWeaponUse(Tile toAttack)
    {
        base.OnWeaponUse(toAttack);
        bullet.SetActive(true);
        StartCoroutine(translate(toAttack.transform.position));
    }

    IEnumerator translate(Vector3 tragetPosition)
    {
        while (Vector3.Distance(bullet.transform.position, tragetPosition) > 0.05f)
        {
            bullet.transform.position = Vector3.Lerp(bullet.transform.position, tragetPosition, Time.deltaTime * speed);
            yield return null;
        }

        yield return null;
        bullet.transform.localPosition = projectileOrigin;
        bullet.SetActive(false);
    }
}
