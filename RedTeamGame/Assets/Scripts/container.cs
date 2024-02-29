using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class container : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
   
    [Header("---- HP")]
    [SerializeField] int HP;
    [SerializeField] int HPOrig;

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
        
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashMat());
        aud.PlayOneShot(metalSound);

        if (HP <= 0)
        {
            Destroy(gameObject);
            //navMeshSurface.BuildNavMesh();
        }
    }
    IEnumerator flashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        model.material.color = Color.white;
    }
}
