using UnityEngine;
using TMPro;

public class QuestsScreenShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questInfoText;
    [SerializeField] private Color _subQuestCompletedColor;
    
    private void OnEnable()
    {
        if (GlobalRepository.ActiveQuest == null)
        {
            _questInfoText.text = "[No quests active]";
            return;
        }

        _questInfoText.text = GlobalRepository.ActiveQuest.GetInfo(_subQuestCompletedColor);
    }

}
