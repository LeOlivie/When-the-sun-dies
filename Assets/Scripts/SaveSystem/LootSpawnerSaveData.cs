using System;

namespace SaveDatas
{
    [Serializable]
    public class LootSpawnerSaveData
    {
        public ContainerSaveData ContainerSave;
        public bool IsLooted;
        public int InstanceID;

        public LootSpawnerSaveData(LootSpawner lootSpawner)
        {
            ContainerSave = new ContainerSaveData(lootSpawner.ItemContainer.Items);
            IsLooted = lootSpawner.IsLooted;
            InstanceID = lootSpawner.GetInstanceID();
        }

        public LootSpawnerSaveData()
        {

        }
    }
}