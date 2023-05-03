using UnityEngine;
using System;

public class ClosedDoorOpener : MonoBehaviour
{
    [Serializable]
    public struct OpeningMethod
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _time;

        public Item Item => _item;
        public int Time => _time;
    }

    [SerializeField] private OpeningMethod[] _openingMethods;
    [SerializeField] private DoorOpenScreenShower _doorMenuShower;
    [SerializeField] private HiddableButtonHandler _interractBtn;
    private DoorOpener[] _doorOpeners;

    private void Start()
    {
        _doorOpeners = this.transform.GetComponentsInChildren<DoorOpener>();

        foreach (DoorOpener doorOpener in _doorOpeners)
        {
            doorOpener.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractBtn.AddListener(OnBtnClicked);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractBtn.RemoveListener(OnBtnClicked);
    }

    private void OnBtnClicked()
    {
        if (_doorMenuShower.gameObject.activeInHierarchy)
        {
            _doorMenuShower.CloseScreen();
        }
        else
        {
            _doorMenuShower.OpenDoorMenu(OnDoorOpened, _openingMethods);
        }
    }

    private void OnDoorOpened()
    {
        foreach (DoorOpener doorOpener in _doorOpeners)
        {
            doorOpener.gameObject.SetActive(true);
        }

        _interractBtn.RemoveListener(OnBtnClicked);
        Destroy(this);
    }
}
