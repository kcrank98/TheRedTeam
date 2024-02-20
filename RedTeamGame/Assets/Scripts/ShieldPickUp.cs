using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    public GameObject useText;
    public static bool hasPickedUpShieldPack = false;
    private bool hasEnteredTrigger = false;
    public gameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<gameManager>(); // Assuming gameManager is a singleton, use FindObjectOfType instead of GetComponent
    }

    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                hasPickedUpShieldPack = true;
                gameManager.instance.playerScript.updateShield(25); // Assuming updateShield is a method in playerScript to update shield
                hasEnteredTrigger = false;
                gameObject.SetActive(false);
                useText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider is the player
        {
            useText.SetActive(true);
            hasEnteredTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider is the player
        {
            useText.SetActive(false);
            hasPickedUpShieldPack = false;
            hasEnteredTrigger = false;
        }
    }
}