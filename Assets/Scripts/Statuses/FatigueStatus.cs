using System.Collections;
using System.Collections.Generic;

namespace Statuses
{
    public class FatigueStatus : Status
    {
        public override void TimeUpdated()
        {
            if (GlobalRepository.Fatigue < 10 && _progress < _statusData.MaxProgress)
            {
                _progress += 1;
            }
            else if (GlobalRepository.Fatigue >= 10)
            {
                _progress -= 5;
                if (_progress <= 0)
                {
                    GlobalRepository.RemoveStatus(this);
                    return;
                }
            }
        }

        public FatigueStatus(StatusData statusData) : base(statusData) { }
    }
}