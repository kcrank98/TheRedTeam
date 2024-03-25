using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] float spawnTimer;
    [SerializeField] Transform[] spawnPos;
   

    public int totalEnemy;
    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    [Header("---- Audio")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip gunSpawnSound;
    [Range(0, 1)][SerializeField] float gunSpawnVol;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager.instance.updateGameGoal(numberToSpawn);
        totalEnemy = numberToSpawn;
        //gameManager.instance.updateGameGoal(totalEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numberToSpawn)
        {
            StartCoroutine(spawn());
            if (gunSpawnSound != null)
                aud.PlayOneShot(gunSpawnSound, gunSpawnVol);
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;


        int arrayPos = Random.Range(0, spawnPos.Length);

        Instantiate(objectToSpawn, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        spawnCount++;
        yield return new WaitForSeconds(spawnTimer);

        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }
}