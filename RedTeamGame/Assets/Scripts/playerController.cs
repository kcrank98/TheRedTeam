using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;

    [Header("Movement")]
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    
    int jumpCount;

    [Header("Shooting")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;

    [Header("HP")]
    [SerializeField] float HPPerc;
    [SerializeField] int HP;
    [SerializeField] int HPOrig;
    [SerializeField] int shieldAmountOrg;
    [SerializeField] int shieldAmount;


    // test code
    //public static Action shootInput;

    Vector3 move;
    Vector3 playerVel;
    bool isShooting;
    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        shieldAmountOrg = shieldAmount;
        respawn();
    }

    // Update is called once per frame
    void Update()
    {
        sprint();

        if (!gameManager.instance.isPaused)
        {
            movement();


            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(shoot());
            }
        }
    }
    private void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        move = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpForce;
            jumpCount++;
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
        }
    }
    IEnumerator shoot()
    {
        isShooting = true;
        
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);       
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    
    }

    public void takeDamage(int amount)
    {
        if(shieldAmount > 0)
        {
            StartCoroutine(flashShieldDamage());
            shieldAmount -= amount;
        }
        else
        {
            HP -= amount;

            StartCoroutine(flashDamage());
            checkHPBelowPerc();
        }
        
        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }


    void checkHPBelowPerc()
    {
        if (HP <= HPOrig * HPPerc)
        {
            gameManager.instance.damagePersist.gameObject.SetActive(true);
        }
        else
        {
            gameManager.instance.damagePersist.gameObject.SetActive(false);
        }
    }
    IEnumerator flashShieldDamage()
    {
        gameManager.instance.shieldDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.shieldDamage.SetActive(false);
    }

    IEnumerator flashDamage()
    {
        gameManager.instance.damageFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.damageFlash.gameObject.SetActive(false);

    }
    public void respawn()
    {
        HP = HPOrig;
        shieldAmountOrg = shieldAmount;

        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void updateHealth(int amount)
    {
        HP += amount;
    }
}
