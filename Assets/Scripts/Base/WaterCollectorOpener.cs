using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveDatas;

public class WaterCollectorOpener : Savable
{
    [SerializeField] private WaterCollector _waterCollector = new WaterCollector();
    [SerializeField] private HiddableButtonHandler _interractButton;
    [SerializeField] private WaterCollectorScreenShower _waterCollectorScreenShower;
    [SerializeField] private SpriteRenderer _waterCollectorSpriteRenderer;

    public WaterCollector WaterCollector => _waterCollector;

    private void Awake()
    {
        _waterCollector.CheckWaterCollection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.AddListener(OpenWaterCollectorScreen);
        _waterCollector.OnUpgradedEvent += ChangeSprite;
    }

    private void OpenWaterCollectorScreen()
    {
        if (_waterCollectorScreenShower.gameObject.activeInHierarchy)
        {
            _waterCollectorScreenShower.CloseScreen();
        }
        else
        {
            _waterCollectorScreenShower.OpenWaterCollectorScreen(_waterCollector);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.RemoveListener(OpenWaterCollectorScreen);
        _waterCollector.OnUpgradedEvent -= ChangeSprite;
    }

    private void ChangeSprite()
    {
        _waterCollectorSpriteRenderer.sprite = _waterCollector.UpgradedSprite;
    }

    public override SaveData GetSaveData()
    {
        return new WaterCollectorSaveData(_waterCollector);
    }

    public override void LoadSaveData(SaveData saveData)
    {
        _waterCollector.LoadSaveData((WaterCollectorSaveData)saveData);
        ChangeSprite();
    }
}
