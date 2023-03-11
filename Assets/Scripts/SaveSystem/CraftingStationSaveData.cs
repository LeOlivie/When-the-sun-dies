namespace SaveDatas
{
    [System.Serializable]
    public class CraftingStationSaveData
    {
        public UpgradableSaveData UpgradableSave;
        public int CraftingTimeLeft;
        public CraftingRecipieData ActiveCraft;

        public CraftingStationSaveData(CraftingStation craftingStation)
        {
            UpgradableSave = new UpgradableSaveData(craftingStation);
            CraftingTimeLeft = craftingStation.CraftTimeLeft;
            ActiveCraft = craftingStation.ActiveCraftData;
        }
    }
}
