
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public GameObject useText;
    public bool hasEnteredTrigger = false;

    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerController player = FindObjectOfType<playerController>();
                if (player != null)
                {
                    player.RestoreHealth(20); // Heal the player by 20 HP
                }

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
        hasEnteredTrigger = false;
    }
}
