using UnityEngine;
using TMPro;

public class HealthScreenShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nutrientsText;


    private void Update()
    {
        string kcal = Mathf.Round(GlobalRepository.Kcal).ToString();

        _nutrientsText.text = "";

        while (kcal.Length < 4)
        {
            kcal = "0" + kcal;
        }

        string water = Mathf.Round(GlobalRepository.Water).ToString();

        while (water.Length < 4)
        {
            water = "0" + water;
        }

        _nutrientsText.text += string.Format("Nutrients\n<size=25><color=#FFA500>{0}/2000</color>kcal    <color=#00E3FF>{1}/2000</color>ml</size=>", kcal, water);
        _nutrientsText.text += string.Format("\nHappiness: {0}", GlobalRepository.Happiness);

    }
}
