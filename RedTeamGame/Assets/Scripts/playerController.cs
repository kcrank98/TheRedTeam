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
    [SerializeField] float origanalPlayerSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    
    int jumpCount;
    // sprint attempt
    [SerializeField] float sprintSpeed;
    [SerializeField] float sprintDuration;
    [SerializeField] float sprintRemaining;
    //private bool isSprinting = false;

    [Header("Shooting")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("HP")]
    [SerializeField] float HPPerc;
    [SerializeField] int HP;
    [SerializeField] int HPOrig;
    [SerializeField] int shieldAmountOrg;
    [SerializeField] int shieldAmount;


    [HideInInspector][SerializeField] gun currentGun;
    [HideInInspector][SerializeField] int currentGunDamage;

    [HideInInspector][SerializeField] Transform shootPos;
    [HideInInspector][SerializeField] GameObject bullet;

    // test code
    //public static Action shootInput;

    Vector3 move;
    Vector3 playerVel;
    bool isShooting;

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
        if (!gameManager.instance.isPaused)
        {
            movement();

            //if (Input.GetButtonDown("Sprint") && sprintRemaining > 0f)
            //{
            //    sprint();
            //}

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
    //private void sprint()
    //{
    //    // add a sprint 
    //    // make player bob
       
    //    //if (Input.GetKeyDown(KeyCode.LeftShift) && sprintRemaining > 0f)
    //    //if (Input.GetButtonDown("Sprint") && sprintRemaining > 0f)
    //    {
    //        isSprinting = true;
    //        playerSpeed = sprintSpeed;
    //        sprintRemaining -= Time.deltaTime;
    //        if (sprintRemaining <= 0f)
    //        {
    //            isSprinting = false;
    //            sprintRemaining = sprintDuration;
    //        }
    //    }
    //    playerSpeed = origanalPlayerSpeed;
    //}
    IEnumerator shoot()
    {
        isShooting = true;
        muzzleFlash.Play();
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
        muzzleFlash.Stop();
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
}
