using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossRoomController : MonoBehaviour
{
    [SerializeField] doorControl door;
    public bool playerInFight = false;
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){

            if (!playerInFight)
            {
                door.toggleDoor();
                playerInFight=true;
            }
        }
    }
    

}
