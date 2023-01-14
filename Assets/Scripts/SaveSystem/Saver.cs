using UnityEngine;
using SaveDatas;
using System.Collections.Generic;

public class Saver : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Save"))
        {
            SavePlayer();
            SaveLocation();
        }


        if (Input.GetButtonDown("Load"))
        {
            LoadPlayer();
            LoadLocation();
        }
    }

    private void SavePlayer()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
        int totalTime = GlobalRepository.Days * 24 * 60 + GlobalRepository.Hours * 60 + GlobalRepository.Minutes;
        ContainerSaveData invSave = new ContainerSaveData(GlobalRepository.Inventory.Items);
        PlayerSaveData playerSaveData = new PlayerSaveData(playerPos, GlobalRepository.Kcal, GlobalRepository.Water, invSave, totalTime);
        SaveLoadManager.Save<PlayerSaveData>("PlayerSave", playerSaveData);
        Debug.Log("Player data saved");
    }

    private void SaveLocation()
    {

        LootSpawner[] lootSpawners = GameObject.FindObjectsOfType<LootSpawner>(true);

        Harvester[] harvesters = GameObject.FindObjectsOfType<Harvester>(true);

        SaveLoadManager.Save<LocationSaveData>("LocSave", new LocationSaveData(lootSpawners, harvesters));
        Debug.Log("Location saved");
    }

    private void LoadPlayer()
    {
        PlayerSaveData playerSave = SaveLoadManager.Load<PlayerSaveData>("PlayerSave");
        GlobalRepository.LoadSaveData(playerSave);
        Debug.Log("Player data loaded");
    }

    private void LoadLocation()
    {
        LocationSaveData locSave = SaveLoadManager.Load<LocationSaveData>("LocSave");
        Dictionary<int, LootSpawnerSaveData> lootSpawnersDict = new Dictionary<int, LootSpawnerSaveData>();

        foreach (LootSpawnerSaveData lootSpawnerSaveData in locSave.LootSpawnerSaveDatas)
        {
            lootSpawnersDict.Add(lootSpawnerSaveData.InstanceID, lootSpawnerSaveData);
        }

        foreach (LootSpawner lootSpawner in GameObject.FindObjectsOfType<LootSpawner>(true))
        {
            lootSpawner.LoadSaveData(lootSpawnersDict[lootSpawner.GetInstanceID()]);
        }


        Dictionary<int, HarvestPOISave> harvestersDict = new Dictionary<int, HarvestPOISave>();

        foreach (HarvestPOISave harvesterSave in locSave.HarvestSaves)
        {
            harvestersDict.Add(harvesterSave.InstanceID, harvesterSave);
        }

        foreach (Harvester harvester in GameObject.FindObjectsOfType<Harvester>())
        {
            harvester.LoadSaveData(harvestersDict[harvester.GetInstanceID()]);
        }

        Debug.Log("Location loaded");
    }
}
