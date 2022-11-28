using UnityEngine;

public class CharacterBackMover : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CharacterMover>().AddSortOrder(this.gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<CharacterMover>().RemoveSortOrder(this.gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1);
        }
    }
}
