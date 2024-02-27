using Unity.VisualScripting;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public GameObject useText;
    public static bool hasPickedUpHealthPack = false;
    private bool hasEnteredTrigger = false;
    public gameManager gameManager;
    //public GameObject player;


    private void Start()
    {
        gameManager = GetComponent<gameManager>();
    }


    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                hasPickedUpHealthPack = true;
                gameManager.instance.playerScript.updateHealth(10);
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
        hasPickedUpHealthPack = false;
        hasEnteredTrigger = false;
    }
}