using UnityEngine;
using System;

namespace SaveDatas
{
    [Serializable]
    public class HarvestPOISave
    {
        [Serializable]
        public struct HarvestOptionSave
        {
            public HarvesterData.HarvestOptionData Data;
            public int HarvestsLeft;

            public HarvestOptionSave(Harvester.HarvestOption harvestOption)
            {
                Data = harvestOption.HarvestOptionData;
                HarvestsLeft = harvestOption.HarvestsLeft;
            }
        }

        public HarvestOptionSave[] HarvestOptionSaves;

        public HarvestPOISave(Harvester harvester)
        {
            HarvestOptionSaves = new HarvestOptionSave[harvester.HarvestOptions.Length];

            for (int i = 0; i < harvester.HarvestOptions.Length; i++)
            {
                HarvestOptionSaves[i] = new HarvestOptionSave(harvester.HarvestOptions[i]);
            }
        }
    }
}