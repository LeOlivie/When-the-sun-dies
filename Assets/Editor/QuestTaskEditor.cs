using UnityEditor;

[CustomEditor(typeof(QuestTaskData))]
public class QuestTaskEditor : Editor
{
    #region SerializedProperties
    SerializedProperty _taskType;
    SerializedProperty _id;
    SerializedProperty _subQuestText;
    SerializedProperty _itemsRequired;
    #endregion

    private void OnEnable()
    {
        _taskType = serializedObject.FindProperty("_taskType");
        _id = serializedObject.FindProperty("_id");
        _subQuestText = serializedObject.FindProperty("_subQuestText");
        _itemsRequired = serializedObject.FindProperty("_itemsRequired");
    }

    public override void OnInspectorGUI()
    {
        QuestTaskData questTask = (QuestTaskData)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(_taskType);
        EditorGUILayout.PropertyField(_id);
        EditorGUILayout.PropertyField(_subQuestText);

        if (questTask.TaskType == QuestTaskData.TaskTypeEnum.Fix || questTask.TaskType == QuestTaskData.TaskTypeEnum.Give)
        {
            EditorGUILayout.PropertyField(_itemsRequired);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
