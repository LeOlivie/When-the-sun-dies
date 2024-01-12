using UnityEngine;
using TMPro;

public class CropShower : MonoBehaviour
{
    [SerializeField] private ItemShower[] _requierementItemShowers;
    [SerializeField] private ItemShower _outputItemShower;
    [SerializeField] private IndexedButtonHandler _startGrowingBtn;
    [SerializeField] private TextMeshProUGUI _timeRequieredText;
    [SerializeField] private TextMeshProUGUI _cropNameText;
    [SerializeField] private Color _unavailableColor;
    [SerializeField] private Color _availableColor;

    public IndexedButtonHandler StartGrowingBtn => _startGrowingBtn;

    public void ShowCrop(CropsData cropData, int index, IndexedButtonHandler.IndexedButtonDelegate doOnBtnPress)
    {
        _startGrowingBtn.RemoveListener(doOnBtnPress);
        _cropNameText.text = "";

        if (cropData == null)
        {
            for (int i = 0; i < _requierementItemShowers.Length; i++)
            {
                _requierementItemShowers[i].ShowItem(null);
            }

            _outputItemShower.ShowItem(null);
            _timeRequieredText.text = "";
            return;
        }

        for (int i = 0; i < _requierementItemShowers.Length; i++)
        {
            _requierementItemShowers[i].ShowItem(null);
        }

        for (int i = 0; i < cropData.Requirements.Length; i++)
        {
            _requierementItemShowers[i].ShowItem(cropData.Requirements[i]);
        }

        _cropNameText.text = cropData.Name;
        _outputItemShower.ShowItem(cropData.Output);
        _timeRequieredText.text = TimeConverter.InsertTime("{0} days {1}:{2}",(int)cropData.GrowTime,TimeConverter.InsertionType.DayHourMinute);

        foreach (Item item in cropData.Requirements)
        {
            if (!GlobalRepository.PlayerVars.Inventory.CheckIfHas(item.ItemData, item.Count))
            {
                _startGrowingBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = _unavailableColor;
                return;
            }
        }

        _startGrowingBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = _availableColor;
        _startGrowingBtn.SetIndex(index);
        _startGrowingBtn.AddListener(doOnBtnPress);
    }
}
