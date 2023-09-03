using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class Quest
{
    public struct SubQuest
    {
        private QuestTaskData _subQuestData;
        private bool _isCompleted;
        private int _progress;
        private string _id;

        public QuestTaskData SubQuestData => _subQuestData;
        public bool IsCompleted => _isCompleted;
        public int Progress => _progress;
        public string ID => _id;

        public void ChangeProgess(int change)
        {
            _progress += change;
        }

        public void SetCompletedStatus(bool status)
        {
            _isCompleted = status;
        }

        public SubQuest(QuestTaskData subQuestData)
        {
            _subQuestData = subQuestData;
            _isCompleted = false;
            _progress = 0;
            _id = subQuestData.ID;
        }
    }

    private QuestData _data;
    private bool _isCompleted;
    private SubQuest[] _subQuests;
        
    public QuestData Data => _data;
    public bool IsCompleted => _isCompleted;
    public SubQuest[] SubQuests => _subQuests;


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != _data.QuestLocationName)
        {
            return;
        }
        
        foreach (QuestPointManager questPointManager in GameObject.FindObjectsOfType<QuestPointManager>(true))
        {
            if (CheckIfHasSubQuest(questPointManager.ID) == false || FindSubQuest(questPointManager.ID).IsCompleted)
            {
                continue;
            }
            questPointManager.ActivateQuestPoint();
            questPointManager.OnQuestPointExecuted += OnQuestPointExecuted;
        }
    }

    private void OnQuestPointExecuted(string ID)
    {
        FindSubQuest(ID).SetCompletedStatus(true);
        CheckIfQuestCompleted();
    }

    public void ChangeTaskProgress(ref SubQuest subQuest, int progressChange)
    {
        subQuest.ChangeProgess(progressChange);

        if (subQuest.Progress >= subQuest.SubQuestData.ItemsRequiered.Count)
        {
            subQuest.SetCompletedStatus(true);
            CheckIfQuestCompleted();
        }
    }

    private void CheckIfQuestCompleted()
    {
        foreach (SubQuest subQuest in _subQuests)
        {
            if (!subQuest.IsCompleted)
            {
                return;
            }
        }
        _isCompleted = true;
    }

    public string GetInfo(Color subQuestCompletedColor)
    {
        string info = "";
        info = $"<size=30>{this.Data.QuestName}</size>\n";
        info += $"<size=15>Location: {this.Data.QuestLocationName}</size>\n\n";
        info += $"<size=15>{this.Data.QuestDescription}</size>\n\n";

        foreach (Quest.SubQuest subQuest in this.SubQuests)
        {
            Color taskTextColor = Color.white;

            if (subQuest.IsCompleted)
            {
                taskTextColor = subQuestCompletedColor;
            }

            info += $"<color=#{ColorUtility.ToHtmlStringRGBA(taskTextColor)}>Task: {subQuest.SubQuestData.SubQuestText}";

            if (subQuest.SubQuestData.TaskType == QuestTaskData.TaskTypeEnum.Give)
            {
                info += $" - {subQuest.Progress}/{subQuest.SubQuestData.ItemsRequiered.Count}";
            }

            if (subQuest.IsCompleted)
            {
                info += " (DONE)";
            }

            info += "</color>\n";

        }

        if (this.IsCompleted)
        {
            info += $"\n<color=#{ColorUtility.ToHtmlStringRGBA(subQuestCompletedColor)}>Quest completed\n<size=10>Return to the trader to claim your reward</size></color>";
        }

        return info;
    }
    public ref SubQuest FindSubQuest(string ID)
    {
        for (int i = 0; i < _subQuests.Length; i++)
        {
            if (_subQuests[i].SubQuestData.ID == ID)
            {
                return ref _subQuests[i];
            }
        }
        return ref _subQuests[0];
    }

    public bool CheckIfHasSubQuest(string ID)
    {
        foreach (SubQuest subQuest in _subQuests)
        {
            if (subQuest.SubQuestData.ID == ID)
            {
                return true;
            }
        }
        return false;
    }

    public Quest(QuestData data)
    {
        _data = data;
        SceneManager.sceneLoaded += OnSceneLoaded;
        _subQuests = new SubQuest[data.QuestTaskDatas.Length];

        for (int i = 0; i < data.QuestTaskDatas.Length; i++)
        {
            _subQuests[i] = new SubQuest(data.QuestTaskDatas[i]);
        }
    }

    public Quest(SaveDatas.QuestSaveData questSaveData) : this(questSaveData.Data)
    {
        for (int i = 0; i < questSaveData.TaskStatuses.Count; i++)
        {
            _subQuests[i].SetCompletedStatus(questSaveData.TaskStatuses[i]);
            _subQuests[i].ChangeProgess(questSaveData.SubQuestsProgress[i]);
        }
    }
}
