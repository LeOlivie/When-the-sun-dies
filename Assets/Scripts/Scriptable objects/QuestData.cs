using UnityEngine;
using System;

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/QuestData", order = 12)]
public class QuestData : ScriptableObject
{
    [SerializeField] private string _questName;
    [SerializeField, TextArea] private string _questDescription;
    [SerializeField] private string _questLocationName;
    [SerializeField] private Item[] _questReward;
    [SerializeField] private QuestTaskData[] _questTaskDatas;

    public string QuestName => _questName;
    public string QuestDescription => _questDescription;
    public string QuestLocationName => _questLocationName;
    public Item[] QuestReward => _questReward;
    public QuestTaskData[] QuestTaskDatas => _questTaskDatas;
}
