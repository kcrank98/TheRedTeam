using Unity.VisualScripting;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] int restoreHealthAmount;
    //public GameObject useText;
    bool hasEnteredTrigger = false;
    //public GameObject player;


    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameManager.instance.playerScript.updateHealth(restoreHealthAmount);
                hasEnteredTrigger = false;
                gameObject.SetActive(false);
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