using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage, IPushBack
{
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Collider capsuleCollider;
    [SerializeField] float originalFOV;
    [SerializeField] AudioSource aud;
    [SerializeField] GameObject muzzlePos;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject muzzleFlashGO;
    [SerializeField] Animator anim;
    [SerializeField] Animator gunAnimator;
    [SerializeField] RuntimeAnimatorController runtimeAnimatorController;
    [SerializeField] bool DeathDown;
    public bool hasKey;
    public bool isAlive = true;


    [Header("---- Health")]
    [Range(0, 60)][SerializeField] int HP;
    [SerializeField] int HPOrig;
    [Range(.1f, .99f)][SerializeField] float HPPerc;

    [Header("---- Shield")]
    [SerializeField] public int shieldAmountOrg;
    [Range(0, 30)][SerializeField] public int shieldAmount;

    [Header("---- Money")]
    [SerializeField] int coins;
    [SerializeField] int coinsOrig;

    [Header("---- Movement")]
    [Range(3, 25)][SerializeField] float playerSpeed;
    [Range(3, 25)][SerializeField] float playerSpeedOrig;

    [Range(-10, -30)][SerializeField] float gravity;
    [Range(-10, -30)][SerializeField] float gravityOrig;
    [Range(0, 25)][SerializeField] int pushBackResolve;
    [SerializeField] int playerPositionY;

    [Header("---- Dash")]
    [Range(1, 3)][SerializeField] int dashMax;
    [Range(1, 500)][SerializeField] float dashForce;
    [Range(.1f, 5)][SerializeField] float dashDuration;
    [Range(3, 25)][SerializeField] float maxDashSpeed;
    [Range(1, 5)][SerializeField] float dashCooldown;
    [Range(1, 5)][SerializeField] float dashDeceleration;

    [SerializeField] int dashCount;
    [SerializeField] bool isDashing;

    [Header("---- Gun Stats")]

    [SerializeField] string gunName;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] float aimFOV;
    [SerializeField] float aimSpeed;
    [SerializeField] public int magazine;
    [SerializeField] int magazineMax;
    [SerializeField] public int reserves;
    [SerializeField] int reservesMax;


    [Header("---- Gun")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] List<GameObject> gunGameObjectList = new List<GameObject>();
    [SerializeField] GameObject gunModel;
    public Transform gunparentObject;
    public int selectedGunGameObject;

    [Header("---- Gun Audio")]

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

    [SerializeField] AudioClip[] shieldBreakSound;
    [Range(0, 1)][SerializeField] float shieldBreakVol;

    [SerializeField] AudioClip[] jumpSound;
    [Range(0, 1)][SerializeField] float jumpSoundVol;

    [SerializeField] AudioClip[] dashSound;
    [Range(0, 1)][SerializeField] float dashSoundVol;

    [SerializeField] AudioClip[] gunPickupSound;
    [Range(0, 1)][SerializeField] float gunPickupVol;

    Vector3 move;
    Vector3 playerVel;
    Vector3 pushBack;
    Vector3 dashDirection;
    bool isShooting;
    bool isSprinting;
    bool isCrouched;
    int selectedGun;
    //int selectedGunGameObject;
    bool aimedIn;
    int jumpCount;
    bool isPlayingSteps;

    // Start is called before the first frame update
    void Start()
    {
        //hasKey = false;
        HPOrig = HP;
        shieldAmountOrg = shieldAmount;
        coinsOrig = coins;
        playerSpeedOrig = playerSpeed;
        gravityOrig = gravity;
        originalFOV = mainCamera.GetComponent<Camera>().fieldOfView;
        gameManager.instance.UpdateShiieldUi();
        hasKey = false;
        respawn();
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameManager.instance.isPaused && isAlive)
        {
            movement();


            if (gunList.Count > 0)
            {
                selectGun();
                aim();

                if (Input.GetButton("Shoot") && !isShooting)
                {
                    if (magazine > 0)
                    {
                        StartCoroutine(shoot());
                    }
                    else if(magazine <=0) 
                    {
                        aud.PlayOneShot(clickSound);

                    }
                }
            }
        }
    }

    private void movement()
    {
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);

        if (controller.isGrounded)
        {
            playerVel = Vector3.zero;
        }

        move = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);


        if (Input.GetButtonDown("Dash") && dashCount < dashMax && !isDashing)
        {
            StartCoroutine(DASH());
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move((playerVel + pushBack) * Time.deltaTime);

        if (controller.isGrounded && move.normalized.magnitude > 0.3f && !isPlayingSteps)
        {
            StartCoroutine(playFootsteps());
        }
    }

    IEnumerator DASH()
    {
        isDashing = true;
        dashCount++;

        //gravity = 0;
        //playerVel += transform.forward * dashForce;

        Vector3 dashTarget = move.normalized * dashForce * Time.deltaTime;
        //controller.Move(move.normalized * dashForce * Time.deltaTime);

        controller.Move(Vector3.MoveTowards(move, dashTarget, dashDuration));

        StartCoroutine(playDashSound());
        dashCount++;

        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
        dashCount = 0;
        //gravity = gravityOrig;
    }

    public void pushBackDir(Vector3 dir)
    {
        pushBack += dir;
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
        magazine--;
        //gunList[selectedGun].magazine--;

        isShooting = true;

        aud.PlayOneShot(gunList[selectedGun].shootSound);
        StartCoroutine(showMuzzleFlash());

        //gunAnimator.SetTrigger("Shoot");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {

            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            GameObject muzzleFlashInstance = Instantiate(muzzleFlashGO, hit.point, Quaternion.identity);
            Destroy(muzzleFlashInstance, .05f);


            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

        else if (gunList[selectedGun].magazine == 0)
        {
            aud.PlayOneShot(gunList[selectedGun].clickSound);
        }
        gameManager.instance.updateAmmo();

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

            if (shieldAmount <= 0)
            {
                aud.PlayOneShot(shieldBreakSound[Random.Range(0, shieldBreakSound.Length)], shieldBreakVol);
            }
        }
        else
        {
            HP -= amount;

            StartCoroutine(flashDamage());
            checkHPBelowPerc();
        }

        if (HP <= 0)
        {
            StartCoroutine(playDeath());
            //gameManager.instance.youLose();
        }
        gameManager.instance.UpdateShiieldUi();

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
    IEnumerator playDeath()
    {
        if (gunGameObjectList.Count > 0)
        {
            gunGameObjectList[selectedGunGameObject].SetActive(false);
        }

        isAlive = false;
        controller.enabled = false;
        gameManager.instance.shieldDamage.SetActive(true);

        Vector3 startCamPos = mainCamera.GetComponent<Camera>().transform.position;
        Vector3 targetCamPosDown = new Vector3(startCamPos.x, startCamPos.y * 1.2f, startCamPos.z);
        Vector3 targetCamPosUp = new Vector3(startCamPos.x, startCamPos.y / 1.2f, startCamPos.z);
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (DeathDown)
            {
                mainCamera.GetComponent<Camera>().transform.position = Vector3.Lerp(startCamPos, targetCamPosDown, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if (!DeathDown)
            {
                mainCamera.GetComponent<Camera>().transform.position = Vector3.Lerp(startCamPos, targetCamPosUp, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitForSeconds(.1f);

        gameManager.instance.youLose();

    }
    public void respawn()
    {
        HP = HPOrig;
        shieldAmountOrg = shieldAmount;
        coinsOrig = coins;
        playerSpeedOrig = playerSpeed;
        gravityOrig = gravity;
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
        aud.PlayOneShot(gunPickupSound[Random.Range(0, gunPickupSound.Length)], gunPickupVol);

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
        magazine = gun.magazine;
        magazineMax = gun.magazineMax;
        reserves = gun.reserves;
        reservesMax = gun.reservesMax;

        GameObject gunGun = Instantiate(gun.model);
        gunGun.transform.SetParent(gunparentObject);
        gunGun.transform.localPosition = Vector3.zero;
        gunGun.transform.localRotation = Quaternion.identity;
        gunGun.transform.localScale = Vector3.one;
        if (gunGameObjectList.Count <= 0)
        {
            gunGameObjectList.Add(gunGun);
        }
        else if (gunGameObjectList.Count > 0)
        {
            gunGameObjectList[selectedGunGameObject].SetActive(false);
            gunGameObjectList.Add(gunGun);
        }

        selectedGun = gunList.Count - 1;
        selectedGunGameObject = gunList.Count - 1;
        gameManager.instance.setActiveGun();

    }
    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1 && !aimedIn)
        {
            gunGameObjectList[selectedGunGameObject].SetActive(false);
            selectedGunGameObject++;

            selectedGun++;
            changeGun();
            changeGunObject();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0 && !aimedIn)
        {
            gunGameObjectList[selectedGunGameObject].SetActive(false);
            selectedGunGameObject--;

            selectedGun--;
            changeGun();
            changeGunObject();
        }
    }
    void changeGun()
    {
        aud.PlayOneShot(gunPickupSound[Random.Range(0, gunPickupSound.Length)], gunPickupVol);

        gunName = gunList[selectedGun].gunName;
        shootDamage = gunList[selectedGun].shootDamage;
        shootDistance = gunList[selectedGun].shootDistance;
        shootRate = gunList[selectedGun].shootRate;
        aimFOV = gunList[selectedGun].aimFOV;
        aimSpeed = gunList[selectedGun].aimSpeed;
        reloadSound = gunList[selectedGun].reloadSound;
        clickSound = gunList[selectedGun].clickSound;
        shootSound = gunList[selectedGun].shootSound;
        magazine = gunList[selectedGun].magazine;
        magazineMax = gunList[selectedGun].magazineMax;
        reserves = gunList[selectedGun].reserves;
        reservesMax = gunList[selectedGun].reservesMax;

        gameManager.instance.setActiveGun();

    }
    void changeGunObject()
    {
        gunGameObjectList[selectedGunGameObject].SetActive(true);
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
    public void reloadGun()
    {
        if (gunList[selectedGun] != null)
        {
            int difFromMagMax = magazineMax - magazine;
            if (reserves - difFromMagMax >= 0)
            {
                magazine = magazineMax;
                reserves -= difFromMagMax;
                //gunList[selectedGun].reserves -= difFromMagMax;
            }
            else
            {
                magazine += reserves;
                reserves = 0;
                //gunList[selectedGun].reserves = 0;
            }
            aud.PlayOneShot(reloadSound);
            gameManager.instance.updateAmmo();
        }

    }
    public void ammoCountUpdate(int amount)
    {
        reserves += amount;
        gameManager.instance.updateAmmo();
    }
}