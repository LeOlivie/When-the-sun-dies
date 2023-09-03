using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private ButtonHandler _newGameBtn;
    [SerializeField] private ButtonHandler _continueBtn;
    [SerializeField] private ButtonHandler _exitBtn;
    [SerializeField] private TextMeshProUGUI _continueText;
    [SerializeField] private Saver _saver;

    private void Start()
    {
        _newGameBtn.AddListener(NewGame);
        _exitBtn.AddListener(Exit);

        if (!SaveLoadManager.CheckIfSaveExists("PlayerSave"))
        {
            Color btnColor = _continueBtn.GetComponent<Image>().color;
            _continueBtn.GetComponent<Image>().color = new Color(btnColor.r, btnColor.g, btnColor.b, 0.3f);
            _continueText.color = new Color(_continueText.color.r, _continueText.color.g, _continueText.color.b, 0.3f);
            return;
        }
        _continueBtn.AddListener(Continue);
    }

    private void NewGame()
    {
        SceneManager.LoadScene("DifficultySelector");
    }

    private void Continue()
    {
        _saver.LoadPlayer();
        SceneManager.LoadScene("Base");
    }

    private void Exit()
    {
        Application.Quit();
    }
}
