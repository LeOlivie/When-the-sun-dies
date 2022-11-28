using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SicknessData", menuName = "ScriptableObjects/SicknessData", order = 1)]
public class SicknessData : ScriptableObject
{
    [Serializable]
    struct EffectStruct
    {
        [SerializeField] private int _progressRequired;
        [SerializeField] private Effect _effect;

        public int ProgressRequired => _progressRequired;
        public Effect Effect => _effect;
    }

    [SerializeField] private string _name;
    [SerializeField] private int _maxProgress;
    [SerializeField] private int _pointOfNoReturn;
    [SerializeField] private EffectStruct[] _effectStructs = new EffectStruct[0];
    [SerializeField] private int _progress;

    public string Name => _name;
    public int Progress => _progress;
    public int MaxProgress => _maxProgress;

    public Effect[] ActiveEffects()
    {
        Effect[] _effects = new Effect[0];

        foreach (EffectStruct effect in _effectStructs)
        {
            if (_progress >= effect.ProgressRequired)
            {
                Array.Resize(ref _effects, _effects.Length + 1);
                _effects[_effects.Length - 1] = effect.Effect;
            }
        }

        return _effects;
    }

    public void ChangeProgress(int change)
    {
        if (_progress > _maxProgress)
        {
            return;
        }

        if (_progress < _pointOfNoReturn )
        {
            _progress += change;
        }
        else if (_progress >= _pointOfNoReturn && change > 0)
        {
            _progress += change;
        }
    }
}
