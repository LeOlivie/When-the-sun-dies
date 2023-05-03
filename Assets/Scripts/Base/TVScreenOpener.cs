using UnityEngine;

public class TVScreenOpener : MonoBehaviour
{
    [SerializeField] private TV _tv = new TV();
    [SerializeField] private HiddableButtonHandler _interractButton;
    [SerializeField] private TVScreenShower _tvSreenShower;
    [SerializeField] private SpriteRenderer _tvSpriteRenderer;

    public TV Tv => _tv;

    private void Awake()
    {
        GlobalRepository.OnTimeUpdated += _tv.AddWatchTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.AddListener(OpenWaterCollectorScreen);
        _tv.OnUpgradedEvent += ChangeSprite;
    }

    private void OpenWaterCollectorScreen()
    {
        if (_tvSreenShower.gameObject.activeInHierarchy)
        {
            _tvSreenShower.CloseScreen();
        }
        else
        {
            _tvSreenShower.OpenTVScreen(_tv);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.RemoveListener(OpenWaterCollectorScreen);
        _tv.OnUpgradedEvent -= ChangeSprite;
    }

    private void ChangeSprite()
    {
        _tvSpriteRenderer.sprite = _tv.UpgradedSprite;
    }

    public void LoadSaveData(SaveDatas.TVSaveData saveData)
    {
        _tv.LoadSaveData(saveData);
        ChangeSprite();
    }
}
