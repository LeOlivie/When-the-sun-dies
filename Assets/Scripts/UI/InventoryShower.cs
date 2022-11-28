using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryShower : MonoBehaviour
{
    [SerializeField] private ButtonHandler _sortBtn;
    [SerializeField] private ButtonHandler _useBtn;
    [SerializeField] private ItemShower _inspectItemShower;
    [SerializeField] private TextMeshProUGUI _itemDescText;
    [SerializeField] private TextMeshProUGUI _weightText;
    [SerializeField] private Image _weightBar;
    [SerializeField] private int _nameSize;
    [SerializeField] private ItemShower[] _itemShowers;

    private void Start()
    {
        ShowInventory();
        _sortBtn.AddListener(SortInventory);
        _useBtn.AddListener(Use);
        GlobalRepository.Inventory.ContainerUpdated += ShowInventory;
    }

    private void ShowInventory()
    {
        for (int i = 0; i < GlobalRepository.Inventory.Items.Length; i++)
        {
            _itemShowers[i].ShowItem(GlobalRepository.Inventory.Items[i]);
            _itemShowers[i].InspectDelegate = InspectItem;
        }
        GlobalRepository.CountWeight();

        float r = 0.6f - 0.3f / GlobalRepository.MaxWeight * (GlobalRepository.MaxWeight - GlobalRepository.Weight);
        float g = 0.6f - 0.3f / GlobalRepository.MaxWeight * GlobalRepository.Weight;

        _weightBar.color = new Color(r, g, 0.3f);
        _weightBar.rectTransform.localScale = new Vector3(0.216f / GlobalRepository.MaxWeight * GlobalRepository.Weight, 0.216f, 0.216f);

        _weightText.text = string.Format("{0}/{1} KG", GlobalRepository.Weight, GlobalRepository.MaxWeight);
    }

    private void SortInventory()
    {
        GlobalRepository.Inventory.Sort();
    }

    private void InspectItem(Item item)
    {
        _inspectItemShower.ShowItem(null);
        _itemDescText.text = string.Format("");
        _useBtn.gameObject.SetActive(false);

        if (item == null || item.ItemData == null)
        {
            return;
        }

        _inspectItemShower.ShowItem(item);

        _itemDescText.text = string.Format("<size={0}><b>{1} ({2}/{3})</b></size>", _nameSize, item.Name, item.Count, item.MaxInStack);
        _itemDescText.text += "\n\n" + item.Description;
        _itemDescText.text += "\n\n" + item.AdditionalInfo;
        _useBtn.gameObject.SetActive(item.ItemData.IsUsable);
    }

    private void Use()
    {
        if (_inspectItemShower.Item == null || _inspectItemShower.Item.ItemData == null)
        {
            return;
        }

        _inspectItemShower.Item.Use();
        ShowInventory();
        InspectItem(_inspectItemShower.Item);
    }
}
