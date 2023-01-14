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
    [SerializeField] private TextMeshProUGUI _stationInfo;
    private IUpgradable _upgradableObject;

    public delegate void UpgradeEndedDelegate();
    private UpgradeEndedDelegate OnUpgradeEnded;

    public void ShowUpgradeMenu(IUpgradable upgradableObject, UpgradeEndedDelegate upgradeEndedDelegate)
    {
        _upgradableObject = upgradableObject;
        OnUpgradeEnded = upgradeEndedDelegate;
        _upgradeBtn.RemoveListener(StartUpgrade);

        if (_upgradableObject.IsBeingUpgraded)
        {
            _upgradeInProgressScr.SetActive(true);
            _upgradeTimeLeftText.text = _upgradableObject.UpgradeTimeLeft + " minutes";
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
        _upgradableObject = null;
        OnUpgradeEnded = null;
        _upgradeBtn.RemoveListener(StartUpgrade);
    }

    private void ShowUpgradeRequieremnts()
    {
        _stationInfo.text = string.Format("{0}\nLevel: {1}/{2}", _upgradableObject.ObjectName, _upgradableObject.CurrLevel, _upgradableObject.MaxLevel);

        for (int i = 0; i < _upgradeRequirementShowers.Length; i++)
        {
            if (_upgradableObject.UpgradeRequirements.Length <= i)
            {
                _upgradeRequirementShowers[i].ShowItem(null);
                continue;
            }

            _upgradeRequirementShowers[i].ShowItem(_upgradableObject.UpgradeRequirements[i]);
        }

        _upgradeTimeRequieredText.text = _upgradableObject.UpgradeTimeRequired + " minutes";

        foreach (Item item in _upgradableObject.UpgradeRequirements)
        {
            if (!GlobalRepository.Inventory.CheckIfHas(item.ItemData, item.Count))
            {
                _upgradeBtn.RemoveListener(StartUpgrade);
                return;
            }
        }

        if (_upgradableObject.CurrLevel >= _upgradableObject.MaxLevel)
        {
            _upgradeBtn.RemoveListener(StartUpgrade);
            return;
        }
        
        _upgradeBtn.AddListener(StartUpgrade);
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
        _pauseUpgradeBtn.RemoveListener(ResumeUpgrade);
        _pauseUpgradeBtn.AddListener(PauseUpgrade);
        GlobalRepository.OnTimeUpdated += UpgradeInProgress;
        Time.timeScale = 40;
    }

    public void PauseUpgrade()
    {
        _pauseUpgradeBtn.RemoveListener(PauseUpgrade);
        _pauseUpgradeBtn.AddListener(ResumeUpgrade);
        GlobalRepository.OnTimeUpdated -= UpgradeInProgress;
        Time.timeScale = 1;
    }

    private void UpgradeInProgress()
    {
        _upgradeTimeLeftText.text = _upgradableObject.UpgradeTimeLeft + " minutes";
        _upgradableObject.DecreaseUpgradeTimeLeft();

        if (_upgradableObject.UpgradeTimeLeft <= 0)
        {
            Debug.Log("Aaa");
            PauseUpgrade();
            _upgradableObject.OnUpgradeEnded();
            //_pauseUpgradeBtn.RemoveListener(PauseUpgrade);
            // _pauseUpgradeBtn.RemoveListener(ResumeUpgrade);
            _upgradeInProgressScr.SetActive(false);
            OnUpgradeEnded?.Invoke();
        }
    }
}