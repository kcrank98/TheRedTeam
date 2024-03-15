using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [SerializeField] Animator anim;
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
                gameManager.instance.playerScript.hasKey = false;
                anim.SetTrigger("HasKey");
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
