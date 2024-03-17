using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalScript : MonoBehaviour
{
    Vector3 newPos;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        newPos = new Vector3(-6f, 23f, 93f);

        count = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && count == 0)
        {
            gameManager.instance.playerScript.controller.enabled = false;
            gameManager.instance.player.transform.position = newPos;
            gameManager.instance.playerSpawnPos.transform.position = newPos;
            gameManager.instance.playerScript.controller.enabled = true;

            count++;
        }
    }
}
