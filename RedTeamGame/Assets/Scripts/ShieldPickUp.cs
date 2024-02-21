using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    public GameObject useText;
    public static bool hasPickedUpShieldPack = false;
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
                hasPickedUpShieldPack = true;
                gameManager.instance.playerScript.updateShield(25); 
                hasEnteredTrigger = false;
                gameObject.SetActive(false);
                useText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            useText.SetActive(true);
            hasEnteredTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            useText.SetActive(false);
            hasPickedUpShieldPack = false;
            hasEnteredTrigger = false;
        }
    }
}