using UnityEngine;
using System;

namespace SaveDatas
{
    [Serializable]
    public class LootSpawnerSaveData
    {
        public ContainerSaveData ContainerSave;
        public bool IsLooted;

        public LootSpawnerSaveData(LootSpawner lootSpawner)
        {
            if (lootSpawner.ItemContainer == null)
            {
                return;
            }

            ContainerSave = new ContainerSaveData(lootSpawner.ItemContainer.Items);
            IsLooted = lootSpawner.IsLooted;
        }

        public LootSpawnerSaveData()
        {

        }
    }
}