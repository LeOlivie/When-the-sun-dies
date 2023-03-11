namespace SaveDatas
{
    [System.Serializable]
    public class TVSaveData
    {
        public UpgradableSaveData UpgradableSave;
        public ushort WatchTime;

        public TVSaveData(TV tv)
        {
            UpgradableSave = new UpgradableSaveData(tv);
            WatchTime = tv.WatchTime;
        }
    }
}
