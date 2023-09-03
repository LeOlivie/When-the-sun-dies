using UnityEngine;

public class QuestPointTypeLocate : QuestPointManager
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            QuestPointExecuted();
            Destroy(this);
        }
    }
}
