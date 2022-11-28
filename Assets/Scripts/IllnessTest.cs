using UnityEngine;
using UnityEngine.UI;

public class IllnessTest : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private Item[] items;
    [SerializeField] private Button _button;
    [SerializeField] private DifficultyData _difficultyData;
    private int a = 0;

    private void Awake()
    {
        GlobalRepository.SetDifficulty(_difficultyData);
    }

    private void Start()
    {
        _button.onClick.AddListener(Pressed);
    }

    private void Pressed()
    {
        if (a >= items.Length)
        {
            return;
        }

        GlobalRepository.Inventory.AddItem(items[a], false);
        a++;
    }
}
