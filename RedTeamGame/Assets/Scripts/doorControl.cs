using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class doorControl : MonoBehaviour
{
    //door parts
    private Animator anim;
    public bool isOpen;

    private void Start()
    {

        anim = GetComponent<Animator>();
    }
    //public varables
    public void toggleDoor()
    {
        anim.SetTrigger("toggleDoor");
        isOpen = !isOpen;
    }


  
    
}
