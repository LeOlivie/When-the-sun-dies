namespace SaveDatas
{
    [System.Serializable]
    public class TVSaveData : UpgradableSaveData
    {
        public ushort WatchTime;

        public TVSaveData(TV tv): base(tv)
        {
            WatchTime = tv.WatchTime;
        }
    }
}
