using System.Collections;
using System.Collections.Generic;

namespace Statuses
{
    public class DepressionStatus : Status
    {
        public override void TimeUpdated()
        {
            if (GlobalRepository.Happiness < 5 && _progress < _statusData.MaxProgress)
            {
                _progress += 1;
            }
            else if(GlobalRepository.Happiness >= 5)
            {
                _progress -= 5;
                if (_progress <= 0)
                {
                    GlobalRepository.RemoveStatus(this);
                    return;
                }
            }
        }

        public DepressionStatus(StatusData statusData) : base(statusData) { }
    }
}