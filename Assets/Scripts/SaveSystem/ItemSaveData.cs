using System;

namespace SaveDatas
{
    [Serializable]
    public class ItemSaveData
    {
        public ItemData Data;
        public int Count;

        public ItemSaveData(ItemData data, int count)
        {
            Data = data;
            Count = count;
        }

        public ItemSaveData(Item item)
        {
            if (item != null) 
            {
                Data = item.ItemData;
                Count = item.Count;
            }
        }

        public ItemSaveData()
        {

        }
    }
}
