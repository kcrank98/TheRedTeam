using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyScript : MonoBehaviour
{
    bool hasEnteredTrigger;

    // Start is called before the first frame update
    void Start()
    {
        hasEnteredTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                hasEnteredTrigger = false;
                gameManager.instance.playerScript.hasKey = true;
                Destroy(gameObject);
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
