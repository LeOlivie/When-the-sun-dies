using UnityEngine;
using TMPro;

public class CropGrowthShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private ButtonHandler _waterBtn;
    [SerializeField] private ButtonHandler _uprootBtn;
    [SerializeField] private ButtonHandler _collectBtn;
    [SerializeField] private ItemShower _waterSlotItemShower;
    [SerializeField] private Item _waterItem;
    [SerializeField] private Item _waterBottleItem;

    private Hydroponics _hydroponics;

    private void Awake()
    {
        _uprootBtn.AddListener(UprootCrop);
        _waterBtn.AddListener(WaterCrop);
        _collectBtn.AddListener(HarvestCrop);
        _waterSlotItemShower.ShowItem(_waterItem);
    }

    public void ShowCropGrowth(Hydroponics hydroponics)
    {
        this.gameObject.SetActive(true);
        _hydroponics = hydroponics;
        ShowInfo();
        GlobalRepository.OnTimeUpdated -= ShowInfo;
        GlobalRepository.OnTimeUpdated += ShowInfo;
    }

    public void ShowInfo()
    {
        if (_hydroponics == null || _hydroponics.GrowingCropData == null)
        {
            this.gameObject.SetActive(false);
        }

        _infoText.text = "";

        if ((_hydroponics.LastWateringTime + _hydroponics.GrowingCropData.WateringTime <= GlobalRepository.SystemVars.GlobalTime)&&(_hydroponics.GrowStartTime + _hydroponics.GrowingCropData.GrowTime >= _hydroponics.LastWateringTime + _hydroponics.GrowingCropData.WateringTime))
        {
            _infoText.text = "The plant has died!";
            _uprootBtn.gameObject.SetActive(true);
            _waterBtn.gameObject.SetActive(false);
            _waterSlotItemShower.transform.parent.gameObject.SetActive(false);
            _collectBtn.gameObject.SetActive(false);
            return;
        }

        if (_hydroponics.GrowStartTime + _hydroponics.GrowingCropData.GrowTime <= GlobalRepository.SystemVars.GlobalTime)
        {
            _infoText.text = "The plant can be harvested.";
            _uprootBtn.gameObject.SetActive(false);
            _waterBtn.gameObject.SetActive(false);
            _waterSlotItemShower.transform.parent.gameObject.SetActive(false);
            _collectBtn.gameObject.SetActive(true);
            return;
        }

        float waterLvlTemp = ((GlobalRepository.SystemVars.GlobalTime - _hydroponics.LastWateringTime) / (float)_hydroponics.GrowingCropData.WateringTime) * 100;
        int waterLvl = 100-Mathf.RoundToInt(waterLvlTemp);
        int waterTime = (int)(_hydroponics.LastWateringTime + _hydroponics.GrowingCropData.WateringTime - GlobalRepository.SystemVars.GlobalTime);
        int growingTimeLeft = (int)(_hydroponics.GrowStartTime + _hydroponics.GrowingCropData.GrowTime - GlobalRepository.SystemVars.GlobalTime);

        _infoText.text += _hydroponics.GrowingCropData.Name + "\n\n";
        _infoText.text += $"Water level: {waterLvl}%\n";
        _infoText.text += TimeConverter.InsertTime("Plant's death in\n{0} days {1}:{2}\n\n", waterTime, TimeConverter.InsertionType.DayHourMinute);
        _infoText.text += TimeConverter.InsertTime("Time until harvest\n{0} days {1}:{2}", growingTimeLeft,TimeConverter.InsertionType.DayHourMinute);
        _uprootBtn.gameObject.SetActive(true);
        _waterBtn.gameObject.SetActive(true);
        _waterSlotItemShower.transform.parent.gameObject.SetActive(true);
        _collectBtn.gameObject.SetActive(false);
    }

    public void WaterCrop()
    {
        if (!GlobalRepository.PlayerVars.Inventory.CheckIfHas(_waterItem.ItemData, _waterItem.Count))
        {
            return;
        }

        GlobalRepository.PlayerVars.Inventory.RemoveItem(_waterItem, _waterItem.Count);
        GlobalRepository.PlayerVars.Inventory.AddItem(_waterBottleItem, false);
        _hydroponics.WaterCrop();
    }

    public void HarvestCrop()
    {
        if (!GlobalRepository.PlayerVars.Inventory.CheckIfHas(_waterItem.ItemData, _waterItem.Count))
        {
            return;
        }

        GlobalRepository.PlayerVars.Inventory.AddItem(_hydroponics.GrowingCropData.Output, false);
        _hydroponics.Reset();
        this.gameObject.SetActive(false);
        GlobalRepository.OnTimeUpdated -= ShowInfo;
    }

    public void UprootCrop()
    {
        _hydroponics.Reset();
        this.gameObject.SetActive(false);
        GlobalRepository.OnTimeUpdated -= ShowInfo;
    }
}
