using UnityEngine;
using System;
using UnityEditor.SearchService;

[CreateAssetMenu(fileName = "LocationData", menuName = "ScriptableObjects/LocationData", order = 10)]
public class LocationData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _description;
    [SerializeField] private float _distance;
    [SerializeField, Tooltip("ID in scene manager.")] private string _locationID;

    public string Name => _name;
    public string Description => _description;
    public float Distance => _distance;
    public string LocationID => _locationID;
}
