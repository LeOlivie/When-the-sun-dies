using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveDatas
{
    [Serializable]
    public class QuestSaveData
    {
        public QuestData Data;
        public List<bool> TaskStatuses;
        public List<int> SubQuestsProgress;

        public QuestSaveData(Quest quest)
        {
            Data = quest.Data;
            TaskStatuses = new List<bool>();
            SubQuestsProgress = new List<int>();

            foreach (Quest.SubQuest subQuest in quest.SubQuests)
            {
                TaskStatuses.Add(subQuest.IsCompleted);
                SubQuestsProgress.Add(subQuest.Progress);
            }
        }
    }
}