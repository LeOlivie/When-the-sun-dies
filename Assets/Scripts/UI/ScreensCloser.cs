using UnityEngine;

public class ScreensCloser : MonoBehaviour
{
    [SerializeField] private GameObject[] _screens;
    
    public void CloseAllScreens()
    {
        foreach (GameObject screenGO in _screens)
        {
            screenGO.GetComponent<IClosable>().CloseScreen();
        }
    }
}
