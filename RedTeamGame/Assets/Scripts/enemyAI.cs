using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Collider weaponCollider;
    [SerializeField] Transform headPos;
    [SerializeField] Image HPBar;

    [Header("-----Enemy Type-----")]
    [SerializeField] bool Shooter;
    [SerializeField] bool Melee;

    [Header("-----Enemy Parameters-----")]
    [Range(1, 250)] [SerializeField] int HP;
    [Range(1, 90)] [SerializeField] int viewCone;
    [Range(1, 10)] [SerializeField] int shootCone;
    [Range(1, 50)] [SerializeField] int targetFaceSpeed;
    [Range(1, 50)] [SerializeField] int animSpeedTrans;
    [Range(1, 10)] [SerializeField] int roamPauseTime;
    [Range(1, 50)] [SerializeField] int roamDistance;

    [Header("-----Score Parameters-----")]
    [Range(0, 5000)][SerializeField] int scoreValue;
    [Range(0, 5000)][SerializeField] int moneyValue;

    [Header("-----Enemy Attack Stats-----")]
    [SerializeField] GameObject bullet;
    [Range(0, 1)] [SerializeField] float shootRate;
    [Range(0, 1)] [SerializeField] float meleeRate;
    [Range(0, 10)] [SerializeField] float meleeRange;

    [Header("-----Audio-----")]
    [SerializeField] AudioClip[] enemySteps;
    [Range(0, 1)][SerializeField] float enemyStepsVol;
    [SerializeField] AudioClip[] soundHurt;
    [Range(0, 1)][SerializeField] float soundHurtVol;

    bool isAttacking;
    bool playerInRange;
    float angleToPlayer;
    Vector3 playerDir;
    int HPOrig;
    Vector3 startingPos;
    bool destinChosen;
    float stoppingDistanceOrig;
    //bool isPlayingSteps;



    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        HPOrig = HP;
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
    }

    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange && !canSeePlayer())
        {
            //roam
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            //roam
            StartCoroutine(roam());
        }
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinChosen)
        {
            destinChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randPos = Random.insideUnitSphere * roamDistance;
            randPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randPos, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            destinChosen = false;
        }
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;

        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isAttacking && angleToPlayer <= shootCone)
                {
                    if(Shooter)
                        StartCoroutine(shoot());

                    if (Melee)
                    {
                        if(agent.remainingDistance <= meleeRange)
                            StartCoroutine(melee());
                    }
                }

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

                agent.stoppingDistance = stoppingDistanceOrig;

                return true;
            }
        }

        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }


    public void takeDamage(int amount)
    {
        anim.SetTrigger("Damaged");

        weaponColliderOff();

        agent.SetDestination(gameManager.instance.player.transform.position);

        HP -= amount;
        updateUI();

        StartCoroutine(flashMat());

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isAttacking = true;
        //isShooting = true;

        anim.SetTrigger("Shoot");


        yield return new WaitForSeconds(shootRate);
        isAttacking = false;
        //isShooting = false;
    }

    IEnumerator melee()
    {
        isAttacking = true;
        //isMelee = true;

        anim.SetTrigger("Melee");

        yield return new WaitForSeconds(meleeRate);
        isAttacking = false;
        //isMelee = false;
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void weaponColliderOn()
    {
        weaponCollider.GetComponent<Collider>().enabled = true;
    }

    public void weaponColliderOff()
    {
        weaponCollider.GetComponent<Collider>().enabled = false;
    }

    void updateUI()
    {
        HPBar.fillAmount = (float)HP / (float)HPOrig;
    }
}
