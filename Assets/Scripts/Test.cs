using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Item[] _items;

    private void Start()
    {
        foreach (Item item in _items)
        {
            GlobalRepository.Inventory.AddItem(item,false);
        }
    }
}
