using System;

namespace SaveDatas
{
    [Serializable]
    public class LocationSaveData 
    {
        public LootSpawnerSaveData[] LootSpawnerSaveDatas;
        public HarvestPOISave[] HarvestSaves;


        public LocationSaveData(LootSpawner[] lootSpawners, Harvester[] harvesters)
        {
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
