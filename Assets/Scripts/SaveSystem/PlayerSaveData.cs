using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Statuses;

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
        public List<StatusSaveData> statusSaveDatas = new List<StatusSaveData>();
        public float Health;
        public bool IsStartKitReceived;
        public List<string> LastEatenFood = new List<string>();

        public PlayerSaveData(Vector3 playerPos, ContainerSaveData inventorySaveData)
        {
            PlayerPos = new Vec3(playerPos.x, playerPos.y, playerPos.z);
            Calories = GlobalRepository.PlayerVars.KCal;
            Water = GlobalRepository.PlayerVars.KCal;
            InventorySaveData = inventorySaveData;
            GlobalTime = GlobalRepository.SystemVars.GlobalTime;
            QuestsProgress = GlobalRepository.PlayerVars.QuestsProgress;
            DifficultyData = GlobalRepository.SystemVars.Difficulty;
            QuestSave = null;
            Health = GlobalRepository.PlayerVars.Health;
            IsStartKitReceived = GlobalRepository.SystemVars.IsStartKitReceived;
            LastEatenFood = GlobalRepository.PlayerVars.LastEatenFood;

            if (GlobalRepository.SystemVars.ActiveQuest != null)
            {
                QuestSave = new QuestSaveData(GlobalRepository.SystemVars.ActiveQuest);
            }

            foreach (Status status in GlobalRepository.PlayerVars.ActiveStatuses)
            {
                statusSaveDatas.Add(new StatusSaveData(status));
            }
        }

        public PlayerSaveData()
        {

        }
    }
}