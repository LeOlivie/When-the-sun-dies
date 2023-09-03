using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveDatas;

public class RadioScreenOpener : Savable
{
    [SerializeField] private Radio _radio;
    [SerializeField] private HiddableButtonHandler _interractButton;
    [SerializeField] private RadioScreenShower _radioScreenShower;
    [SerializeField] private SpriteRenderer _radioSpriteRenderer;

    public Radio Radio => _radio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.AddListener(OpenRadioScreen);
        _radio.OnUpgradedEvent += ChangeSprite;
    }

    private void OpenRadioScreen()
    {
        if (_radioScreenShower.gameObject.activeInHierarchy)
        {
            _radioScreenShower.CloseScreen();
        }
        else
        {
            _radioScreenShower.OpenRadioScreen(_radio);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.RemoveListener(OpenRadioScreen);
        _radio.OnUpgradedEvent -= ChangeSprite;
    }

    private void ChangeSprite()
    {
        _radioSpriteRenderer.sprite = _radio.UpgradedSprite;
    }

    public override SaveData GetSaveData()
    {
        return new RadioSaveData(_radio);
    }

    public override void LoadSaveData(SaveData saveData)
    {
        _radio.LoadSaveData((RadioSaveData)saveData);
        ChangeSprite();
    }
}
