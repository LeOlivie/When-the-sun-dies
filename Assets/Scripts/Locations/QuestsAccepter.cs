using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestsAccepter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questInfoText;
    [SerializeField] private TextMeshProUGUI _acceptBtnText;
    [SerializeField] private Color _subQuestCompletedColor;
    [SerializeField] private QuestData[] _questDatas;
    [SerializeField] private ButtonHandler _acceptBtn;
    [SerializeField] private ItemShower[] _rewardItemShowers;

    private void OnEnable()
    {
        _acceptBtn.ResetListeners();

        if (GlobalRepository.SystemVars.ActiveQuest != null)
        {
            _questInfoText.text = GlobalRepository.SystemVars.ActiveQuest.GetInfo(_subQuestCompletedColor);
            ShowQuestRewards(GlobalRepository.SystemVars.ActiveQuest.Data.QuestReward);
            _acceptBtnText.text = "Finish";
            _acceptBtnText.color = Color.white;

            if (GlobalRepository.SystemVars.ActiveQuest.IsCompleted)
            {
                _acceptBtn.AddListener(FinishQuest);
                _acceptBtnText.color = _subQuestCompletedColor;
            }
        }
        else
        {
            _questInfoText.text = new Quest(_questDatas[GlobalRepository.PlayerVars.QuestsProgress]).GetInfo(_subQuestCompletedColor);
            _acceptBtnText.text = "Accept";
            _acceptBtnText.color = _subQuestCompletedColor;
            ShowQuestRewards(new Item[0]);
            _acceptBtn.AddListener(AcceptQuest);
        }
    }

    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
    }

    private void AcceptQuest()
    {
        GlobalRepository.SystemVars.ActiveQuest=new Quest(_questDatas[GlobalRepository.PlayerVars.QuestsProgress]);
        OnEnable();
    }

    private void ShowQuestRewards(Item[] items)
    {
        foreach (ItemShower itemShower in _rewardItemShowers)
        {
            itemShower.ShowItem(null);
        }

        for (int i = 0; i < items.Length; i++)
        {
            _rewardItemShowers[i].ShowItem(items[i]);
        }
    }

    private void FinishQuest()
    {
        foreach (Item item in GlobalRepository.SystemVars.ActiveQuest.Data.QuestReward)
        {
            GlobalRepository.PlayerVars.Inventory.AddItem(new Item(item.ItemData, item.Count), false);
        }
        GlobalRepository.PlayerVars.QuestsProgress = GlobalRepository.PlayerVars.QuestsProgress + 1;
        GlobalRepository.SystemVars.ActiveQuest = null;
        OnEnable();
    }
}
