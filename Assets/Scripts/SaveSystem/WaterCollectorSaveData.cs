namespace SaveDatas
{
    [System.Serializable]
    public class WaterCollectorSaveData : UpgradableSaveData
    {
        public ushort WaterCollected;
        public uint WaterCollectionStart;

        public WaterCollectorSaveData(WaterCollector waterCollector) : base(waterCollector)
        {
            WaterCollected = waterCollector.WaterCollected;
            WaterCollectionStart = waterCollector.WaterCollectionStart;
        }
    }
}
