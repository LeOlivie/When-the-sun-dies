using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statuses;

public class TimedStatus : Status
{
    private int _endTime;

    public override void TimeUpdated()
    {
        if (GlobalRepository.GlobalTime < _endTime)
        {
            _progress += 1;
        }
        else
        {
            GlobalRepository.RemoveStatus(this);
            return;
        }
    }

    public TimedStatus(TimedStatusData timedStatusData) : base(timedStatusData) 
    {
        _endTime = GlobalRepository.GlobalTime + timedStatusData.Duration;
    }
}
