using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathInfoShower : MonoBehaviour
{
    [SerializeField] private string _textToShow;
    [SerializeField] private float _showTime = 5;
    [SerializeField] private ButtonHandler _menuBtn;
    private TextMeshProUGUI _infoText;

    private void Start()
    {
        _infoText = GetComponent<TextMeshProUGUI>();
        _infoText.text = "";
        _menuBtn.gameObject.SetActive(false);
        _menuBtn.AddListener(GoToMenu);
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        foreach (char letter in _textToShow)
        {
            _infoText.text += letter;
            yield return new WaitForSeconds(_showTime / _textToShow.Length);
        }
        _infoText.text += "\n\n<size=80>";

        foreach (char letter in "You perish.")
        {
            _infoText.text += letter;
            yield return new WaitForSeconds(0.45f);
        }
        _infoText.text += "</size>";

        yield return new WaitForSeconds(2f);
        _infoText.text += $"\n<size=30>Days survived {GlobalRepository.SystemVars.GlobalTime / 1440}</size>";
        yield return new WaitForSeconds(2f);
        _menuBtn.gameObject.SetActive(true);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
