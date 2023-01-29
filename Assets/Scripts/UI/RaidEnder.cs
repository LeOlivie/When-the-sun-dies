using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RaidEnder : MonoBehaviour
{
    [SerializeField] private ButtonHandler _leaveButton;
    [SerializeField] private LocationData _baseData;
    private Saver saver;

    private void Start()
    {
        saver = GameObject.FindObjectOfType<Saver>();
        _leaveButton.AddListener(EndRaid);
    }

    public void EndRaid()
    {
        saver.SaveLocation();
        GlobalRepository.ChangeLocation(_baseData);
    }
}
