//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Collider capsuleCollider;
    [SerializeField] float originalFOV;
    [SerializeField] AudioSource aud;
    [SerializeField] GameObject muzzlePos;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("---- Health")]
    [Range(0, 60)][SerializeField] int HP;
    [SerializeField] int HPOrig;
    [Range(.1f, .99f)][SerializeField] float HPPerc;

    [Header("---- Shield")]
    [SerializeField] int shieldAmountOrg;
    [Range(0, 30)][SerializeField] int shieldAmount;

    [Header("---- Money")]
    [SerializeField] int coins;
    [SerializeField] int coinsOrig;

    [Header("---- Movement")]
    [Range(3, 25)][SerializeField] float playerSpeed;
    [Range(3, 25)][SerializeField] float playerSpeedOrig;
    [Range(1.0f, 5)][SerializeField] float sprintMod;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(5, 30)][SerializeField] float jumpForce;
    [Range(-10, -30)][SerializeField] float gravity;

    [Header("---- Crouch")]
    [Range(.1f, 1f)][SerializeField] float crouchSpeedMod;
    [SerializeField] int controllerHeightOrig;
    [SerializeField] float controllerYOrig;
    [SerializeField] int controllerCrouchHeight;
    [SerializeField] float controllerCrouchY;

    [Header("---- Dash")]
    [Range(1, 3)][SerializeField] int dashMax;
    [Range(5, 30)][SerializeField] float dashForce;
    [SerializeField] int dashCount;

    [Header("---- Gun Stats")]
    [SerializeField] string gunName;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] float aimFOV;
    [SerializeField] float aimSpeed;
    [SerializeField] Vector3 attachmentPosition;

    [Header("---- Gun")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] GameObject gunModel;
    //[SerializeField] GameObject gunAttachment;
    public AudioClip reloadSound;
    public AudioClip clickSound;
    public AudioClip shootSound;

    [Header("---- Grenade")]
    [SerializeField] GameObject grenadeModel;
    [SerializeField] int grenadeMax;
    [SerializeField] int grenadeCount;
    [SerializeField] bool isThrowingGrenadee;

    [Header("---- Audio")]
    [SerializeField] AudioClip[] playerSteps;
    [Range(0, 1)][SerializeField] float playerStepsVol;

    [SerializeField] AudioClip[] soundHurt;
    [Range(0, 1)][SerializeField] float soundHurtVol;

    [SerializeField] AudioClip[] jumpSound;
    [Range(0, 1)][SerializeField] float jumpSoundVol;

    [SerializeField] AudioClip[] dashSound;
    [Range(0, 1)][SerializeField] float dashSoundVol;

    Vector3 move;
    Vector3 playerVel;
    bool isShooting;
    bool isSprinting;
    bool isCrouched;
    int selectedGun;
    bool aimedIn;
    int jumpCount;
    bool isPlayingSteps;

   

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        shieldAmountOrg = shieldAmount;
        coinsOrig = coins;
        playerSpeedOrig = playerSpeed;
        originalFOV = mainCamera.GetComponent<Camera>().fieldOfView;
        respawn();
    }

    // Update is called once per frame
    void Update()
    {
        sprint();
        crouch();
        dash();

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
                //else if (Input.GetButton("Shoot") && !gameManager.instance.updateBullet())
                //{

                //    aud.PlayOneShot(clickSound, gunList[selectedGun].clickSoundVol);

                //}
            }
        }
    }

    private void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            dashCount = 0;
            playerVel = Vector3.zero;
        }

        move = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpForce;
            StartCoroutine(playJumpSound());
            jumpCount++;
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if (controller.isGrounded && move.normalized.magnitude > 0.3f && !isPlayingSteps)
        {
            StartCoroutine(playFootsteps());
        }
    }
    void crouch()
    {
        if (Input.GetButtonDown("Crouch") && controller.isGrounded && !isSprinting)
        {
            controller.height = controllerCrouchHeight;
            mainCamera.GetComponent<Camera>().transform.localPosition = mainCamera.GetComponent<Camera>().transform.localPosition + new Vector3(0, -.7f, 0);
            playerSpeed *= crouchSpeedMod;

        }
        else if (Input.GetButtonUp("Crouch"))
        {
            controller.height = controllerHeightOrig;
            mainCamera.GetComponent<Camera>().transform.localPosition = new Vector3(0, 1, 0);
            playerSpeed = playerSpeedOrig;
        }
    }
    void dash()
    {
        if (!controller.isGrounded && Input.GetButtonDown("Dash") && dashCount < dashMax)
        {
            playerVel += transform.forward * dashForce;
            StartCoroutine(playDashSound());
            dashCount++;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && jumpCount < 1)
        {
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed = playerSpeedOrig;
            isSprinting = false;
        }
    }
    IEnumerator playFootsteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(playerSteps[Random.Range(0, playerSteps.Length)], playerStepsVol);

        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }

        isPlayingSteps = false;
    }
    IEnumerator playJumpSound()
    {
        aud.PlayOneShot(jumpSound[Random.Range(0, jumpSound.Length)], jumpSoundVol);
        yield return new WaitForSeconds(0.01f);
    }
    IEnumerator playDashSound()
    {
        aud.PlayOneShot(dashSound[Random.Range(0, dashSound.Length)], dashSoundVol);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        if(gameManager.instance.updateBullet())
        {
            aud.PlayOneShot(gunList[selectedGun].shootSound);
            StartCoroutine(showMuzzleFlash());
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
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;

    }
    private IEnumerator showMuzzleFlash()
    {
        muzzleFlash.Play();
        yield return new WaitForSeconds(gunList[selectedGun].shootRate);
        muzzleFlash.Stop();
    }

    public void takeDamage(int amount)
    {
        aud.PlayOneShot(soundHurt[Random.Range(0, soundHurt.Length)], soundHurtVol);

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
        coinsOrig = coins;
        playerSpeedOrig = playerSpeed;
        originalFOV = mainCamera.GetComponent<Camera>().fieldOfView;

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

    public void updateCoins(int amount)
    {
        coins += amount;
    }

    public void getGunStats(gunStats gun)
    {
       
        gunList.Add(gun);

        gunName = gun.gunName;
        shootDamage = gun.shootDamage;
        shootDistance = gun.shootDistance;
        shootRate = gun.shootRate;
        aimFOV = gun.aimFOV;
        aimSpeed = gun.aimSpeed;
        reloadSound = gun.reloadSound;
        clickSound = gun.clickSound;
        shootSound = gun.shootSound;

        attachmentPosition = gun.attachmentPosition;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        //gunAttachment.GetComponent<MeshFilter>().sharedMesh = gun.attachment.GetComponent<MeshFilter>().sharedMesh;
        //gunAttachment.GetComponent<MeshRenderer>().sharedMaterial = gun.attachment.GetComponent<MeshRenderer>().sharedMaterial;

       // gunAttachment.GetComponent <Transform>().position = attachmentPosition;
        selectedGun = gunList.Count - 1;
    }
    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1 && !aimedIn)
        {
            gameManager.instance.setActiveGun(gameManager.instance.guns[selectedGun]);
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0 && !aimedIn)
        {
            gameManager.instance.setActiveGun(gameManager.instance.guns[selectedGun]);
            selectedGun--;
            changeGun();
        }
    }
    void changeGun()
    {

        gunName = gunList[selectedGun].gunName;
        shootDamage = gunList[selectedGun].shootDamage;
        shootDistance = gunList[selectedGun].shootDistance;
        shootRate = gunList[selectedGun].shootRate;
        aimFOV = gunList[selectedGun].aimFOV;
        aimSpeed = gunList[selectedGun].aimSpeed;
        reloadSound = gunList[selectedGun].reloadSound;
        clickSound = gunList[selectedGun].clickSound;
        shootSound = gunList[selectedGun].shootSound;

        attachmentPosition = gunList[selectedGun].attachmentPosition;



        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        //gunAttachment.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].attachment.GetComponent<MeshFilter>().sharedMesh;
        //gunAttachment.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].attachment.GetComponent<MeshRenderer>().sharedMaterial;

        //gunAttachment.GetComponent<Transform>().position = gunList[selectedGun].attachmentPosition;

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

   

}
