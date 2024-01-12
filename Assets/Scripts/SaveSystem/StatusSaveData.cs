using Statuses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveDatas
{
    [Serializable]
    public class StatusSaveData
    {
        public StatusData StatusData;
        public int Progress;

        public StatusSaveData(Status status) 
        {
            StatusData = status.Data;
            Progress = status.Progress;
        }
    }
}