using UnityEngine;
using UnityEngine.UI;

public class HiddableButtonHandler : ButtonHandler
{
    [SerializeField] private Image _imageComponent;
    [SerializeField] private int _subscribersCount;

    private void Awake()
    {
        _imageComponent = this.GetComponent<Image>();
    }


    public override void AddListener(ButtonDelegate doOnPressMethod)
    {
        base.AddListener(doOnPressMethod);
        _imageComponent.enabled = true;
        _subscribersCount++;
    }

    public override void RemoveListener(ButtonDelegate doOnPressMethod)
    {
        base.RemoveListener(doOnPressMethod);
        _subscribersCount--;

        if (_subscribersCount <= 0)
        {
            _imageComponent.enabled = false;
        }
    }
}
