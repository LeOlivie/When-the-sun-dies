using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

public class TradingScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private ItemShower[] _traderItemShowers;
    [SerializeField] private SellItemShower[] _tableItemShowers;
    [SerializeField] private ItemShower[] _playerItemShowers;
    [SerializeField] private ButtonHandler _addOrRemoveBtn;
    [SerializeField] private ButtonHandler _tradeBtn;
    [SerializeField] private ItemShower _inspectItemShower;
    [SerializeField] private TextMeshProUGUI _itemInfoText;
    [SerializeField] private TextMeshProUGUI _addOrRemoveBtnText;
    [SerializeField] private TextMeshProUGUI _valuesText;
    [SerializeField] private TextMeshProUGUI _tradeBtnText;
    private ItemContainer _traderContainer;
    private ItemContainer _buyContainer = new ItemContainer(12);
    private ItemContainer _sellContainer = new ItemContainer(12);
    private ItemContainer _playerContainer;
    public int _kcalPrice = 0;
    public int _mlPrice = 0;

    private void Start()
    {
        this.gameObject.SetActive(false);
        _addOrRemoveBtn.AddListener(AddItem);
        _tradeBtn.AddListener(Trade);
        GameObject.FindObjectOfType<RaidEnder>().RaidEnded += CloseScreen;
    }

    public void OpenTradingScreen(ItemContainer traderContainer)
    {
        this.gameObject.SetActive(true);
        _traderContainer = traderContainer;
        _playerContainer = GlobalRepository.Inventory;
        UpdateItemShowers();

        foreach (ItemShower itemShower in _traderItemShowers)
        {
            itemShower.InspectDelegate = ShowItemInfo;
        }

        foreach (ItemShower itemShower in _playerItemShowers)
        {
            itemShower.InspectDelegate = ShowItemInfo;
        }

        foreach (SellItemShower sellItemShower in _tableItemShowers)
        {
            sellItemShower.InspectDelegate = ShowItemInfo;
        }
    }

    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
        foreach (Item item in _sellContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                _playerContainer.AddItem(item, true);
            }
        }

        foreach (Item item in _buyContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                _traderContainer.AddItem(item, true);
            }
        }

        _sellContainer = new ItemContainer(12);
        _buyContainer = new ItemContainer(12);
    }

    private void UpdateItemShowers()
    {
        UpdateItemContainer(_traderContainer, _traderItemShowers);
        UpdateItemContainer(_playerContainer, _playerItemShowers);
        ShowTradingTable();
    }

    private void UpdateItemContainer(ItemContainer itemContainer, ItemShower[] itemShowers)
    {
        for (int i = 0; i < itemContainer.Items.Length;i++)
        {
            itemShowers[i].ShowItem(itemContainer.Items[i]);
        }
    }

    private void ShowTradingTable()
    {
        int itemIndex = 0;

        foreach (SellItemShower sellItemShower in _tableItemShowers)
        {
            sellItemShower.ShowSellableItem(new Item(), "");
        }

        foreach (Item item in _buyContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                _tableItemShowers[itemIndex].ShowSellableItem(item, "Buy");
                itemIndex++;
            }
        }

        foreach (Item item in _sellContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                _tableItemShowers[itemIndex].ShowSellableItem(item, "Sell");
                itemIndex++;
            }
        }

        _kcalPrice = 0;
        _mlPrice = 0;

        foreach (Item item in _buyContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                _kcalPrice -= item.ItemData.KcalPrice * item.Count;
                _mlPrice -= item.ItemData.MLPrice * item.Count;
            }
        }

        foreach (Item item in _sellContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                _kcalPrice += item.ItemData.KcalPrice * item.Count;
                _mlPrice += item.ItemData.MLPrice * item.Count;
            }
        }

        _valuesText.text = "VALUES";

        if (_kcalPrice > 0)
        {
            _valuesText.text += $"\nFood: +{_kcalPrice} KCal";
        }
        else
        {
            _valuesText.text += $"\nFood: {_kcalPrice} KCal";
        }

        if (_mlPrice > 0)
        {
            _valuesText.text += $"\nWater: +{_mlPrice} ml";
        }
        else
        {
            _valuesText.text += $"\nWater: {_mlPrice} ml";
        }

        if (_mlPrice >= 0 && _kcalPrice >= 0)
        {
            _tradeBtn.enabled = true;
            _tradeBtnText.color = new Color(0.7f, 0.85f, 0.7f, 1);
        }
        else
        {
            _tradeBtn.enabled = false;
            _tradeBtnText.color = new Color(0.77f, 0.55f, 0.55f, 1);
        }
    }

    private void ShowItemInfo(Item item)
    {
        if (item == null || item.ItemData == null || item.Count <= 0)
        {
            _inspectItemShower.ShowItem(new Item());
            _itemInfoText.text = "";
            return;
        }

        _inspectItemShower.ShowItem(item);
        _itemInfoText.text = $"{item.Name}\nKCal price: {item.ItemData.KcalPrice}\nML price: {item.ItemData.MLPrice}";

        if (_buyContainer.Items.Contains(item) || _sellContainer.Items.Contains(item))
        {
            _addOrRemoveBtnText.text = "Remove";
        }
        else
        {
            _addOrRemoveBtnText.text = "Add";
        }
    }

    private bool CheckForTableOverflow()
    {
        int itemIndex = 0;

        foreach (Item item in _buyContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                itemIndex++;
            }
        }

        foreach (Item item in _sellContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                itemIndex++;
            }
        }

        return itemIndex > 12;
    }

    private void AddItem()
    {
        Item itemToAdd = new Item(_inspectItemShower.Item.ItemData, 1);

        if (_buyContainer.Items.Contains(_inspectItemShower.Item))
        {
            _traderContainer.AddItem(itemToAdd, true);
        }
        else if (_sellContainer.Items.Contains(_inspectItemShower.Item))
        {
            _playerContainer.AddItem(itemToAdd, true);
        }
        else if (_traderContainer.Items.Contains(_inspectItemShower.Item))
        {
            _buyContainer.AddItem(itemToAdd, true);

            if (CheckForTableOverflow())
            {
                _buyContainer.RemoveItem(new Item(_inspectItemShower.Item.ItemData, 1), 1);
                return;
            }
        }
        else if (_playerContainer.Items.Contains(_inspectItemShower.Item))
        {
            _sellContainer.AddItem(itemToAdd, true);

            if (CheckForTableOverflow())
            {
                _sellContainer.RemoveItem(new Item(_inspectItemShower.Item.ItemData, 1), 1);
                return;
            }
        }

        _inspectItemShower.Item.AddCount(-1);
        ShowItemInfo(_inspectItemShower.Item);
        UpdateItemShowers();
    }

    private void Trade()
    {
        foreach (Item item in _buyContainer.Items)
        {
            if (item != null && item.ItemData != null && item.Count > 0)
            {
                _playerContainer.AddItem(item, true);
            }
        }

        _sellContainer = new ItemContainer(12);
        _buyContainer = new ItemContainer(12);
        UpdateItemShowers();
    }
}
