using UnityEngine;

[CreateAssetMenu(fileName = "LightSourceItem", menuName = "ScriptableObjects/Items/LightSourceItem", order = 3)]
public class LightSourceData : ItemData
{
    [SerializeField] private float _searchSpeed;
    [SerializeField] private float _harvestSpeed;
    [SerializeField] private float _lightRadius;
    [SerializeField] private float _lightIntensity;
    [SerializeField] private Color _lightColor;
    [SerializeField] private Item _disposableItem;

    public float SearchSpeed => _searchSpeed;
    public float HarvestSpeed => _harvestSpeed;
    public float LightRadius => _lightRadius;
    public float LightIntensity => _lightIntensity;
    public Color LightColor => _lightColor;
    public Item DisposableItem => _disposableItem;
}
