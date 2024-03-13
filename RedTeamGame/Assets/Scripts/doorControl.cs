using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class doorControl : MonoBehaviour
{
    //door parts
    [SerializeField] Collider trigger;
    [SerializeField] Animator anim;
  
    //public varables
    public bool isOpen;
    public void toggleDoor()
    {
        anim.SetTrigger("toggleDoor");
        isOpen = !isOpen;
    }


  
    
}
