using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeatherUIShower : MonoBehaviour
{
    [SerializeField] private GlobalRepository.WeatherTypeEnum _weatherType;
    private bool _isActive = false;
    private Image _image;

    private void Start()
    {
        _image = this.GetComponent<Image>();
        GlobalRepository.OnTimeUpdated += CheckWeatherChange;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
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
        while (_image.color.a < 1)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a + 0.01f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Hide()
    {
        while (_image.color.a > 0)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a - 0.01f);
            yield return new WaitForSeconds(0.2f);
        }
        this.gameObject.SetActive(false);
    }
}
