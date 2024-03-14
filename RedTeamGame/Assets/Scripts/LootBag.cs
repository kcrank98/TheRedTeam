using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField] GameObject lootPrefab;
    [SerializeField] List<Loot> lootList = new List<Loot>();
    [SerializeField] float dropForce;

    Loot getLoot()
    {
        int randNum = Random.Range(1, 101); // 1-100 (min is inclusive, max is exclusive)
        List<Loot> possibleLoot = new List<Loot>();

        foreach (var loot in lootList)
        {
            if(randNum <= loot.dropChance)
            {
                possibleLoot.Add(loot);
            }
        }

        if(possibleLoot.Count > 0)
        {
            return possibleLoot[Random.Range(0, possibleLoot.Count)];
        }

        Debug.Log("No loot dropped");
        return null;
    }

    public void instantiateLoot(Vector3 spawnPos)
    {
        Loot droppedLoot = getLoot();

        if(droppedLoot != null)
        {
            GameObject lootGameObject = Instantiate(lootPrefab, spawnPos, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedLoot.lootSprite;

            HealthPack healthPotion = lootGameObject.GetComponent<HealthPack>();
            ShieldPickUp shieldPotion = lootGameObject.GetComponent<ShieldPickUp>();

            if (droppedLoot.lootName == "Health Potion")
            {
                healthPotion.enabled = true;
            }

            if(droppedLoot.lootName == "Shield Potion")
            {
                shieldPotion.enabled = true;
            }

            Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            lootGameObject.GetComponent<Rigidbody>().AddForce(dropDirection * dropForce, ForceMode.Impulse);
        }
    }
}
