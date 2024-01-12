using UnityEngine;
using TMPro;

public class CraftingRecipieShower : MonoBehaviour
{
    [SerializeField] private ItemShower[] _inputShowers;
    [SerializeField] private ItemShower _outputShower;
    [SerializeField] private IndexedButtonHandler _startBtn;
    [SerializeField] private TextMeshProUGUI _startBtnText;
    [SerializeField] private TextMeshProUGUI _outputNameText;
    [SerializeField] private TextMeshProUGUI _craftLengthText;

    private void Awake()
    {
        ShowRecipie(null, null);
    }

    public void ShowRecipie(CraftingRecipieData recipieData, IndexedButtonHandler.IndexedButtonDelegate startCraftDelegate)
    {
        foreach(ItemShower itemShower in _inputShowers)
        {
            itemShower.ShowItem(null);
        }

        _startBtn.RemoveListener(startCraftDelegate);

        if (recipieData == null)
        {
            _outputShower.ShowItem(null);
            _outputNameText.text = "";
            _craftLengthText.text = "";
            _startBtnText.color = new Color(0.8f, 0.4f, 0.4f, 1);
            return;
        }

        for (int i = 0; i < recipieData.ItemRequirements.Length; i++)
        {
            _inputShowers[i].ShowItem(recipieData.ItemRequirements[i]);
        }

        _outputShower.ShowItem(recipieData.Output[0]);
        _outputNameText.text = recipieData.Output[0].Name;
        _craftLengthText.text = TimeConverter.InsertTime("{0}:{1}", recipieData.Time,TimeConverter.InsertionType.HourMinute);

        foreach (Item input in recipieData.ItemRequirements)
        {
            if (input != null && input.ItemData != null && !GlobalRepository.PlayerVars.Inventory.CheckIfHas(input.ItemData, input.Count))
            {
                return;
            }
        }

        _startBtnText.color = new Color(1, 1, 1, 1);
        _startBtn.AddListener(startCraftDelegate);
    }
}
