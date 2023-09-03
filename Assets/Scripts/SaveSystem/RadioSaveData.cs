using System;

namespace SaveDatas
{
    [Serializable]
    public class RadioSaveData : UpgradableSaveData
    {
        public EventLocClusterData ActiveEventLocCluster;
        public int EventSwitchOffTime;
        public int NextEventTime;

        public RadioSaveData(Radio radio) : base(radio)
        {
            ActiveEventLocCluster = radio.ActiveEventLocCluster;
            EventSwitchOffTime = radio.EventSwitchOffTime;
            NextEventTime = radio.NextEventTime;
        }
    }
}