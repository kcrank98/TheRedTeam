using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;

    public int totalEnemy;
    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(numberToSpawn);
        totalEnemy = numberToSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numberToSpawn)
        {
            StartCoroutine(spawn());
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