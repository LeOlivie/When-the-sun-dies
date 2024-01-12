using UnityEngine;
using System;
using UnityEditor;

namespace Statuses
{
    public enum DependentVarEnum { Water, KCal, Fatigue, Happiness, Time, Health }

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
        [SerializeField] private string _iconName;
        [SerializeField] private DependentVarEnum _dependentVar;
        [Tooltip("Value at which status will begin removing itself.")]
        [SerializeField] private float _removeValue = 200;
        [SerializeField] private int _startProgress;
        [SerializeField] private int _maxProgress;
        [SerializeField] private EffectStruct[] _effectStructs;

        public string Name => _name;
        public string IconName => _iconName;
        public int StartProgress => _startProgress;
        public int MaxProgress => _maxProgress;
        public EffectStruct[] EffectStructs => _effectStructs;

        public int CheckForProgression()
        {
            if (_dependentVar.Equals(DependentVarEnum.Time))
            {
                return -1;
            }

            float value = (float)GlobalRepository.PlayerVars.GetType().GetProperty(_dependentVar.ToString()).GetValue(GlobalRepository.PlayerVars);

            if (value < _removeValue)
            {
                return 1;
            }
            return -5;
        }
    }
}