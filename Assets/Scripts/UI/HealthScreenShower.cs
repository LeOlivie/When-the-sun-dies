using UnityEngine;
using TMPro;
using System;

public class HealthScreenShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nutrientsText;


    private void Update()
    {
        string kcal = Mathf.Round(GlobalRepository.Kcal).ToString();

        _nutrientsText.text = "Skills\n";

        foreach (string str in Enum.GetNames(typeof(GlobalRepository.SkillType)))
        {
            GlobalRepository.SkillType skillType = (GlobalRepository.SkillType)Enum.Parse(typeof(GlobalRepository.SkillType), str);
            _nutrientsText.text += string.Format("<size=25><color=#E5DE1B>{0}</color>: {1} Lvl.\n</size>", skillType.ToString(), GlobalRepository.Skills[skillType]);
        }

        while (kcal.Length < 4)
        {
            kcal = "0" + kcal;
        }

        string water = Mathf.Round(GlobalRepository.Water).ToString();

        while (water.Length < 4)
        {
            water = "0" + water;
        }


        _nutrientsText.text += string.Format("\n\nNutrients\n<size=25><color=#FFA500>{0}/2000</color>kcal    <color=#00E3FF>{1}/2000</color>ml</size>", kcal, water);
        _nutrientsText.text += string.Format("\n<size=25>Happiness: {0}</size>", GlobalRepository.Happiness);

    }
}
