using UnityEngine;
using System.Collections.Generic;

public class Harvester : MonoBehaviour
{
    [SerializeField] private HarvestScreenShower _harvestScreenShower;
    [SerializeField] private HarvesterData _harvesterData;
    [SerializeField] private ButtonHandler _interractBtn;
    private HarvestOption[] _harvestOptions;

    public HarvestOption[] HarvestOptions => _harvestOptions;

    public struct HarvestOption
    {
        private HarvesterData.HarvestOptionData _harvestOptionData;
        private int _harvestsLeft;

        public HarvesterData.HarvestOptionData HarvestOptionData => _harvestOptionData;
        public int HarvestsLeft => _harvestsLeft;

        public void ChangeHarvestsLeftAmount(int change)
        {
            _harvestsLeft += change;

            if (_harvestsLeft < 0)
            {
                _harvestsLeft = 0;
            }
            else if(_harvestsLeft > _harvestOptionData.MaxHarvests)
            {
                _harvestsLeft = _harvestOptionData.MaxHarvests;
            }
        }

        public HarvestOption(HarvesterData.HarvestOptionData harvestOptionData, int harvestsAmount)
        {
            _harvestOptionData = harvestOptionData;
            _harvestsLeft = harvestsAmount;
        }
    }

    private void Start()
    {
        List<HarvestOption> options = new List<HarvestOption>();

        foreach (HarvesterData.HarvestOptionData data in _harvesterData.HarvestOptionDatas)
        {
            if (Random.Range(1,101) <= data.AvailabilityChance)
            {
                options.Add(new HarvestOption(data, data.MaxHarvests));
            }
        }

        _harvestOptions = options.ToArray();

        if (_harvestOptions.Length <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _interractBtn.AddListener(Interract);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _interractBtn.RemoveListener(Interract);
    }

    private void Interract()
    {
        if (!_harvestScreenShower.gameObject.activeInHierarchy)
        {
            _harvestScreenShower.OpenHarvestMenu(ref _harvestOptions);
        }
        else
        {
            _harvestScreenShower.CloseScreen();
        }
    }

    public void LoadSaveData(SaveDatas.HarvestPOISave saveData)
    {
        _harvestOptions = new HarvestOption[saveData.HarvestOptionSaves.Length];

        for(int i = 0; i < saveData.HarvestOptionSaves.Length; i++)
        {
            _harvestOptions[i] = new HarvestOption(saveData.HarvestOptionSaves[i].Data, saveData.HarvestOptionSaves[i].HarvestsLeft);
        }

        if (_harvestOptions.Length <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
