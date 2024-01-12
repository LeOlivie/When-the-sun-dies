using Statuses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AcriveEffectsShower : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = "";

        foreach (Status status in GlobalRepository.PlayerVars.ActiveStatuses)
        {
            _text.text += $"<sprite name=\"{status.Data.IconName}\"> ";
        }
    }
}
