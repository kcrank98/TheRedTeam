using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class doorControl : MonoBehaviour
{
    //door parts
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;
    [SerializeField] GameObject leftDoorSubPanel;
    [SerializeField] GameObject rightDoorSubPanel;
    //door move locations
    [SerializeField] GameObject leftDoorMoveEnd;
    [SerializeField] GameObject rightDoorMoveEnd;
    
    //public varables
    public bool isOpen;
    public int doorSpeed;
    //start postitions
    Vector3 leftDoorStart;
    Vector3 rightDoorStart;
    Vector3 leftSubStart;
    Vector3 rightSubStart;
    public int subPanelSpeed;
    //object reference transforms
    Transform leftDoorTransform;
    Transform rightDoorTransform;
    Transform leftSubTransform;
    Transform rightSubTransform;
    void Start()
    {
        //left door parts start transforms and postitions
        leftDoorStart = leftDoor.transform.position;
        leftSubStart = leftDoorSubPanel.transform.position;
        leftDoorTransform = leftDoor.transform;
        leftSubTransform = leftDoorSubPanel.transform;
        //right door parts start transforms
        rightDoorStart = rightDoor.transform.position;
        rightSubStart = rightDoorSubPanel.transform.position;
        rightDoorTransform = rightDoor.transform;
        rightSubTransform = rightDoorSubPanel.transform;
        //set door panel speed
        if(subPanelSpeed == 0)
        {
            subPanelSpeed = doorSpeed * 2;
        }
    }

    //open door and close door
    public void move(Vector3 leftMoveLocation, Vector3 rightMoveLocation)
    {
        //distance from end point of left door 
        float Ldistance = Vector3.Distance(leftDoor.transform.position, leftMoveLocation);
        float LSdistance = Vector3.Distance(leftDoorSubPanel.transform.position, leftMoveLocation);
        //right door distance erelivant, object is mirrored so distance is the same

        //checks if the door has reached its destination or not for either panel, if one but not the other has than it will continue
        if (LSdistance > .1f)
        {
            leftDoorTransform.position = (Vector3.MoveTowards(leftDoor.transform.position, leftMoveLocation, doorSpeed * Time.deltaTime));
            leftSubTransform.position = (Vector3.MoveTowards(leftDoorSubPanel.transform.position, leftMoveLocation, subPanelSpeed * Time.deltaTime));
            rightDoorTransform.position = (Vector3.MoveTowards(rightDoor.transform.position, rightMoveLocation, doorSpeed * Time.deltaTime));
            rightSubTransform.position = (Vector3.MoveTowards(rightDoorSubPanel.transform.position, rightMoveLocation, subPanelSpeed * Time.deltaTime));
        }
        else
        {
            Debug.Log("made it");
        }
    }
    //open door
    public void open()
    {
        move(leftDoorMoveEnd.transform.position, rightDoorMoveEnd.transform.position);
        isOpen = true;
    }
    //close door
    public void close()
    {
        move(leftDoorStart, rightDoorStart);
        isOpen = false;
    }


  
    
}
