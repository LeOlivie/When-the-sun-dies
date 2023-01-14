using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Collections;

public class LootScreenShower : MonoBehaviour, IClosable
{
    public delegate void OnLooted();

    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _lootScr;
    [SerializeField] private GameObject _lootingInProgressScr;
    [SerializeField] private Image _weightBar;
    [SerializeField] private ItemShower[] _inventoryItemShowers;
    [SerializeField] private ItemShower[] _containerItemShowers;
    [SerializeField] private ItemShower _inspectItemShower;
    [SerializeField] private TextMeshProUGUI _weightText;
    [SerializeField] private TextMeshProUGUI _inspectItemDesc;
    [SerializeField] private TextMeshProUGUI _containerNameText;
    [SerializeField] private TextMeshProUGUI _lootingProgressText;
    [SerializeField] private ButtonHandler _useBtn;
    [SerializeField] private ButtonHandler _takeBtn;
    [SerializeField] private ButtonHandler _putBtn;
    [SerializeField] private ButtonHandler _closeBtn;
    private ItemContainer _lootContainer;
    private OnLooted _onLooted;
    private ScreensCloser _screensCloser;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>(true);
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        _takeBtn.AddListener(TakeItem);
        _putBtn.AddListener(PutItem);
    }

    public void OpenLootScreen(ItemContainer lootContainer, string containerName, bool isLooted, OnLooted onLootedDelegate, float lootTime)
    {
        _screensCloser.CloseAllScreens();
        _joystick.enabled = false;
        _lootContainer = lootContainer;
        _containerNameText.text = containerName;
        _lootScr.SetActive(true);
        _closeBtn.AddListener(CloseScreen);
        InspectItem(null);
        ShowInventory();
        _onLooted = onLootedDelegate;

        if (isLooted)
        {
            _lootingInProgressScr.SetActive(false);
        }
        else
        {
            _lootingInProgressScr.SetActive(true);
            StartCoroutine(Loot(lootTime));
        }
    }

    public void CloseScreen()
    {
        _joystick.enabled = true;
        _lootScr.SetActive(false);
        _closeBtn.RemoveListener(CloseScreen);
        StopAllCoroutines();
    }

    private void ShowInventory()
    {
        GlobalRepository.Inventory.ContainerUpdated();
        ShowContainer(_containerItemShowers, _lootContainer);
        ShowContainer(_inventoryItemShowers, GlobalRepository.Inventory);

        GlobalRepository.CountWeight();

        float r = 0.6f - 0.3f / GlobalRepository.MaxWeight * (GlobalRepository.MaxWeight - GlobalRepository.Weight);
        float g = 0.6f - 0.3f / GlobalRepository.MaxWeight * GlobalRepository.Weight;

        _weightBar.color = new Color(r, g, 0.3f);
        _weightBar.rectTransform.localScale = new Vector3(0.216f / GlobalRepository.MaxWeight * GlobalRepository.Weight, 0.216f, 0.216f);

        _weightText.text = string.Format("{0}/{1} KG", GlobalRepository.Weight, GlobalRepository.MaxWeight);
    }

    private void ShowContainer(ItemShower[] itemShowers, ItemContainer itemContainer)
    {
        for (int i = 0; i < itemContainer.Items.Length; i++)
        {
            itemShowers[i].ShowItem(itemContainer.Items[i]);
            itemShowers[i].InspectDelegate = InspectItem;
        }
    }

    private void InspectItem(Item item)
    {

        _inspectItemShower.ShowItem(item);

        _inspectItemDesc.text = "";
        _useBtn.RemoveListener(null);

        if (item != null && item.ItemData != null)
        {
            _inspectItemDesc.text = item.Name + "\n" + item.Description + "\n" + item.AdditionalInfo;
            _useBtn.AddListener(Use);
            _useBtn.gameObject.SetActive(item.ItemData.IsUsable);
        }
    }

    private void TakeItem()
    {
        if (_inspectItemShower.Item == null || _inspectItemShower.Item.ItemData == null)
        {
            return;
        }

            Item itemToAdd = new Item(_inspectItemShower.Item.ItemData, 1);

        if (GlobalRepository.Inventory.CheckIfCanFit(itemToAdd) && _lootContainer.Items.Contains(_inspectItemShower.Item))
        {
            GlobalRepository.Inventory.AddItem(itemToAdd, true);
            _inspectItemShower.Item.AddCount(-1);
            InspectItem(_inspectItemShower.Item);
            ShowInventory();
        }
    }

    private void PutItem()
    {
        if (_inspectItemShower.Item == null || _inspectItemShower.Item.ItemData == null)
        {
            return;
        }

        Item itemToAdd = new Item(_inspectItemShower.Item.ItemData, 1);

        if (_lootContainer.CheckIfCanFit(itemToAdd) && GlobalRepository.Inventory.Items.Contains(_inspectItemShower.Item))
        {
            _lootContainer.AddItem(itemToAdd, true);
            _inspectItemShower.Item.AddCount(-1);
            InspectItem(_inspectItemShower.Item);
            ShowInventory();
        }
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

    IEnumerator Loot(float lootTime)
    {
        float currTime = 0;
        while (currTime < lootTime)
        {
            int progress = Mathf.RoundToInt(currTime / lootTime * 100);
            _lootingProgressText.text = "Looting... ";

            if (progress < 100)
            {
                _lootingProgressText.text += "0";
            }
            if (progress < 10)
            {
                _lootingProgressText.text += "0";
            }

            _lootingProgressText.text += progress + "%";

            yield return new WaitForSeconds(lootTime/100);
            currTime += 0.1f;
        }
        _lootingInProgressScr.SetActive(false);
        _onLooted();
    }
}
