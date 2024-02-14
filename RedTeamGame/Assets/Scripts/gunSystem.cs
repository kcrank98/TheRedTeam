using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunSystem : MonoBehaviour
{
    public enum GunType
    {
        Pistol,
        Rifle
        // Add more gun types as needed
    }

    [System.Serializable]
    public class GunProperties
    {
        public GunType gunType;
        public float shootRate;
        public int shootDamage;
        public int shootDistance;
    }

    public GunProperties currentGun;

    private void Start()
    {
        // Set the default gun when the game starts
        SwitchGun(GunType.Pistol);
    }

    public void SwitchGun(GunType gunType)
    {
        // Set the properties of the selected gun
        switch (gunType)
        {
            case GunType.Pistol:
                currentGun = new GunProperties { gunType = GunType.Pistol, shootRate = 0.5f, shootDamage = 2, shootDistance = 20 };
                break;
            case GunType.Rifle:
                currentGun = new GunProperties { gunType = GunType.Rifle, shootRate = 0.1f, shootDamage = 10, shootDistance = 100 };
                break;
                // Add cases for more gun types if needed
        }
    }
    public GunProperties GetCurrentGun()
    {
        return currentGun;
    }

}
