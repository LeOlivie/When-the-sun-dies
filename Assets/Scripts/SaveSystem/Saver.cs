using UnityEngine;
using SaveDatas;

public class Saver : MonoBehaviour
{
    [SerializeField] private string _saveName;

    private void Start()
    {
        if (_saveName == "Base")
        {
            LoadBase();
            return;
        }

        if (!SaveLoadManager.CheckIfSaveExists(_saveName))
        {
            return;
        }


        LoadLocation();
        //SaveLoadManager.Save<StorageSaveData>("Test",new StorageSaveData(GameObject.FindObjectOfType<StorageOpener>().Storage));
        //GameObject.FindObjectOfType<StorageOpener>().LoadSaveData(SaveLoadManager.Load<StorageSaveData>("Test"));
    }

    void Update()
    {
        if (Input.GetButtonDown("Save"))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Saves Deleted.");
        }
    }

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

        SaveLoadManager.Save<LocationSaveData>(_saveName, new LocationSaveData(lootSpawners, harvesters));
        Debug.Log("Location saved");
    }

    public void SaveBase()
    {
        CraftingStation workbench = GameObject.FindObjectsOfType<CraftStationOpener>(true)[0].Station;
        CraftingStation cookingStation = GameObject.FindObjectsOfType<CraftStationOpener>(true)[1].Station;
        Storage storage = GameObject.FindObjectOfType<StorageOpener>().Storage;
        TV tv = GameObject.FindObjectOfType<TVScreenOpener>(true).Tv;
        Bed bed = GameObject.FindObjectOfType<BedOpener>().Bed;
        WaterCollector waterCollector = GameObject.FindObjectOfType<WaterCollectorOpener>().WaterCollector;
        SaveLoadManager.Save<BaseSaveData>("BaseSaveData", new BaseSaveData(workbench,cookingStation,storage,tv,bed,waterCollector));
        Debug.Log("Base saved");
    }

    public void LoadBase()
    {
        BaseSaveData baseSaveData = SaveLoadManager.Load<BaseSaveData>("BaseSaveData");
        GameObject.FindObjectsOfType<CraftStationOpener>(true)[0].LoadSaveData(baseSaveData.WorkbenchSave);
        GameObject.FindObjectsOfType<CraftStationOpener>(true)[1].LoadSaveData(baseSaveData.CookingStationSave);
        GameObject.FindObjectOfType<StorageOpener>().LoadSaveData(baseSaveData.StorageSave);
        GameObject.FindObjectOfType<TVScreenOpener>(true).LoadSaveData(baseSaveData.TVSave);
        GameObject.FindObjectOfType<BedOpener>().LoadSaveData(baseSaveData.BedSave);
        GameObject.FindObjectOfType<WaterCollectorOpener>().LoadSaveData(baseSaveData.WaterCollectorSave);
        Debug.Log("Base load");
    }

    private void LoadPlayer()
    {
        PlayerSaveData playerSave = SaveLoadManager.Load<PlayerSaveData>("PlayerSave");
        GlobalRepository.LoadSaveData(playerSave);
        Debug.Log("Player data loaded");
    }

    public void LoadLocation()
    {
        LocationSaveData locSave = SaveLoadManager.Load<LocationSaveData>(_saveName);
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
