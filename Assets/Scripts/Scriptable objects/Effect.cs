using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effect", order = 0)]
public class Effect : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _description;

    public string Name => _name;
    public string Description => _description;
}
