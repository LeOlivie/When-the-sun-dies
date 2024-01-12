using UnityEngine;
using TMPro;

public class DoorOpenScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _doorOpeningScreen;
    [SerializeField] private ItemShower _openToolItemShower;
    [SerializeField] private TextMeshProUGUI _openToolNameText;
    [SerializeField] private TextMeshProUGUI _timeRequieredText;
    [SerializeField] private TextMeshProUGUI _openMethodIndexText;
    [SerializeField] private TextMeshProUGUI _openingTimeLeftText;
    [SerializeField] private ButtonHandler _openDoorBtn;
    [SerializeField] private ButtonHandler _cancelOpeningBtn;
    [SerializeField] private IndexedButtonHandler _prevOpenMethodBtn;
    [SerializeField] private IndexedButtonHandler _nextOpenMethodBtn;
    [SerializeField] private Color _availableColor;
    [SerializeField] private Color _unavailableColor;

    public delegate void OnDoorOpenedDelegate();
    private OnDoorOpenedDelegate _onDoorOpened;
    private ScreensCloser _screensCloser;
    private ClosedDoorOpener.OpeningMethod[] _openingMethods;
    private int _openMethodIndex;
    private int _openingTimeLeft;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        this.gameObject.SetActive(false);
        _prevOpenMethodBtn.SetIndex(-1);
        _nextOpenMethodBtn.SetIndex(1);
        _cancelOpeningBtn.AddListener(CancelOpening);
    }

    public void OpenDoorMenu(OnDoorOpenedDelegate onDoorOpened, ClosedDoorOpener.OpeningMethod[] openingMethods)
    {
        _screensCloser.CloseAllScreens();
        this.gameObject.SetActive(true);
        _joystick.enabled = false;
        _doorOpeningScreen.SetActive(false);
        _openingMethods = openingMethods;
        _onDoorOpened = onDoorOpened;
        _prevOpenMethodBtn.AddListener(ChangeOpenMethod);
        _nextOpenMethodBtn.AddListener(ChangeOpenMethod);
        _openMethodIndex = 0;
        ChangeOpenMethod(0);
    }

    public void CloseScreen()
    {
        _joystick.enabled = true;
        this.gameObject.SetActive(false);
        _prevOpenMethodBtn.RemoveListener(ChangeOpenMethod);
        _nextOpenMethodBtn.RemoveListener(ChangeOpenMethod); 
        CancelOpening();
    }

    private void ChangeOpenMethod(int change)
    {
        _openMethodIndex += change;
        _openDoorBtn.RemoveListener(StartOpeningDoor);
        _openDoorBtn.transform.GetComponentInChildren<TextMeshProUGUI>().color = _unavailableColor;

        if (_openMethodIndex >= _openingMethods.Length)
        {
            _openMethodIndex = 0;
        }
        else if (_openMethodIndex < 0)
        {
            _openMethodIndex = _openingMethods.Length - 1;
        }
        _openMethodIndexText.text = $"{_openMethodIndex+1}/{_openingMethods.Length}";
        _openToolItemShower.ShowItem(_openingMethods[_openMethodIndex].Item);
        _openToolNameText.text = _openingMethods[_openMethodIndex].Item.Name;
        _timeRequieredText.text = TimeConverter.InsertTime("{0}:{1}", _openingMethods[_openMethodIndex].Time, TimeConverter.InsertionType.HourMinute);

        if (GlobalRepository.PlayerVars.Inventory.CheckIfHas(_openingMethods[_openMethodIndex].Item.ItemData, _openingMethods[_openMethodIndex].Item.Count))
        {
            _openDoorBtn.AddListener(StartOpeningDoor);
            _openDoorBtn.transform.GetComponentInChildren<TextMeshProUGUI>().color = _availableColor;
        }
    }

    private void StartOpeningDoor()
    {
        GlobalRepository.OnTimeUpdated += OpenProgress;
        _doorOpeningScreen.SetActive(true);
        _openingTimeLeft = _openingMethods[_openMethodIndex].Time;
        Time.timeScale = 5;

    }

    private void OpenProgress()
    {
        _openingTimeLeft--;
        _openingTimeLeftText.text = TimeConverter.InsertTime("{0}:{1}",_openingTimeLeft,TimeConverter.InsertionType.HourMinute);

        if(_openingTimeLeft <= 0)
        {
            OpeningEnd();
        }
    }

    private void OpeningEnd()
    {
        GlobalRepository.PlayerVars.Inventory.RemoveItem(_openingMethods[_openMethodIndex].Item, _openingMethods[_openMethodIndex].Item.Count);
        GlobalRepository.OnTimeUpdated -= OpenProgress;
        _onDoorOpened();
        CloseScreen();
    }

    private void CancelOpening()
    {
        Time.timeScale = 1;
        GlobalRepository.OnTimeUpdated -= OpenProgress;
        _doorOpeningScreen.SetActive(false);
    }
}
