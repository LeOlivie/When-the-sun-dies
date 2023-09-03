using System.Collections;
using UnityEngine;
using TMPro;

public class GameNameShower : MonoBehaviour
{
    [SerializeField] private string _gameName;
    [SerializeField] private TextMeshProUGUI _gameNameText;
    [SerializeField] private TextMeshProUGUI _gameVersionText;

    void Start()
    {
        _gameVersionText.text = "version " + Application.version;
        StartCoroutine(ShowName());
    }

    private IEnumerator ShowName()
    {
        foreach (char letter in _gameName)
        {
            _gameNameText.text += letter;
            yield return new WaitForSeconds(0.07f);
        }
    }

}
