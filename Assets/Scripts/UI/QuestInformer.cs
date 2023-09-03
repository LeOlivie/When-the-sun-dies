using UnityEngine;
using System.Collections;
using TMPro;

public class QuestInformer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private Animation _animation;

    private void Awake()
    {
        _animation = this.gameObject.GetComponent<Animation>();
    }

    public void ShowMessage(string message)
    {
        if (_animation.isPlaying)
        {
            StartCoroutine(CoolDown(message));
        }

        _text.text = message;
        _animation.Play();
    }

    private IEnumerator CoolDown(string message)
    {
        while (_animation.isPlaying)
        {
            yield return new WaitForSeconds(0.5f);
        }

        ShowMessage(message);
    }
}
