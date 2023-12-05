using UnityEngine;

public class StartKitGiver : MonoBehaviour
{
    private void Start()
    {
        if (GlobalRepository.IsStartKitReceived)
        {
            return;
        }

        foreach (Item item in GlobalRepository.Difficulty.StartKit)
        {
            GlobalRepository.Inventory.AddItem(item,false);
        }
        GlobalRepository.IsStartKitReceived = true;
    }
}
