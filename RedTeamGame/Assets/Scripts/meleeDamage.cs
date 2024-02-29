using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeDamage : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] int pushPower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();
        IPushBack push = other.GetComponent<IPushBack>();

        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }

        if(push != null)
        {
            push.pushBackDir((other.transform.position - transform.position).normalized * damageAmount * pushPower);
        }
    }
}
