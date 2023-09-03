using UnityEngine;
using TMPro;

public class ReadScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private ItemShower _iconShower;
    [SerializeField] private TextMeshProUGUI _bookName;
    [SerializeField] private TextMeshProUGUI _additionalInfo;
    [SerializeField] private TextMeshProUGUI _startBtnText;
    [SerializeField] private ButtonHandler _startBtnHandler;
    [SerializeField] private ButtonHandler _closeBtnHandler;
    [SerializeField] private GameObject _menu;
    private int _timeLeft;
    private BookData _bookData;
    private ScreensCloser _screensCloser;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        this.gameObject.SetActive(false);
    }

    public void OpenScreen(BookData bookData)
    {
        _screensCloser.CloseAllScreens();
        this.gameObject.SetActive(true);
        _bookData = bookData;
        _iconShower.ShowItem(new Item(bookData, 1));
        _bookName.text = bookData.Name;
        _startBtnText.text = "Start";
        _timeLeft = bookData.TimeToRead;
        _startBtnHandler.AddListener(StartReading);
        _closeBtnHandler.AddListener(CloseScreen);
        ShowTime();
    }

    private void StartReading()
    {
        _startBtnHandler.RemoveListener(StartReading);
        _startBtnHandler.AddListener(PauseReading);
        _startBtnText.text = "Pause";
        GlobalRepository.OnTimeUpdated += ReadProgress;
        Time.timeScale = 40 * GlobalRepository.Difficulty.DayCycleLength / 24;
    }

    private void ReadProgress()
    {
        _timeLeft -= 1;

        if (_timeLeft <= 0)
        {
            _startBtnHandler.RemoveListener(PauseReading);
            _startBtnHandler.RemoveListener(StartReading);
            _startBtnText.text = "---";
            GlobalRepository.OnTimeUpdated -= ReadProgress;
            Time.timeScale = 1;
        }

        ShowTime();
    }

    private void PauseReading()
    {
        _startBtnHandler.RemoveListener(PauseReading);
        _startBtnHandler.AddListener(StartReading);
        _startBtnText.text = "Start";
        GlobalRepository.OnTimeUpdated -= ReadProgress;
        Time.timeScale = 1;
    }

    public void CloseScreen()
    {
        if (_bookData != null)
        {
            if (_timeLeft <= 0)
            {
                GlobalRepository.Skills[_bookData.SkillType] += 1;
            }

            if (_timeLeft > 0)
            {
                GlobalRepository.Inventory.AddItem(new Item(_bookData, 1), false);
            }

            _menu.SetActive(true);
            GlobalRepository.OnTimeUpdated -= ReadProgress;
            _bookData = null;
        }

        _startBtnHandler.RemoveListener(PauseReading);
        _startBtnHandler.RemoveListener(StartReading);
        _closeBtnHandler.RemoveListener(CloseScreen);
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    private void ShowTime()
    {
        string hrsToRead = (_timeLeft / 60).ToString();
        string minsToRead = (_timeLeft - _timeLeft / 60 * 60).ToString();

        while (hrsToRead.Length <= 1)
        {
            hrsToRead = "0" + hrsToRead;
        }

        while (minsToRead.Length <= 1)
        {
            minsToRead = "0" + minsToRead;
        }

        _additionalInfo.text = string.Format("Time to read left:\n<size=40>{0}:{1}</size>\n", hrsToRead, minsToRead);
    }
}
