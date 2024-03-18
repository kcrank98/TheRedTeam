using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
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
                GetComponent<LootBag>().instantiateLoot(transform.position);
                hasEnteredTrigger = false;
                gameManager.instance.togglePopUpTxt();
                Destroy(gameObject);
                //useText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.togglePopUpTxt("Press 'E' to interact");
            hasEnteredTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.togglePopUpTxt();
            hasEnteredTrigger = false;
        }
    }
}
