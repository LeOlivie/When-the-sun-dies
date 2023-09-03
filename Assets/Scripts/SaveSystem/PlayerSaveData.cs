using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace SaveDatas {

    [Serializable]
    public class PlayerSaveData
    {
        [Serializable]
        public struct Vec3
        {
            public float xPos, yPos, zPos;

            public Vec3(float x, float y, float z)
            {
                xPos = x;
                yPos = y;
                zPos = z;
            }
        }

        public Vec3 PlayerPos;
        public float Calories;
        public float Water;
        public ContainerSaveData InventorySaveData;
        public int GlobalTime;
        public QuestSaveData QuestSave;
        public int QuestsProgress;
        public DifficultyData DifficultyData;

        public PlayerSaveData(Vector3 playerPos, ContainerSaveData inventorySaveData)
        {
            PlayerPos = new Vec3(playerPos.x, playerPos.y, playerPos.z);
            Calories = GlobalRepository.Kcal;
            Water = GlobalRepository.Water;
            InventorySaveData = inventorySaveData;
            GlobalTime = GlobalRepository.GlobalTime;
            QuestsProgress = GlobalRepository.QuestsProgress;
            DifficultyData = GlobalRepository.Difficulty;
            QuestSave = null;

            if (GlobalRepository.ActiveQuest != null)
            {
                QuestSave = new QuestSaveData(GlobalRepository.ActiveQuest);
            }
        }

        public PlayerSaveData()
        {

        }
    }
}