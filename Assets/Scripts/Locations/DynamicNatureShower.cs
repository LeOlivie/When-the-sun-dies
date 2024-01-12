using UnityEngine;
using System;

public class DynamicNatureShower : MonoBehaviour
{
    [Serializable]
    private struct Stage
    {
        [SerializeField] private Sprite _stageSprite;
        [SerializeField] private ushort _day;

        public Sprite StageSprite => _stageSprite;
        public ushort Day => _day;
    }

    [SerializeField] private Stage[] _stages;

    private void Start()
    {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        int day = GlobalRepository.SystemVars.GlobalTime / 1440;

        foreach (Stage stage in _stages)
        {
            if (stage.Day <= day)
            {
                renderer.enabled = true;
                renderer.sprite = stage.StageSprite;
            }
        }
    }
}
