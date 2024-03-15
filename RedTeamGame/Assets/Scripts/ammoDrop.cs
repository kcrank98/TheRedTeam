using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoDrop : MonoBehaviour
{
    [SerializeField] int ammoAmount;
    bool hasEnteredTrigger;


    // Start is called before the first frame update
    void Start()
    {
        hasEnteredTrigger = false;
        //bullet = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //aud.PlayOneShot(addAmmo, addAmmoVol);
                gameManager.instance.playerScript.ammoCountUpdate(ammoAmount);
                hasEnteredTrigger = false;
                //bullet.enabled = false;
                Destroy(gameObject/*, 3*/);
                //useText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //useText.SetActive(true);
            hasEnteredTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //useText.SetActive(false);
            hasEnteredTrigger = false;
        }
    }
}
