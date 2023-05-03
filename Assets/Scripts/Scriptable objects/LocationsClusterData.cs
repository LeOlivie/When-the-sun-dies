using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationsClusterData", menuName = "ScriptableObjects/LocationsClusterData", order = 11)]
public class LocationsClusterData : ScriptableObject
{
    [SerializeField] private LocationData[] _locationDatas;

    public LocationData[] LocationDatas => _locationDatas;
}
