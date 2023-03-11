namespace SaveDatas
{
    [System.Serializable]
    public class BaseSaveData
    {
        public CraftingStationSaveData WorkbenchSave;
        public CraftingStationSaveData CookingStationSave;
        public StorageSaveData StorageSave;
        public TVSaveData TVSave;
        public UpgradableSaveData BedSave;
        public WaterCollectorSaveData WaterCollectorSave;

        public BaseSaveData(CraftingStation workbench,CraftingStation cookingStation,Storage storage, TV tv, Bed bed, WaterCollector waterCollector)
        {
            WorkbenchSave = new CraftingStationSaveData(workbench);
            CookingStationSave = new CraftingStationSaveData(cookingStation);
            StorageSave = new StorageSaveData(storage);
            TVSave = new TVSaveData(tv);
            BedSave = new UpgradableSaveData(bed);
            WaterCollectorSave = new WaterCollectorSaveData(waterCollector);
        }
    }
}