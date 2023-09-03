using System;

namespace SaveDatas
{
    [Serializable]
    public class UpgradableSaveData : SaveData
    {
        public bool IsBeingUpgraded;
        public int UpgradeTimeLeft;
        public uint CurrLevel;

        public UpgradableSaveData(IUpgradable upgradable)
        {
            IsBeingUpgraded = upgradable.IsBeingUpgraded;
            UpgradeTimeLeft = upgradable.UpgradeTimeLeft;
            CurrLevel = upgradable.CurrLevel;
        }
    }
}