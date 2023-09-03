using UnityEngine;

public class QuestPointTypeCollect : QuestPointManager
{
    [SerializeField] private ButtonHandler _interractBtn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _interractBtn.AddListener(Collect);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _interractBtn.RemoveListener(Collect);
        }
    }

    private void Collect()
    {
        base.QuestPointExecuted();
        Destroy(this.gameObject);
    }
}
