using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShower : MonoBehaviour
{
    [SerializeField] private Sprite[] _tutorialSprites;
    [SerializeField] private ButtonHandler _backBtn;
    [SerializeField] private ButtonHandler _prevBtn;
    [SerializeField] private ButtonHandler _nextBtn;
    private int _imgIndex = 0;

    private void Start()
    {
        _backBtn.AddListener(BackToMenu);
        _prevBtn.AddListener(PrevImg);
        _nextBtn.AddListener(NextImg);
    }

    private void BackToMenu()
    {
        
    }

    private void PrevImg()
    {
        _imgIndex--;
        
        if (_imgIndex < 0)
        {
            _imgIndex = 0;
        }
    }

    private void NextImg() 
    {
        _imgIndex++;

        if (_imgIndex >= _tutorialSprites.Length)
        {
            _imgIndex = _tutorialSprites.Length-1;
        }
    }
}
