namespace SaveDatas
{
    [System.Serializable]
    public class WaterCollectorSaveData
    {
        public UpgradableSaveData UpgradableSave;
        public ushort WaterCollected;
        public uint WaterCollectionStart;

        public WaterCollectorSaveData(WaterCollector waterCollector)
        {
            UpgradableSave = new UpgradableSaveData(waterCollector);
            WaterCollected = waterCollector.WaterCollected;
            WaterCollectionStart = waterCollector.WaterCollectionStart;
        }
    }
}
