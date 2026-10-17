using System;

namespace MonoCoC.Contents.World
{
    public class Time
    {
        public int Days = 1;

        public int Hours = 6;

        public int Minutes;

        /// <summary>
        /// Hours advanced by the most recent time change. Used by sleep scenes
        /// to describe how long the player rested.
        /// </summary>
        public float LastPassedHours { get; set; }

        public event Action<int> HourPassed;

        public float TotalTime => Days * 24 + Hours;

        public bool IsDay => Hours > 5 && Hours < 21;

        public bool IsNight => !IsDay;

        public bool IsTimeBetween(float min, float max)
        {
            var current = (float)Hours;

            return max > min ? current >= min && current <= max : current >= min || current <= max;
        }

        public void PassTime(int hours)
        {
            LastPassedHours = hours;

            for (var index = 0; index < hours; index++)
            {
                Hours++;

                if (Hours > 23)
                {
                    Hours = 0;

                    Days++;
                }

                HourPassed?.Invoke(Hours);
            }
        }

        public void PassTime(float hours) => PassMinutes((int)Math.Round(hours * 60));

        public void PassMinutes(int minutes)
        {
            if (minutes <= 0)
                return;

            LastPassedHours = minutes / 60f;

            Minutes += minutes;

            while (Minutes >= 60)
            {
                Minutes -= 60;

                Hours++;

                if (Hours > 23)
                {
                    Hours = 0;

                    Days++;
                }

                HourPassed?.Invoke(Hours);
            }
        }

        public string TimeOfDay() => Hours switch
        {
            < 12 => "morning",

            < 17 => "afternoon",

            < 21 => "evening",

            _ => "night"
        };

        public string DayPart() => Hours switch
        {
            < 12 => "morning",

            < 19 => "day",

            _ => "evening"
        };

        public string Sun() => Hours < 21 ? "sun" : "moon";
    }
}