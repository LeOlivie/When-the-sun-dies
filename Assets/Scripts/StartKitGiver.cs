using UnityEngine;

public class StartKitGiver : MonoBehaviour
{
    private void Start()
    {
        if (GlobalRepository.SystemVars.IsStartKitReceived)
        {
            return;
        }

        foreach (Item item in GlobalRepository.SystemVars.Difficulty.StartKit)
        {
            GlobalRepository.PlayerVars.Inventory.AddItem(item,false);
        }
        GlobalRepository.SystemVars.IsStartKitReceived = true;
    }
}
