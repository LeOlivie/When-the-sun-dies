using System;

namespace SaveDatas
{
    [Serializable]
    public class HydroponicsSaveData : UpgradableSaveData
    {
        public CropsData GrowingCropData;
        public int GrowStartTime;
        public int LastWateringTime;

        public HydroponicsSaveData(Hydroponics hydroponics) : base(hydroponics)
        {
            GrowingCropData = hydroponics.GrowingCropData;
            GrowStartTime = hydroponics.GrowStartTime;
            LastWateringTime = hydroponics.LastWateringTime;
        }
    }
}