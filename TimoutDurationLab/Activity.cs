using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimoutDurationLab
{
    public class Activity
    {

        public TimeSpan CalculateRemaningDuration()
        {
            var now = SystemTime.Now();
            var duration = EndDate.Value.Date.AddDays(1) - now.Date;
            return duration.Add(StartDate.Value - now); ;
        }

        public void CalculateRemainingTimeoutDuration()
        {
            var duration = CalculateRemaningDuration();

            if (SendReminder && !ReminderSent)
            {
                TimeoutDuration = duration - ReminderTimeoutDuration;
            }
            else
            {
                TimeoutDuration = duration;
            }
        }


        public void InitializeDuration()
        {
            var now = SystemTime.Now();

            if (null == StartDate)
            {
                StartDate = now.Date;
            }
            else
            {
                // Sanitize. Start date could have time part
                StartDate = StartDate.Value.Date;
            }

            // calculate delta time to midnight
            var deltaTimeToMidnight = StartDate.Value - now ;
            if (null != EndDate)
            {

                var duration = EndDate.Value.Date.AddDays(1) - now.Date;
                TotalTimeoutDuration = duration.Add(deltaTimeToMidnight);
            }
            else
            {
                var duration = deltaTimeToMidnight.Add(TimeSpan.FromDays(1));
                TotalTimeoutDuration = TotalTimeoutDuration.Add(duration);

                EndDate = now.Date.Add(TotalTimeoutDuration).Date;
            }


            if(SendReminder)
            {
                TimeoutDuration = TotalTimeoutDuration - ReminderTimeoutDuration;
            }
            else
            {
                TimeoutDuration = TotalTimeoutDuration;
            }

            if (TimeoutDuration < TimeSpan.Zero)
            {
                throw new InvalidOperationException("Timeoutduration is negative");
            }

        }


        public void InitializeStartDelay()
        {
            var now = SystemTime.Now();

            if (null != StartDate && StartDate.Value > now.Date)
            {
                var deltaTimeToMidnight = StartDate.Value.Subtract(now);
                StartDelay = deltaTimeToMidnight;
            }
        }


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TimeSpan ReminderTimeoutDuration { get; set; } = TimeSpan.FromHours(12);

        public TimeSpan TotalTimeoutDuration { get; set; } = TimeSpan.FromDays(1);

        public TimeSpan TimeoutDuration { get; set; }

        public bool SendReminder { get; set; }
        
        public bool ReminderSent { get; set; }

        public TimeSpan StartDelay { get; set; }
    }
}

