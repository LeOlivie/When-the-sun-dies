using System;
using System.Diagnostics;

namespace SaveDatas
{
    [Serializable]
    public class LocationSaveData 
    {
        public int SaveTime;
        public LootSpawnerSaveData[] LootSpawnerSaveDatas;
        public HarvestPOISave[] HarvestSaves;


        public LocationSaveData(string saveName, LootSpawner[] lootSpawners, Harvester[] harvesters)
        {
            if (SaveLoadManager.CheckIfSaveExists(saveName))
            {
                SaveTime = SaveLoadManager.Load<LocationSaveData>(saveName).SaveTime;
            }
            else
            {
                SaveTime = GlobalRepository.SystemVars.GlobalTime;
            }
            
            LootSpawnerSaveDatas = new LootSpawnerSaveData[lootSpawners.Length];

            for(int i = 0; i < lootSpawners.Length; i++)
            {
                LootSpawnerSaveDatas[i] = new LootSpawnerSaveData(lootSpawners[i]);
            }

            HarvestSaves = new HarvestPOISave[harvesters.Length];

            for (int i = 0; i < HarvestSaves.Length; i++)
            {
                HarvestSaves[i] = new HarvestPOISave(harvesters[i]);
            }
        }

        public LocationSaveData()
        {
        }
    }
}
