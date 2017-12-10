using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimoutDurationLab
{
    public class Activity
    {

        public void Run()
        {
            InitializeStartDelay();

            InitializeDuration();
            
        }


        public void Call()
        {
            SetRemainingTimeoutDuration();
        }



        private void SetRemainingTimeoutDuration()
        {
            var now = SystemTime.Now();

            // Calculate remaining duration
            var duration = EndDate.Value.Date.AddDays(1) - now.Date;
            duration = duration.Add(StartDate.Value - now);

            if (SendReminder && !ReminderSent)
            {
                TimeoutDuration = duration - ReminderTimeoutDuration;
            }
            else
            {
                TimeoutDuration = duration;
            }
        }

        private void InitializeDuration()
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
                // Sanitize. just in case
                EndDate = EndDate.Value.Date;

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


        private void InitializeStartDelay()
        {
            var now = SystemTime.Now();

            if (null != StartDate && StartDate.Value > now.Date)
            {

                // TODO:
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

