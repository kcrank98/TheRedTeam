using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class doorControl : MonoBehaviour
{
    //door parts
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip openingSound;
    [SerializeField] float openingSoundVol;
    private Animator anim;
    public bool isOpen;

    private void Start()
    {

        anim = GetComponent<Animator>();
    }
    //public varables
    public void toggleDoor()
    {
        if (aud != null)
        {
            aud.time = 2f;
            aud.PlayOneShot(openingSound, openingSoundVol);
            Destroy(aud, 5);
        }
        anim.SetTrigger("toggleDoor");
        isOpen = !isOpen;
    }


  
    
}
