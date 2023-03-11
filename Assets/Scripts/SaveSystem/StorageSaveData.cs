using System;

namespace SaveDatas
{
    [Serializable]
    public class StorageSaveData
    {
        public UpgradableSaveData UpgradableSave;
        public ContainerSaveData ContainerSave;

        public StorageSaveData(Storage storage)
        {
            UpgradableSave = new UpgradableSaveData(storage);
            ContainerSave = new ContainerSaveData(storage.ItemContainer.Items);
        }
    }
}