using UnityEngine;
using System.Collections;

public class WeatherSpriteShower : MonoBehaviour
{

    [SerializeField] private GlobalRepository.WeatherTypeEnum _weatherType;
    private bool _isActive = false;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        GlobalRepository.OnTimeUpdated += CheckWeatherChange;
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0);
        this.gameObject.SetActive(false);
    }

    private void CheckWeatherChange()
    {
        if (_weatherType == GlobalRepository.WeatherType && !_isActive)
        {
            this.gameObject.SetActive(true);
            _isActive = true;
            StopAllCoroutines();
            StartCoroutine(Show());
        }
        else if (_weatherType != GlobalRepository.WeatherType && _isActive)
        {
            _isActive = false;
            StopAllCoroutines();
            StartCoroutine(Hide());
        }
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(30f);
        while (_spriteRenderer.color.a < 1)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a + 0.01f);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(60f);
        while (_spriteRenderer.color.a > 0)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a - 0.01f);
            yield return new WaitForSeconds(0.3f);
        }

        this.gameObject.SetActive(false);
    }
}
