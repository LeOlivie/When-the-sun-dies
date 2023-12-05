using UnityEngine;
using System;


namespace Statuses
{
    public enum StatToChangeEnum { SearchSpeed, HarvestSpeed, HP, KCal, ML, Fatigue, Happiness }
    [CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObjects/SatusesAndEffects/EffectData", order = 0)]
    public class EffectData : ScriptableObject
    {

        [Serializable]
        public struct BuffData
        {
            [SerializeField] private StatToChangeEnum _statToChange;
            [SerializeField] private float _change;

            public StatToChangeEnum StatToChange => _statToChange;
            public float Change => _change;
        }

        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private BuffData[] _buffs;

        public string Name => _name;
        public string Description => _description;
        public BuffData[] Buffs => _buffs;
    }
}