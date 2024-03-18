using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lavaController : MonoBehaviour
{
    bool isInLava;
    [SerializeField] int damageAmount;
    private void OnTriggerEnter(Collider other)
    {
        isInLava = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isInLava = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (isInLava)
        {


            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damageAmount);

            }

        }
    }
    
}
