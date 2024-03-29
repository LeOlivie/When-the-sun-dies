using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDatas;
using System.Collections.Generic;

public class Saver : MonoBehaviour
{
    private const string _baseName = "Base";
    private string _saveName;

    private void Start()
    {
        _saveName = SceneManager.GetActiveScene().name;
        if (_saveName == _baseName)
        {
            LoadBase();
            SavePlayer();
            return;
        }

        if (!SaveLoadManager.CheckIfSaveExists(_saveName))
        {
            return;
        }

        LoadLocation();
    }

    void Update()
    {
        if (Input.GetButtonDown("Save"))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Saves Deleted.");
        }
    }

    public void SavePlayer()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
        ContainerSaveData invSave = new ContainerSaveData(GlobalRepository.PlayerVars.Inventory.Items);
        PlayerSaveData playerSaveData = new PlayerSaveData(playerPos, invSave);
        SaveLoadManager.Save<PlayerSaveData>("PlayerSave", playerSaveData);
        Debug.Log("Player data saved");
    }

    public void SaveLocation()
    {
        LootSpawner[] lootSpawners = GameObject.FindObjectsOfType<LootSpawner>(true);
        Harvester[] harvesters = GameObject.FindObjectsOfType<Harvester>(true);

        LocationSaveData saveData = new LocationSaveData(_saveName, lootSpawners, harvesters);

        SaveLoadManager.Save<LocationSaveData>(_saveName, saveData);
        Debug.Log("Save time: " + saveData.SaveTime);
        Debug.Log("Location saved");
    }

    public void SaveBase()
    {
        Savable[] savables = GameObject.FindObjectsOfType<Savable>(true);
        SaveData[] saveDatas = new SaveData[savables.Length];

        for (int i = 0; i < savables.Length; i++)
        {
            saveDatas[i] = savables[i].GetSaveData();
        }

        SaveLoadManager.Save<BaseSaveData>(_baseName, new BaseSaveData(saveDatas));

        Debug.Log("Base saved");
    }

    public void LoadBase()
    {
        Savable[] loadables = GameObject.FindObjectsOfType<Savable>(true);
        BaseSaveData baseSaveData = SaveLoadManager.Load<BaseSaveData>(_baseName);
        SaveData[] saveDatas = baseSaveData.GetSaveDatas();

        for (int i = 0; i < loadables.Length; i++)
        {
            loadables[i].LoadSaveData(saveDatas[i]);
        }

        Debug.Log("Base load");
    }

    public void LoadPlayer()
    {
        PlayerSaveData playerSave = SaveLoadManager.Load<PlayerSaveData>("PlayerSave");
        GlobalRepository.LoadSaveData(playerSave);
        Debug.Log("Player data loaded");
    }

    public void LoadLocation()
    {
        LocationSaveData locSave = SaveLoadManager.Load<LocationSaveData>(_saveName);
        Debug.Log("Save time: " + locSave.SaveTime);
        if (locSave.SaveTime + GlobalRepository.SystemVars.Difficulty.LocationResetDelay * 1440 <= GlobalRepository.SystemVars.GlobalTime)
        {
            PlayerPrefs.DeleteKey(_saveName);
            Debug.Log("Location reset");
            return;
        }

        for (int i = 0; i < locSave.LootSpawnerSaveDatas.Length; i++)
        {
            GameObject.FindObjectsOfType<LootSpawner>(true)[i].LoadSaveData(locSave.LootSpawnerSaveDatas[i]);
        }

        for (int i = 0; i < locSave.HarvestSaves.Length; i++)
        {
            GameObject.FindObjectsOfType<Harvester>(true)[i].LoadSaveData(locSave.HarvestSaves[i]);
        }

        Debug.Log("Location loaded");
    }
}
