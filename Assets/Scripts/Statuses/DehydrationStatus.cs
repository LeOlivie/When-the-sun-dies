using System.Collections;
using System.Collections.Generic;

namespace Statuses
{
    public class DehydrationStatus : Status
    {
        public override void TimeUpdated()
        {
            if (GlobalRepository.Water < 200 && _progress < _statusData.MaxProgress)
            {
                _progress += 1;
            }
            else if(GlobalRepository.Water >= 200)
            {
                _progress -= 5;
                if (_progress <= 0)
                {
                    GlobalRepository.RemoveStatus(this);
                    return;
                }
            }
        }

        public DehydrationStatus(StatusData statusData) : base(statusData) { }
    }
}