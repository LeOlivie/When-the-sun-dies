using UnityEngine;
using TMPro;

public class QuestsScreenShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questInfoText;
    [SerializeField] private Color _subQuestCompletedColor;
    
    private void OnEnable()
    {
        if (GlobalRepository.SystemVars.ActiveQuest == null)
        {
            _questInfoText.text = "[No quests active]";
            return;
        }

        _questInfoText.text = GlobalRepository.SystemVars.ActiveQuest.GetInfo(_subQuestCompletedColor);
    }

}
