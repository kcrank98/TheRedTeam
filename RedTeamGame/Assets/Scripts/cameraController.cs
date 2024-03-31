using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    [SerializeField] Animator animator;
    public bool isWalking;
    public float playerVelCheck;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isWalking = false;
        sensitivity = dataManager.instance.GetOption("sensitivity");
        if (sensitivity == 0)
            sensitivity = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
        // get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        // invert look
        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        // clamp the rotation on the x-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        // rotate the cam on the x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // rotate the player on the y-axis
        transform.parent.Rotate(Vector3.up * mouseX);

        checkForWalking();
        animator.SetBool("isWalking", isWalking);
        playerVelCheck = gameManager.instance.playerScript.move.magnitude;

    }
    
    void checkForWalking()
    {
        if (gameManager.instance.playerScript.move.magnitude > .1f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
}
