using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject mainCamera;
    [SerializeField] float originalFOV;

    [Header("---- Health")]
    [Range(0, 50)][SerializeField] int HP;
    [SerializeField] int HPOrig;
    [Range(.1f, .99f)][SerializeField] float HPPerc;

    [Header("---- Shield")]
    [SerializeField] int shieldAmountOrg;
    [Range(0, 50)][SerializeField] int shieldAmount;

    [Header("---- Movement")]
    [Range(3, 25)][SerializeField] float playerSpeed;
    [Range(1.0f, 5)][SerializeField] float sprintMod;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(5, 30)][SerializeField] float jumpForce;
    [Range(-10, -30)][SerializeField] float gravity;

    int jumpCount;

    [Header("---- Gun Stats")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] float aimFOV;
    [SerializeField] float aimSpeed;

    [Header("---- Gun")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] GameObject gunModel;

    Vector3 move;
    Vector3 playerVel;
    bool isShooting;
    bool isSprinting;
    int selectedGun;
    bool aimedIn;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        shieldAmountOrg = shieldAmount;
        originalFOV = mainCamera.GetComponent<Camera>().fieldOfView;
        respawn();
    }

    // Update is called once per frame
    void Update()
    {
        sprint();

        if (!gameManager.instance.isPaused)
        {
            movement();


            if (gunList.Count > 0)
            {
                selectGun();
                aim();

                if (Input.GetButton("Shoot") && !isShooting)
                {
                    StartCoroutine(shoot());
                }
            }
        }
    }
    private void aim()
    {
        if (Input.GetButtonDown("Aim"))
        {      
            aimedIn = true;
            mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, aimFOV, aimSpeed);
        }
        else if (Input.GetButtonUp("Aim"))
        {
            mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, originalFOV, aimSpeed);
            aimedIn = false;
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
        if (shieldAmount > 0)
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


    public void updateShield(int amount)
    {
        shieldAmount += amount;

    }
    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);

        shootDamage = gun.shootDamage;
        shootDistance = gun.shootDistance;
        shootRate = gun.shootRate;
        aimFOV = gun.aimFOV;
        aimSpeed = gun.aimSpeed;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
    }
    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1 && !aimedIn)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0 && !aimedIn)
        {
            selectedGun--;
            changeGun();
        }
    }
    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootDistance = gunList[selectedGun].shootDistance;
        shootRate = gunList[selectedGun].shootRate;
        aimFOV = gunList[selectedGun].aimFOV;
        aimSpeed = gunList[selectedGun].aimSpeed;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

}
