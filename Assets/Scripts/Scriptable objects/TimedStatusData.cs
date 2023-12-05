using UnityEngine;
using System;

namespace Statuses
{
    [CreateAssetMenu(fileName = "TimedStatusData", menuName = "ScriptableObjects/SatusesAndEffects/TimedStatusData", order = 1)]
    public class TimedStatusData : StatusData
    {
        [SerializeField] private int _duration;

        public int Duration => _duration;
    }
}