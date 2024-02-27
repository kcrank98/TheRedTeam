using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class floorManager : MonoBehaviour
{
    public static floorManager instance;

    //the floor doors
    [SerializeField] GameObject startDoor;
    [SerializeField] GameObject exitDoor;
    [SerializeField] doorControl startDoorController;
    [SerializeField] doorControl exitDoorController;
    //public varables
    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        //get the floors door controlers
        startDoorController = startDoor.GetComponent<doorControl>();
        exitDoorController = exitDoor.GetComponent<doorControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount <= 0)//if all enemys are dead
        {
            //open the exit door
            exitDoorController.open();
        }
        //else there are still enemys that spawned
        else//close the exit and open the start
        {
            exitDoorController.close();
            startDoorController.open();
        }
      
    }
    public void updateEnemyCount(int count)
    {
        enemyCount += count; //when an enemy is added to the floor, the floor manager needs to be told
        
    }
}
