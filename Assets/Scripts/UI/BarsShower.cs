using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarsShower : MonoBehaviour
{
    private float _barMaxScale;
    [SerializeField] private Image _kcalBar;
    [SerializeField] private TextMeshProUGUI _kcalText;
    [SerializeField] private Image _waterBar;
    [SerializeField] private TextMeshProUGUI _waterText;

    private void Start()
    {
        _barMaxScale = _kcalBar.transform.localScale.x;
        ShowStats();
        GlobalRepository.OnTimeUpdated += ShowStats;
    }

    private void ShowStats()
    {
        _kcalBar.transform.localScale = new Vector2(_barMaxScale / 2000 * GlobalRepository.Kcal, _kcalBar.transform.localScale.y);
        _waterBar.transform.localScale = new Vector2(_barMaxScale / 2000 * GlobalRepository.Water, _waterBar.transform.localScale.y);
        _kcalText.text = Mathf.RoundToInt(GlobalRepository.Kcal).ToString();
        _waterText.text = Mathf.RoundToInt(GlobalRepository.Water).ToString();
    }
}
