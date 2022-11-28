using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemShower : MonoBehaviour
{
    private Item _item;
    [SerializeField] private TextMeshProUGUI _itemCountText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private ButtonHandler _inspectBtn;

    public delegate void InspectDeleg(Item item);
    public InspectDeleg InspectDelegate;

    public Item Item => _item;

    public void ShowItem(Item item)
    {
        this.gameObject.SetActive(true);

        _item = item;

        if (item == null || item.ItemData == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        _itemCountText.text = _item.Count.ToString();
        _iconImage.sprite = _item.Icon;
        _iconImage.SetNativeSize();
        _inspectBtn.AddListener(Inspect);
    }

    public void Inspect()
    {
        InspectDelegate?.Invoke(_item);
    }
}

