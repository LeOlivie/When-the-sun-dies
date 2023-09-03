using System;

namespace SaveDatas
{
    public abstract class Savable : UnityEngine.MonoBehaviour
    {
        public abstract SaveData GetSaveData();
        public abstract void LoadSaveData(SaveData saveData);
    }
}