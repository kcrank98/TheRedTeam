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
    [SerializeField] Animator anim;
    //public varables
    public bool isOpen;
    void Start()
    {
       
    }

    //open door and close door
   
    //open door
    public void open()
    {

        anim.SetTrigger("open");
        isOpen = true;
    }
    //close door
    public void close()
    {
        anim.SetTrigger("close");
        isOpen = false;
    }


  
    
}
