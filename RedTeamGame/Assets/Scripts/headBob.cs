using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headBob : MonoBehaviour
{
    public playerController playerScript;


    [SerializeField] Animator animator;
    public bool isWalking;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
      checkForHeadbob();
       animator.SetBool("isWalking", isWalking);
    }

    private void checkForHeadbob()
    {
        if (playerScript.playerVel.magnitude > .1f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
}
