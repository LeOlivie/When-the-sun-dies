using UnityEngine;

public abstract class QuestPointManager : MonoBehaviour
{
    public delegate void QuestPointExecutedDelegate(string ID);
    public event QuestPointExecutedDelegate OnQuestPointExecuted;
    private QuestInformer _questInformer;
    private bool _isQuestPointActivated;
    [SerializeField] protected string _id;

    public string ID => _id;

    private void Awake()
    {
        _questInformer = FindObjectOfType<QuestInformer>(true);
        this.gameObject.SetActive(_isQuestPointActivated);
    }

    public virtual void QuestPointExecuted()
    {
        _questInformer.ShowMessage("Task complete: " + GlobalRepository.SystemVars.ActiveQuest.Data.QuestName);
        OnQuestPointExecuted?.Invoke(_id);
        OnQuestPointExecuted = null;
    }

    public void ActivateQuestPoint()
    {
        _isQuestPointActivated = true;
        this.gameObject.SetActive(true);
        
    }
}
