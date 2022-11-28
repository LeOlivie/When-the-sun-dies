using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _playerTrans;
    private Transform _cameraTrans;

    private void Start()
    {
        _cameraTrans = this.GetComponent<Transform>();
    }

    void Update()
    {
        _cameraTrans.localPosition = _playerTrans.localPosition;
    }
}
