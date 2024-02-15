using System.Collections;
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

    [Header("Shooting")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("HP")]
    [SerializeField] float HPPerc;
    [SerializeField] int HP;
    [SerializeField] int HPOrig;
    [SerializeField] int shieldAmount;

    [HideInInspector][SerializeField] Transform shootPos;
    [HideInInspector][SerializeField] GameObject bullet;

    Vector3 move;
    Vector3 playerVel;
    bool isShooting;

    void Start()
    {
        HPOrig = HP;
        respawn();
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();

            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(shoot());
            }

            if (HealthPack.hasPickedUpHealthPack)
            {
                HealPlayer(20);
                HealthPack.hasPickedUpHealthPack = false; // Reset the flag
            }
        }
    }

    void movement()
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

    void HealPlayer(int amount)
    {
        HP += amount;
        if (HP > HPOrig)
            HP = HPOrig;
    }
}