using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsController : MonoBehaviour
{
    [SerializeField] private GameObject _thisFloorGO;
    [SerializeField] private GameObject _nextFloorGO;
    [SerializeField] private Transform _newPositionTrans;
    [SerializeField] private HiddableButtonHandler _interractBtn;
    private Transform _playerTrans;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _playerTrans = collision.transform;
            _interractBtn.AddListener(UseStairs);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _interractBtn.RemoveListener(UseStairs);
        }
    }

    private void UseStairs()
    {
        _thisFloorGO.SetActive(false);
        _nextFloorGO.SetActive(true);
        Vector2 movePos = _newPositionTrans.position;
        movePos.y += 2;
        _playerTrans.position = movePos;
    }
}
