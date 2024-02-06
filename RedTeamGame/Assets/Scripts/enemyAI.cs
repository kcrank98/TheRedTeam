using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour//, IDamage
{
    [SerializeField] int HP;

    [SerializeField] Renderer model;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashMat());

        if (HP <= 0)
        {
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
}
