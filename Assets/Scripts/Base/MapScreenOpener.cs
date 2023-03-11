using UnityEngine;

public class MapScreenOpener : MonoBehaviour
{
    [SerializeField] private ButtonHandler _interractBtn;
    [SerializeField] private MapScreenShower _mapScreen;

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
        _interractBtn.RemoveListener(OnBtnClicked);
    }

    private void OnBtnClicked()
    {
        if (_mapScreen.gameObject.activeInHierarchy)
        {
            _mapScreen.CloseScreen();
        }
        else
        {
            _mapScreen.OpenScreen();
        }
    }
}
