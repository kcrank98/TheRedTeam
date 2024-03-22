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

    List<Loot> getLootList()
    {
        int randNum = Random.Range(1, 101); // 1-100 (min is inclusive, max is exclusive)
        List<Loot> possibleLoot = new List<Loot>();

        foreach (var loot in lootList)
        {
            if (randNum <= loot.dropChance)
            {
                possibleLoot.Add(loot);
            }
        }

        if (possibleLoot.Count > 0)
        {
            return possibleLoot;
        }

        Debug.Log("No loot dropped");
        return null;
    }

    public void instantiateLoot(Vector3 spawnPos, int notChest = 0)
    {
        Loot droppedLoot = null;
        List<Loot> droppedLootList = null;

        if (notChest == 0)
        {
            droppedLoot = getLoot();
        }
        else
        {
            droppedLootList = getLootList();
        }

        if(droppedLoot != null)
        {
            GameObject lootGameObject = Instantiate(lootPrefab, spawnPos, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedLoot.lootSprite;

            HealthPack healthPotion = lootGameObject.GetComponent<HealthPack>();
            ShieldPickUp shieldPotion = lootGameObject.GetComponent<ShieldPickUp>();
            keyScript key = lootGameObject.GetComponent<keyScript>();
            ammoDrop ammo = lootGameObject.GetComponent<ammoDrop>();

            if (droppedLoot.lootName == "Health Potion")
            {
                healthPotion.enabled = true;
            }

            if(droppedLoot.lootName == "Shield Potion")
            {
                shieldPotion.enabled = true;
            }

            if (droppedLoot.lootName == "Key Drop")
            {
                key.enabled = true;
            }

            if (droppedLoot.lootName == "Ammo Drop")
            {
                ammo.enabled = true;
            }

            //Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            //lootGameObject.GetComponent<Rigidbody>().AddForce(dropDirection * dropForce, ForceMode.Impulse);
        }

        if (droppedLootList != null)
        {
            GameObject lootGameObject = null;

            for (int i = 0; i < droppedLootList.Count; i++)
            {
                lootGameObject = Instantiate(lootPrefab, spawnPos, Quaternion.identity);
                lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedLootList[i].lootSprite;

                HealthPack healthPotion = lootGameObject.GetComponent<HealthPack>();
                ShieldPickUp shieldPotion = lootGameObject.GetComponent<ShieldPickUp>();
                keyScript key = lootGameObject.GetComponent<keyScript>();
                ammoDrop ammo = lootGameObject.GetComponent<ammoDrop>();

                if (droppedLootList[i].lootName == "Health Potion")
                {
                    healthPotion.enabled = true;
                }

                if (droppedLootList[i].lootName == "Shield Potion")
                {
                    shieldPotion.enabled = true;
                }

                if (droppedLootList[i].lootName == "Key Drop")
                {
                    key.enabled = true;
                }

                if (droppedLootList[i].lootName == "Ammo Drop")
                {
                    ammo.enabled = true;
                }
            }

            //Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            //lootGameObject.GetComponent<Rigidbody>().AddForce(dropDirection * dropForce, ForceMode.Impulse);
        }
    }
}
