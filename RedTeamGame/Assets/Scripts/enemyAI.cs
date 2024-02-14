using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] Image HPBar;
    [SerializeField] int viewCone;
    [SerializeField] int targetFaceSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float attackDistance;
    [SerializeField] float shootRate;

    bool isShooting;
    bool playerInRange;
    float angleToPlayer;
    Vector3 playerDir;
    int HPOrig;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;



    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        HPOrig = HP;
    }

    void Update()
    {
        if (playerInRange && canSeePlayer())
        {

        }
        else if(!playerInRange && !canSeePlayer())
        {
            patroling();
        }
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;

        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isShooting && (agent.remainingDistance <= attackDistance))
                {
                    if (!agent.pathPending)
                    {
                        StartCoroutine(shoot());
                    }
                }

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

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
        }
    }


    public void takeDamage(int amount)
    {
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
        Color ogColor = model.material.color;

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = ogColor;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void patroling()
    {
        if (!walkPointSet)
        {
            searchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkPoint reached 
        if(distanceToWalkPoint.magnitude <= 2f)
        {
            walkPointSet = false;
        }
    }

    private void searchWalkPoint()
    {
        //calculate random point in range
        float randZ = Random.Range(-walkPointRange, walkPointRange);
        float randX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        //check if walkPoint is valid
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    void updateUI()
    {
        HPBar.fillAmount = (float)HP / (float)HPOrig;
    }
}
