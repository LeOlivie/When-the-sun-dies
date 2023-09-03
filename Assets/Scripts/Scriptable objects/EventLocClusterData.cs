using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EventLocationsClusterData", menuName = "ScriptableObjects/EventLocationsClusterData", order = 12)]
public class EventLocClusterData : LocationsClusterData
{
    [SerializeField, TextArea] private string _radioText;
    [SerializeField] private int _availableTime;

    public string RadioText => _radioText;
    public int AvailableTime => _availableTime;
}