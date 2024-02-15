using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunSystem : MonoBehaviour
{
    public gun currentGun;
    public GameObject gun;


    private void Start()
    {
        // Initialize with Pistol as the default gun
        
    }

    public void SwitchToPistol()
    {
        currentGun = GetComponent<pistol>();
    }

    public void SwitchToRifle()
    {
        currentGun = GetComponent<rifle>();
    }

    public void Shoot()
    {
        currentGun.Shoot();
    }

}
