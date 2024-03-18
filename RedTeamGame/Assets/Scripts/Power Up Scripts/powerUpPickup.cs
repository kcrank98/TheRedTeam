using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpPickup : MonoBehaviour
{
    [Range(3, 25)][SerializeField] float playerSpeedBoost;
    [Range(1, 25)][SerializeField] float playerSpeedBoostTimer;
    public bool hasEnteredTrigger = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hasEnteredTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                gameManager.instance.playerScript.updateSpeed(playerSpeedBoost);
                hasEnteredTrigger = false;
                this.enabled = false;
                Destroy(gameObject, 3);
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
    IEnumerator applySpeedBoost()
    {
        // Reset player speed to normal
        gameManager.instance.playerScript.updateSpeed(playerSpeedBoost);

        yield return new WaitForSeconds(playerSpeedBoostTimer);

    }
    IEnumerator resetSpeedBoost()
    {
        // Reset player speed to normal
        gameManager.instance.playerScript.updateSpeed(playerSpeedBoost * (-1));
        yield return new WaitForSeconds(.1f);

    }

   

}
