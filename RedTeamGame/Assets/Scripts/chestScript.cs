using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestScript : MonoBehaviour
{
    //[SerializeField] Renderer model;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip openSound;
    //[SerializeField] AudioClip damagedSound;

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
                aud.PlayOneShot(openSound);
                Vector3 lootSpawnMod = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                GetComponent<LootBag>().instantiateLoot(lootSpawnMod);
                hasEnteredTrigger = false;
                gameManager.instance.togglePopUpTxt();
                Destroy(gameObject, 1);
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
