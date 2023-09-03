using UnityEngine;
using System;

[CreateAssetMenu(fileName = "QuestTaskData", menuName = "ScriptableObjects/QuestTaskData", order = 13)]
public class QuestTaskData : ScriptableObject
{
    public enum TaskTypeEnum { Locate, Collect, Fix, Give };
    [SerializeField] private TaskTypeEnum _taskType;
    [SerializeField] private string _id;
    [SerializeField] private string _subQuestText;
    [SerializeField] private Item _itemsRequired;
    
    public TaskTypeEnum TaskType => _taskType;
    public string ID => _id;
    public string SubQuestText => _subQuestText;
    public Item ItemsRequiered => _itemsRequired;

}
