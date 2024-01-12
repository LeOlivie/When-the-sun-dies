using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestPointTypeGive : QuestPointManager, IClosable
{
    [SerializeField] private ButtonHandler _interractBtn;
    [SerializeField] private GameObject _giveScreen;
    [SerializeField] private ItemShower[] _itemShowers;
    [SerializeField] private IndexedButtonHandler[] _giveButtons;
    [SerializeField] private ItemShower[] _invetoryItemShowers;

    private void OnEnable()
    {

        for (int i = 0; i < _itemShowers.Length; i++)
        {
            if (i >= GlobalRepository.SystemVars.ActiveQuest.SubQuests.Length)
            {
                _giveButtons[i].SetIndex(i);
                _giveButtons[i].gameObject.SetActive(false);
                _itemShowers[i].ShowItem(null);
            }

            _giveButtons[i].SetIndex(i);
            _giveButtons[i].AddListener(GiveItem);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _interractBtn.AddListener(OpenGiveScreen);
        }
        GlobalRepository.PlayerVars.Inventory.ContainerUpdated += OnInventoryUpdated;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _interractBtn.RemoveListener(OpenGiveScreen);
        }
        GlobalRepository.PlayerVars.Inventory.ContainerUpdated -= OnInventoryUpdated;
    }

    private void OpenGiveScreen()
    {
        _giveScreen.SetActive(!_giveScreen.activeInHierarchy);
        ShowGiveScreen();
        OnInventoryUpdated();
    }

    private void ShowGiveScreen()
    {
        for (int i = 0; i < GlobalRepository.SystemVars.ActiveQuest.SubQuests.Length; i++)
        {
            if (GlobalRepository.SystemVars.ActiveQuest.SubQuests[i].IsCompleted)
            {
                _giveButtons[i].gameObject.SetActive(false);
                _itemShowers[i].ShowItem(null);
                continue;
            }

            ItemData itemData = GlobalRepository.SystemVars.ActiveQuest.SubQuests[i].SubQuestData.ItemsRequiered.ItemData;
            int countNeeded = GlobalRepository.SystemVars.ActiveQuest.SubQuests[i].SubQuestData.ItemsRequiered.Count;
            int countGiven = GlobalRepository.SystemVars.ActiveQuest.SubQuests[i].Progress;

            _itemShowers[i].ShowItem(new Item(itemData, countNeeded - countGiven));
        }
    }

    private void OnInventoryUpdated()
    {
        for(int i = 0; i < _invetoryItemShowers.Length; i++)
        {
            _invetoryItemShowers[i].ShowItem(GlobalRepository.PlayerVars.Inventory.Items[i]);
        }
    }

    private void GiveItem(int index)
    {
        if (!GlobalRepository.PlayerVars.Inventory.CheckIfHas(_itemShowers[index].Item.ItemData, 1))
        {
            return;
        }

        if (_itemShowers[index].Item.Count == 1)
        {
            _giveButtons[index].gameObject.SetActive(false);
        }

        GlobalRepository.PlayerVars.Inventory.AddItem(new Item(_itemShowers[index].Item.ItemData, -1), false);
        _itemShowers[index].Item.AddCount(-1);
        _itemShowers[index].ShowItem(_itemShowers[index].Item);
        GlobalRepository.SystemVars.ActiveQuest.ChangeTaskProgress(ref GlobalRepository.SystemVars.ActiveQuest.SubQuests[index], 1);
    }

    public void CloseScreen()
    {
        _giveScreen.SetActive(false);
    }
}
