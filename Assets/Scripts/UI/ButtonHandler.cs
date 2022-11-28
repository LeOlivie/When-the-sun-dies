using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler
{
    public delegate void ButtonDelegate();
    protected event ButtonDelegate DoOnPress;
    private int _listenerCount;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DoOnPress != null)
        {
            DoOnPress();
        }
    }

    public virtual void AddListener(ButtonDelegate doOnPressMethod)
    {
        DoOnPress += doOnPressMethod;
    }

    public virtual void RemoveListener(ButtonDelegate doOnPressMethod)
    {
        DoOnPress -= doOnPressMethod;
    }
}