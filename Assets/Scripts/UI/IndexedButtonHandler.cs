using UnityEngine.EventSystems;
using UnityEngine;

public class IndexedButtonHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int _index;
    public delegate void IndexedButtonDelegate(int index);
    protected event IndexedButtonDelegate DoOnPress;

    public int Index => _index;
    public void SetIndex(int index)
    {
        _index = index;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DoOnPress?.Invoke(_index);
    }

    public virtual void AddListener(IndexedButtonDelegate doOnPressMethod)
    {
        DoOnPress += doOnPressMethod;
    }

    public virtual void RemoveListener(IndexedButtonDelegate doOnPressMethod)
    {
        DoOnPress -= doOnPressMethod;
    }
}