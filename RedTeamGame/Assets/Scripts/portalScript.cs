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
        newPos = new Vector3(-8f, 4f, 0.1f);

        count = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && count == 0)
        {
            gameManager.instance.player.transform.position = newPos;
            gameManager.instance.playerSpawnPos.transform.position = newPos;

            count++;
        }
    }
}
