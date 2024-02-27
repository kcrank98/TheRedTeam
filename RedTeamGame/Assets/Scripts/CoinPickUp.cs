using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public GameObject useText;
    public static bool hasPickedUpCoin = false;
    private bool hasEnteredTrigger = false;
    public gameManager gameManager;

   

    private void Start()
    {
        gameManager = FindObjectOfType<gameManager>();
    }

    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                hasPickedUpCoin = true;
                gameManager.instance.playerScript.updateCoins(10);
                hasEnteredTrigger = false;
                gameObject.SetActive(false);
                useText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        useText.SetActive(true);
        hasEnteredTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        useText.SetActive(false);
        hasPickedUpCoin = false;
        hasEnteredTrigger = false;
    }
}
