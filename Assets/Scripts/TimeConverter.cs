public static class TimeConverter
{
    public enum InsertionType { DayHourMinute, HourMinute};

    public static string InsertTime(string str, int time, InsertionType insertionType)
    {
        int days = time / 1440;
        int hours = time  / 60 - days * 24;
        int minutes = time - days * 1440 - hours * 60;

        string hoursStr = hours.ToString();
        string minutesStr = minutes.ToString();

        if (hours < 10)
        {
            hoursStr = "0" + hours;
        }
        
        if (minutes < 10)
        {
            minutesStr = "0" + minutes;
        }

        if (insertionType == InsertionType.DayHourMinute)
        {
            return string.Format(str,days, hoursStr, minutesStr);
        }
        else
        {
            return string.Format(str, hoursStr, minutesStr);
        }
    }
}
