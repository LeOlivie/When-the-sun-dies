using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Statuses;

public class StatusScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private IndexedButtonHandler[] _buttons;
    [SerializeField] private TextMeshProUGUI _effectsText;
    

    private void OnEnable()
    {
        foreach (IndexedButtonHandler buttonHandler in  _buttons)
        {
            buttonHandler.gameObject.SetActive(false);
            buttonHandler.AddListener(ShowEffects);
        }

        if (GlobalRepository.ActiveStatuses.Count == 0)
        {
            _effectsText.text = "No active statuses";
            return;
        }

        _effectsText.text = "-Select status-";
        for (int i = 0; i < GlobalRepository.ActiveStatuses.Count; i++)
        {
            _buttons[i].gameObject.SetActive(true);
            _buttons[i].SetIndex(i);
            _buttons[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = GlobalRepository.ActiveStatuses[i].Data.Name;
        }
    }

    public void CloseScreen()
    {
        foreach (IndexedButtonHandler buttonHandler in _buttons)
        {
            buttonHandler.RemoveListener(ShowEffects);
        }
        this.gameObject.SetActive(false);
    }

    private void ShowEffects(int index)
    {
        _effectsText.text = "";
        foreach (EffectData effect in GlobalRepository.ActiveStatuses[index].GetActiveEffects())
        {
            _effectsText.text += $"<size=25>{effect.Name}</size>\n<size=15>{effect.Description}</size>\n\n";
        }
    }
}
