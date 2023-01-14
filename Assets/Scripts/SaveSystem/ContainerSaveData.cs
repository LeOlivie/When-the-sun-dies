using System;

namespace SaveDatas
{
    [Serializable]
    public class ContainerSaveData
    {
        public ItemSaveData[] SaveDatas;

        public ContainerSaveData(ItemSaveData[] itemSaveDatas)
        {
            SaveDatas = itemSaveDatas;
        }

        public ContainerSaveData(Item[] itemsToSave)
        {
            SaveDatas = new ItemSaveData[itemsToSave.Length];

            for(int i = 0; i < SaveDatas.Length; i++)
            {
                SaveDatas[i] = new ItemSaveData(itemsToSave[i]);
            }
        }

        public ContainerSaveData()
        {
            SaveDatas = new ItemSaveData[0];
        }
    }
}