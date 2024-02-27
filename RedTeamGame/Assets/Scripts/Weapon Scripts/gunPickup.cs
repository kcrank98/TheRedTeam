using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.setActiveGun(gameManager.instance.addGunUi(gun.gunName));
            gameManager.instance.playerScript.getGunStats(gun);
            Destroy(gameObject);
        }
    }
}
