using UnityEngine;
using System;

namespace Statuses
{
    [CreateAssetMenu(fileName = "StatusData", menuName = "ScriptableObjects/SatusesAndEffects/StatusData", order = 1)]
    public class StatusData : ScriptableObject
    {
        [Serializable]
        public struct EffectStruct
        {
            [SerializeField] private int _progressRequired;
            [SerializeField] private EffectData _effectData;

            public int ProgressRequired => _progressRequired;
            public EffectData EffectData => _effectData;
        }

        [SerializeField] private string _name;
        [SerializeField] private int _maxProgress;
        [SerializeField] private EffectStruct[] _effectStructs;

        public string Name => _name;
        public int MaxProgress => _maxProgress;
        public EffectStruct[] EffectStructs => _effectStructs;
    }
}