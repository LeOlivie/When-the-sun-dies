using UnityEngine;

public class Harvester : MonoBehaviour
{
    [SerializeField] private HarvestScreenShower _harvestScreenShower;
    [SerializeField] private HarvesterData _harvesterData;
    [SerializeField] private ButtonHandler _interractBtn;

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
            _harvestScreenShower.OpenHarvestMenu(_harvesterData);
        }
        else
        {
            _harvestScreenShower.CloseHarvestMenu();
        }
    }
}
