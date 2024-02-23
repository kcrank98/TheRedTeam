using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Collider weaponCollider;
    [SerializeField] Transform headPos;
    [SerializeField] bool Shooter;
    [SerializeField] bool Melee;

    [SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int shootCone;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;
    [SerializeField] Image HPBar;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] float meleeRate;

    bool isAttacking;
    bool isShooting;
    bool isMelee;
    bool playerInRange;
    float angleToPlayer;
    Vector3 playerDir;
    int HPOrig;
    Vector3 startingPos;
    bool destinChosen;
    float stoppingDistanceOrig;



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
                        StartCoroutine(melee());
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
        isShooting = true;

        anim.SetTrigger("Shoot");


        yield return new WaitForSeconds(shootRate);
        isAttacking = false;
        isShooting = false;
    }

    IEnumerator melee()
    {
        isAttacking = true;
        isMelee = true;

        anim.SetTrigger("Melee");

        yield return new WaitForSeconds(meleeRate);
        isAttacking = false;
        isMelee = false;
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
