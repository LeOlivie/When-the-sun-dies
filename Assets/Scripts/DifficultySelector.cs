using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] private DifficultyData[] _difficultyDatas;
    [SerializeField] private TextMeshProUGUI _diffNameText;
    [SerializeField] private TextMeshProUGUI _diffDescText;
    [SerializeField] private TextMeshProUGUI _diffModsText;
    [SerializeField] private IndexedButtonHandler _backBtn;
    [SerializeField] private IndexedButtonHandler _nextBtn;
    [SerializeField] private ButtonHandler _beginBtn;
    private int _selectedDiffIndex;

    void Start()
    {
        _backBtn.AddListener(ChangeDifficulty);
        _nextBtn.AddListener(ChangeDifficulty);
        _beginBtn.AddListener(Begin);
        ChangeDifficulty(0);
    }

    private void ChangeDifficulty(int change)
    {
        _selectedDiffIndex += change;

        if (_selectedDiffIndex >= _difficultyDatas.Length)
        {
            _selectedDiffIndex = 0;
        }
        else if (_selectedDiffIndex < 0)
        {
            _selectedDiffIndex = _difficultyDatas.Length - 1;
        }

        _diffNameText.text = _difficultyDatas[_selectedDiffIndex].DifficultyName;
        _diffNameText.color = _difficultyDatas[_selectedDiffIndex].DifficultyColor;
        _diffDescText.text = _difficultyDatas[_selectedDiffIndex].DifficultyDescription;

        _diffModsText.text = "Modifiers\n\n";
        _diffModsText.text += _difficultyDatas[_selectedDiffIndex].StartKitDescription + "\n";
        _diffModsText.text += "Day cycle length: " + _difficultyDatas[_selectedDiffIndex].DayCycleLength + " minutes\n";
        _diffModsText.text += $"Safe scavange time: {_difficultyDatas[_selectedDiffIndex].ScavTimeStart}:00-0{_difficultyDatas[_selectedDiffIndex].ScavTimeEnd}:00\n";
        _diffModsText.text += "Sleep time per day: " + _difficultyDatas[_selectedDiffIndex].SleepTimePerDay + " hours\n";
        _diffModsText.text += "Loot ambulance multiplier: x" + _difficultyDatas[_selectedDiffIndex].LootAmbulanceMultiplier + "\n";
        _diffModsText.text += "Loot spawn chance multiplier: x" + _difficultyDatas[_selectedDiffIndex].LootSpawnChanceMultiplier + "\n";
        _diffModsText.text += "Locations are reset every " + _difficultyDatas[_selectedDiffIndex].LocationResetDelay + " days\n";
        _diffModsText.text += "Radio cooldown multiplier: x" + _difficultyDatas[_selectedDiffIndex].RadioCooldownMultiplier + "\n";
        _diffModsText.text += "Max weight: " + _difficultyDatas[_selectedDiffIndex].MaxWeight + "kg\n";
    }
        
    private void Begin()
    {
        GlobalRepository.SystemVars.Difficulty=_difficultyDatas[_selectedDiffIndex];
        SceneManager.LoadScene("Base");
    }
}
