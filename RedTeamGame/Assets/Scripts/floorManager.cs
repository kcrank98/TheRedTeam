using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class floorManager : MonoBehaviour
{

    //the floor doors
    [SerializeField] GameObject startDoor;
    [SerializeField] GameObject exitDoor;
    [SerializeField] doorControl startDoorController;
    [SerializeField] doorControl exitDoorController;
    [SerializeField] List<spawner> spawners;

    //public varables
    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        //get the floors door controlers
        startDoorController = startDoor.GetComponent<doorControl>();
        exitDoorController = exitDoor.GetComponent<doorControl>();
        foreach (spawner spawner in spawners)
        {
            enemyCount = enemyCount + spawner.totalEnemy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount <= 0)//if all enemys are dead
        {
            //open the exit door
            if(exitDoor != null)
            {
                exitDoorController.open();
            }
        }
        //else there are still enemys that spawned
        else//close the exit and open the start
        {
            if(exitDoor != null)
            {
                exitDoorController.close();
            }
            if (startDoor != null)
            {
                startDoorController.open();
            }
        }
      
    }
}
