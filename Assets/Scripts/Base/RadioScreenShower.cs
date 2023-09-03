using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadioScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _radioScreen;
    [SerializeField] private TextMeshProUGUI _radioContactText;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private Upgrader _upgrader;
    [SerializeField] private GameObject _upgradeBG;

    private Radio _radio;
    private ScreensCloser _screensCloser;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        this.gameObject.SetActive(false);
    }

    public void OpenRadioScreen(Radio radio)
    {
        _screensCloser.CloseAllScreens();
        _radioScreen.SetActive(radio.CurrLevel == 1);
        _radio = radio;
        this.gameObject.SetActive(true);
        _joystick.enabled = false;
        _upgrader.ShowUpgradeMenu(_radio, OpenRadioMenu);
        GlobalRepository.CountWeight();

        if (_radio.CurrLevel == 1)
        {
            OpenRadioMenu();
        }
        else
        {
            _upgradeBG.SetActive(true);
            OpenUpgradesMenu();
        }
    }

    public void CloseScreen()
    {
        _upgrader.CloseUpgradeMenu();
        _joystick.enabled = true;
        this.gameObject.SetActive(false);
    }

    private void OpenRadioMenu()
    {
        _upgradeBG.SetActive(false);
        _radioScreen.SetActive(_radio.CurrLevel == 1);
        _upgradesMenu.SetActive(false);
        _radio.CheckEvent();

        if (_radio.ActiveEventLocCluster != null)
        {
            _radioContactText.text = _radio.ActiveEventLocCluster.RadioText;
        }
        else
        {
            _radioContactText.text = "No radiosignals received.";
        }
    }

    private void OpenUpgradesMenu()
    {
        _upgradesMenu.SetActive(true);
        _upgrader.ShowUpgradeMenu(_radio, OpenRadioMenu);
    }
}
