using UnityEngine;
using SaveDatas;
using System.Collections.Generic;

public class Saver : MonoBehaviour
{
    private void Start()
    {
       LoadLocation();
    }

    /*void Update()
    {
        if (Input.GetButtonDown("Save"))
        {
            SaveLocation();
        }


        if (Input.GetButtonDown("Load"))
        {
            LoadLocation();
        }
    }*/

    private void SavePlayer()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
        ContainerSaveData invSave = new ContainerSaveData(GlobalRepository.Inventory.Items);
        PlayerSaveData playerSaveData = new PlayerSaveData(playerPos, GlobalRepository.Kcal, GlobalRepository.Water, invSave, GlobalRepository.GlobalTime);
        SaveLoadManager.Save<PlayerSaveData>("PlayerSave", playerSaveData);
        Debug.Log("Player data saved");
    }

    public void SaveLocation()
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

    public void LoadLocation()
    {
        LocationSaveData locSave = SaveLoadManager.Load<LocationSaveData>("LocSave");
        /*Dictionary<float, LootSpawnerSaveData> lootSpawnersDict = new Dictionary<float, LootSpawnerSaveData>();

        foreach (LootSpawnerSaveData lootSpawnerSaveData in locSave.LootSpawnerSaveDatas)
        {
            lootSpawnersDict.Add(lootSpawnerSaveData.XCoordinate, lootSpawnerSaveData);
        }


        foreach (LootSpawner lootSpawner in GameObject.FindObjectsOfType<LootSpawner>(true))
        {
            lootSpawner.LoadSaveData(lootSpawnersDict[lootSpawner.transform.position.x]);
            if (lootSpawnersDict.ContainsKey(lootSpawner.transform.position.x))
            {
            }
        }*/

        for (int i = 0; i < locSave.LootSpawnerSaveDatas.Length; i++)
        {
            GameObject.FindObjectsOfType<LootSpawner>(true)[i].LoadSaveData(locSave.LootSpawnerSaveDatas[i]);
        }

        /*
        Dictionary<int, HarvestPOISave> harvestersDict = new Dictionary<int, HarvestPOISave>();


        foreach (HarvestPOISave harvesterSave in locSave.HarvestSaves)
        {
            harvestersDict.Add(harvesterSave.InstanceID, harvesterSave);
        }


        foreach (Harvester harvester in GameObject.FindObjectsOfType<Harvester>())
        {
            if (lootSpawnersDict.ContainsKey(harvester.GetInstanceID()))
            {
                harvester.LoadSaveData(harvestersDict[harvester.GetInstanceID()]);
            }
        }*/

        for (int i = 0; i < locSave.HarvestSaves.Length; i++)
        {
            GameObject.FindObjectsOfType<Harvester>(true)[i].LoadSaveData(locSave.HarvestSaves[i]);
        }

            Debug.Log("Location loaded");
    }
}
