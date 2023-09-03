namespace SaveDatas
{
    [System.Serializable]
    public class CraftingStationSaveData : UpgradableSaveData
    {
        public int CraftingTimeLeft;
        public CraftingRecipieData ActiveCraft;

        public CraftingStationSaveData(CraftingStation craftingStation) : base(craftingStation)
        {
            CraftingTimeLeft = craftingStation.CraftTimeLeft;
            ActiveCraft = craftingStation.ActiveCraftData;
        }
    }
}
