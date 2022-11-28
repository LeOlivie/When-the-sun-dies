using UnityEngine;

public class WalllHider : MonoBehaviour
{
    [SerializeField] private float _opacity = 0.6f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Color color = this.GetComponent<SpriteRenderer>().color;
            this.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, _opacity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Color color = this.GetComponent<SpriteRenderer>().color;
            this.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
        }
    }
}
