using UnityEngine;
using TMPro;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private GameObject _upgradeInProgressScr;
    [SerializeField] private ItemShower[] _upgradeRequirementShowers;
    [SerializeField] private ButtonHandler _pauseUpgradeBtn;
    [SerializeField] private ButtonHandler _upgradeBtn;
    [SerializeField] private TextMeshProUGUI _upgradeTimeLeftText;
    [SerializeField] private TextMeshProUGUI _upgradeTimeRequieredText;
    [SerializeField] private TextMeshProUGUI _skillRequirementsText;
    [SerializeField] private TextMeshProUGUI _stationInfo;
    [SerializeField] private Color _unavailableColor;
    [SerializeField] private Color _availableColor;
    private IUpgradable _upgradableObject;

    public delegate void UpgradeEndedDelegate();
    private UpgradeEndedDelegate OnUpgradeEnded;

    public void ShowUpgradeMenu(IUpgradable upgradableObject, UpgradeEndedDelegate upgradeEndedDelegate)
    {
        _upgradableObject = upgradableObject;
        OnUpgradeEnded = upgradeEndedDelegate;
        _upgradeBtn.RemoveListener(StartUpgrade);
        _pauseUpgradeBtn.ResetListeners();

        if (_upgradableObject.IsBeingUpgraded)
        {
            _upgradeInProgressScr.SetActive(true);
            _upgradeTimeLeftText.text = TimeConverter.InsertTime("{0}:{1}", _upgradableObject.UpgradeTimeLeft,TimeConverter.InsertionType.HourMinute);
            _pauseUpgradeBtn.AddListener(ResumeUpgrade);
        }
        else
        {
            _upgradeInProgressScr.SetActive(false);
            ShowUpgradeRequieremnts();
        }
    }

    public void CloseUpgradeMenu()
    {
        GlobalRepository.OnTimeUpdated -= UpgradeInProgress;
        Time.timeScale = 1;
        _upgradableObject = null;
        OnUpgradeEnded = null;
        _upgradeBtn.RemoveListener(StartUpgrade);
        _pauseUpgradeBtn.ResetListeners();
    }

    private void ShowUpgradeRequieremnts()
    {
        _stationInfo.text = string.Format("{0}\nLevel: {1}/{2}", _upgradableObject.ObjectName, _upgradableObject.CurrLevel, _upgradableObject.MaxLevel);
        _skillRequirementsText.text = "";

        for (int i = 0; i < _upgradeRequirementShowers.Length; i++)
        {
            if (_upgradableObject.UpgradeRequirements.Length <= i)
            {
                _upgradeRequirementShowers[i].ShowItem(null);
                continue;
            }

            _upgradeRequirementShowers[i].ShowItem(_upgradableObject.UpgradeRequirements[i]);
        }

        if (_upgradableObject.SkillRequirements.Length > 0)
        {
            _skillRequirementsText.text += "Skill requirements\n";
        }

        for (int i = 0; i < _upgradableObject.SkillRequirements.Length; i++)
        {
            _skillRequirementsText.text += $"<color=#E5DE1B>{_upgradableObject.SkillRequirements[i].SkillType}</color>: {_upgradableObject.SkillRequirements[i].SkillLevel} Lvl\n";
        }

        _upgradeTimeRequieredText.text = TimeConverter.InsertTime("{0}:{1}", _upgradableObject.UpgradeTimeRequired,TimeConverter.InsertionType.HourMinute);
        
        foreach (Item item in _upgradableObject.UpgradeRequirements)
        {
            if (!GlobalRepository.Inventory.CheckIfHas(item.ItemData, item.Count))
            {
                _upgradeBtn.RemoveListener(StartUpgrade);
                _upgradeBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = _unavailableColor;
                return;
            }
        }


        for (int i = 0; i < _upgradableObject.SkillRequirements.Length; i++)
        {
            if (GlobalRepository.Skills[_upgradableObject.SkillRequirements[i].SkillType] < _upgradableObject.SkillRequirements[i].SkillLevel)
            {
                _upgradeBtn.RemoveListener(StartUpgrade);
                _upgradeBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = _unavailableColor;
                return;
            }
        }

        if (_upgradableObject.CurrLevel >= _upgradableObject.MaxLevel)
        {
            _upgradeBtn.RemoveListener(StartUpgrade);
            _upgradeBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = _unavailableColor;
            return;
        }
        
        _upgradeBtn.AddListener(StartUpgrade);
        _upgradeBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = _availableColor;
    }

    public void StartUpgrade()
    {
        _upgradableObject.OnUpgradeStarted();
        _upgradeInProgressScr.SetActive(true);

        foreach (Item inputItem in _upgradableObject.UpgradeRequirements)
        {
            GlobalRepository.Inventory.RemoveItem(inputItem, inputItem.Count);
        }   

        ResumeUpgrade();
    }

    public void ResumeUpgrade()
    {
        GlobalRepository.OnTimeUpdated += UpgradeInProgress;
        Time.timeScale = 40 * GlobalRepository.Difficulty.DayCycleLength / 24;
        _pauseUpgradeBtn.RemoveListener(ResumeUpgrade);
        _pauseUpgradeBtn.AddListener(PauseUpgrade);
    }

    public void PauseUpgrade()
    {
        GlobalRepository.OnTimeUpdated -= UpgradeInProgress;
        Time.timeScale = 1;
        _pauseUpgradeBtn.RemoveListener(PauseUpgrade);
        _pauseUpgradeBtn.AddListener(ResumeUpgrade);
    }

    private void UpgradeInProgress()
    {
        _upgradeTimeLeftText.text = TimeConverter.InsertTime("{0}:{1}", _upgradableObject.UpgradeTimeLeft, TimeConverter.InsertionType.HourMinute);

        _upgradableObject.DecreaseUpgradeTimeLeft();

        if (_upgradableObject.UpgradeTimeLeft <= 0)
        {
            PauseUpgrade();
            _upgradableObject.OnUpgradeEnded();
            _upgradeInProgressScr.SetActive(false);
            OnUpgradeEnded?.Invoke();
        }
    }
}