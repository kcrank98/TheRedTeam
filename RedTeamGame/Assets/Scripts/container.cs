using System.Collections;
using UnityEngine;

public class container : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
   
    [Header("---- HP")]
    [SerializeField] int HP;
    [SerializeField] int HPOrig;

    [Header("---- Damage")]
    [SerializeField] int damageAmount;
    [SerializeField] int damageTaken;

    [Header("---- Audio")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip metalSound;
    [Range(0, 1)][SerializeField] float metalSoundVol;
    //public NavMeshSurface navMeshSurface;



    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        //navMeshSurface = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        die();
    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        if(model != null)
        {
            StartCoroutine(flashMat());
        }
        aud.PlayOneShot(metalSound);

        die();
    }
    IEnumerator flashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        model.material.color = Color.white;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            if (!other.isTrigger /*&& !other.CompareTag("Player")*/)
            {
                dmg.takeDamage(damageAmount);
                aud.PlayOneShot(metalSound);

                HP -= damageTaken;
            }
        }
    }

    void die()
    {
        if (HP <= 0)
        {
            LootBag loot = gameObject.GetComponent<LootBag>();

            if (loot != null)
            {
                GetComponent<LootBag>().instantiateLoot(transform.position);
            }

            Destroy(gameObject);
            //navMeshSurface.BuildNavMesh();
        }
    }
}
