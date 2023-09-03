namespace SaveDatas
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [System.Serializable]
    public class BaseSaveData
    {
        public List<string> SaveDatasJsons;
        public List<string> SaveDatasTypes;

        public SaveData[] GetSaveDatas()
        {
            List<SaveData> saveDatas = new List<SaveData>();

            for(int i = 0; i < SaveDatasJsons.Count; i++)
            {
                saveDatas.Add((SaveData)JsonUtility.FromJson(SaveDatasJsons[i], Type.GetType(SaveDatasTypes[i])));
            }

            return saveDatas.ToArray();
        }


        public BaseSaveData(params SaveData[] saveDatas)
        {
            SaveDatasJsons = new List<string>();
            SaveDatasTypes = new List<string>();

            foreach(SaveData saveData in saveDatas)
            {
                Type type = saveData.GetType();
                SaveDatasTypes.Add(type.ToString());
                SaveDatasJsons.Add(JsonUtility.ToJson(saveData));
            }
        }
    }
}