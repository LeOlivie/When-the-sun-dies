using System.Collections;
using System.Collections.Generic;

namespace Statuses
{
    public abstract class Status
    {
        protected StatusData _statusData;
        protected int _progress;

        public StatusData Data => _statusData;
        public int Progress => _progress;

        public abstract void TimeUpdated();

        public List<EffectData> GetActiveEffects()
        {
            List<EffectData> activeEffectDatas = new List<EffectData>();

            foreach (StatusData.EffectStruct effectStruct in _statusData.EffectStructs)
            {
                if (effectStruct.ProgressRequired < _progress)
                {
                    activeEffectDatas.Add(effectStruct.EffectData);
                }
            }

            return activeEffectDatas;
        }

        public Status(StatusData statusData)
        {
            _statusData = statusData;
        }
    }
}