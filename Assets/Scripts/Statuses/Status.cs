using SaveDatas;
using System.Collections;
using System.Collections.Generic;

namespace Statuses
{
    public class Status
    {
        protected StatusData _statusData;
        protected int _progress;

        public StatusData Data => _statusData;
        public int Progress => _progress;

        public void TimeUpdated()
        {
            _progress += _statusData.CheckForProgression();

            if (_progress > _statusData.MaxProgress)
            {
                _progress = _statusData.MaxProgress;
            }
            else if (_progress <= 0)
            {
                GlobalRepository.RemoveStatus(this);
                return;
            }
        }
    

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
            _progress = statusData.StartProgress;
        }
        public Status(StatusSaveData saveData) : this(saveData.StatusData)
        {
            _progress = saveData.Progress;
        }
    }
}