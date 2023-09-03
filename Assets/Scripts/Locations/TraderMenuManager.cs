using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderMenuManager : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private QuestsAccepter _questsAccepter;
    [SerializeField] private TradingScreenShower _tradingScreenShower;
    [SerializeField] private ButtonHandler _questsBtn;
    [SerializeField] private ButtonHandler _tradingBtn;
    private ItemContainer _traderContainer;
    
    private void Start()
    {
        _questsBtn.AddListener(ShowQuests);
        _tradingBtn.AddListener(ShowTrading);
        this.gameObject.SetActive(false);
    }

    public void OpenTraderMenu(ItemContainer traderContainer)
    {
        this.gameObject.SetActive(true);
        _joystick.enabled = false;
        _traderContainer = traderContainer;
        ShowTrading();
    }

    public void ShowTrading()
    {
        _tradingScreenShower.OpenTradingScreen(_traderContainer);
        _questsAccepter.CloseScreen();
    }

    private void ShowQuests()
    {
        _tradingScreenShower.CloseScreen();
        _questsAccepter.gameObject.SetActive(true);
    }

    public void CloseScreen()
    {
        _joystick.enabled = true;
        _tradingScreenShower.CloseScreen();
        _questsAccepter.CloseScreen();
        this.gameObject.SetActive(false);
    }
}
