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
    [SerializeField] int jumpMax;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    int jumpCount;

    [Header("Shooting")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("HP")]
    [SerializeField] float HPPerc;
    [SerializeField] int HP;
    [SerializeField] int HPOrig;


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
        respawn();
    }

    // Update is called once per frame
    void Update()
    {
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
    private void sprint()
    {
        // add a sprint 
        // make player bob
    }
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
        HP -= amount;
        
        StartCoroutine(flashDamage());
        checkHPBelowPerc();

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

    IEnumerator flashDamage()
    {
        gameManager.instance.damageFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.damageFlash.gameObject.SetActive(false);

    }
    public void respawn()
    {
        HP = HPOrig;

        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void RestoreHealth(int amount)
    {
        HP += amount;
        HP = Mathf.Min(HP, HPOrig); // Ensure HP doesn't exceed max HP
    }

    private void CheckForHealthPackPickup()
    {
        HealthPack[] healthPacks = FindObjectsOfType<HealthPack>();
        foreach (HealthPack pack in healthPacks)
        {
            if (pack.hasEnteredTrigger)
            {
                RestoreHealth(20); // Heal the player by 20 HP
                pack.gameObject.SetActive(false);
                pack.useText.SetActive(false);
                break;
            }
        }
    }
}
