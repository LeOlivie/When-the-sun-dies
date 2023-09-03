using System;

namespace SaveDatas
{
    [Serializable]
    public class StorageSaveData : UpgradableSaveData
    {
        public ContainerSaveData ContainerSave;

        public StorageSaveData(Storage storage) : base(storage)
        {
            ContainerSave = new ContainerSaveData(storage.ItemContainer.Items);
        }
    }
}