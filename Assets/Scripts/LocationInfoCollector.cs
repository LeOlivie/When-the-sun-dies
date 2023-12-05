using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LocationInfoCollector : MonoBehaviour
{
    private void Start()
    {
        LootSpawner[] lootSpawners = GameObject.FindObjectsOfType<LootSpawner>();
        Harvester[] harvesters = GameObject.FindObjectsOfType<Harvester>();

        float lootSpawnersKCalPrice = 0f;
        float lootSpawnersMLPrice = 0f;

        for (int i = 0; i < 50; i++)
        {
            foreach (LootSpawner lootSpawner in lootSpawners)
            {
                foreach (Item item in lootSpawner.LootSpawnerData.GetSpawnedItems())
                {
                    lootSpawnersKCalPrice += item.ItemData.KcalPrice;
                    lootSpawnersMLPrice += item.ItemData.MLPrice;
                }
            }
        }
        Debug.Log("Avarage KCal loot price: " + lootSpawnersKCalPrice / 50 + "  |||  Avarage ML loot price: " + lootSpawnersMLPrice / 50);
    }
}
