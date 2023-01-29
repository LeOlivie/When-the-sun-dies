using UnityEngine;
using UnityEngine.UI;

public class IllnessTest : MonoBehaviour
{
    [SerializeField] private DifficultyData _difficultyData;

    private void Awake()
    {
        GlobalRepository.SetDifficulty(_difficultyData);
    }
}
