using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class gun : MonoBehaviour
{
    [SerializeField] public float fireRate;
    [SerializeField] public float damage;
    [SerializeField] public float maxDistance;
    [SerializeField] public float reloadTime;


    public virtual void Shoot()
    {
        StartCoroutine(ShootCoroutine());
    }

    protected IEnumerator ShootCoroutine()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, maxDistance))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage((int)damage);
            }
        }

        yield return new WaitForSeconds(fireRate);
    }
}

