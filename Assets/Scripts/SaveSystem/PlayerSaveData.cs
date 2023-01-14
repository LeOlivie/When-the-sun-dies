using UnityEngine;
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
        public int TotalTime;

        public PlayerSaveData(Vector3 playerPos, float calories, float water, ContainerSaveData inventorySaveData, int totalTime)
        {
            PlayerPos = new Vec3(playerPos.x, playerPos.y, playerPos.z);
            Calories = calories;
            Water = water;
            InventorySaveData = inventorySaveData;
            TotalTime = totalTime;
        }

        public PlayerSaveData()
        {

        }
    }
}