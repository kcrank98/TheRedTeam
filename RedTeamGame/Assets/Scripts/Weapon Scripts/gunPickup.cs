using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;
    public Transform parentObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // gameManager.instance.setActiveGun(gameManager.instance.addGunUi(gun.gunName));
            gameManager.instance.playerScript.getGunStats(gun);

            //// Set the parent of the GameObject to the specified parentObject
            //transform.SetParent(parentObject);

            //// Reset the local position and scale relative to the parent
            //transform.localPosition = Vector3.zero;
            //transform.localRotation = Quaternion.identity;
            //transform.localScale = Vector3.one;

            Destroy(gameObject);
        }
    }
}
