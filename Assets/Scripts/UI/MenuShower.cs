using System;
using UnityEngine;

public class MenuShower : MonoBehaviour, IClosable
{
    [Serializable]
    private struct Screen
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private IndexedButtonHandler _buttonHandler;

        public GameObject GameObject => _gameObject;
        public IndexedButtonHandler ButtonHandler => _buttonHandler;
    }

    [SerializeField] private Joystick _joystick;
    [SerializeField] private ButtonHandler _openBtn;
    [SerializeField] private GameObject _menu;
    [SerializeField] private Screen[] _screens;
    private ScreensCloser _screensCloser;
    
    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        _openBtn.AddListener(OpenMenu);
        _openBtn.RemoveListener(CloseScreen);

        for (int i = 0; i < _screens.Length; i++)
        {
            _screens[i].ButtonHandler.SetIndex(i);
            _screens[i].ButtonHandler.AddListener(SelectScreen);
        }
    }

    private void OpenMenu()
    {
        _screensCloser.CloseAllScreens();
        _openBtn.RemoveListener(OpenMenu);
        _openBtn.AddListener(CloseScreen);
        _joystick.enabled = false;
        _menu.SetActive(true);
        SelectScreen(0);
    }

    public void CloseScreen()
    {
        _openBtn.AddListener(OpenMenu);
        _openBtn.RemoveListener(CloseScreen);
        _joystick.enabled = true;
        _menu.SetActive(false);
    }

    private void SelectScreen(int i)
    {
        foreach (Screen screen in _screens)
        {
            screen.GameObject.SetActive(false);
        }

        _screens[i].GameObject.SetActive(true);
    }
}
