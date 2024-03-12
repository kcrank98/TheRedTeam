using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spikeController : MonoBehaviour
{
    [SerializeField] int damageAmount;
    Animator anim;
    bool trapActive;
    bool hasDoneDmg;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("actavate");
    }
    public void toggleDamage()
    {
        trapActive = !trapActive;
    }
    public void ResetDamage()
    {
        hasDoneDmg = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (trapActive)
        {
            if (other.isTrigger)
                return;

            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null && !hasDoneDmg)
            {
                dmg.takeDamage(damageAmount);
                hasDoneDmg = true;
            }

        }
    }
}
