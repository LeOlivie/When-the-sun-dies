using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private Transform _anotherEnterTrans;
    [SerializeField] private ButtonHandler _interractBtn;
    private Transform _playerTrans;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _playerTrans = collision.transform;
            _interractBtn.AddListener(OpenDoor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _interractBtn.RemoveListener(OpenDoor);
        }
    }

    private void OpenDoor()
    {
        Vector2 movePos = _anotherEnterTrans.position;
        movePos.y += 2;
        _playerTrans.position = movePos;
    }
}
