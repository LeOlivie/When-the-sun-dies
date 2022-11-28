using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxAnimationSpeed;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    private List<int> _wallSortOrders = new List<int>();

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_joystick.Horizontal * _speed, _joystick.Vertical * _speed);
        _animator.SetFloat("DirectionX", _joystick.Horizontal);
        _animator.SetFloat("DirectionY", _joystick.Vertical);
        _animator.speed = (Mathf.Abs(_joystick.Horizontal) + Mathf.Abs(_joystick.Vertical)) * _maxAnimationSpeed;
    }

    public void AddSortOrder(int order)
    {
        _wallSortOrders.Add(order);
        ChangeSortOrder();
    }

    public void RemoveSortOrder(int order)
    {
        _wallSortOrders.Remove(order);
        ChangeSortOrder();
    }

    private void ChangeSortOrder()
    {
        if (_wallSortOrders.Count > 0)
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = _wallSortOrders.Min();
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 1000;
        }
    }
}
