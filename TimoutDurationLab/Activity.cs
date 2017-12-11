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
            var timeOfday = now.TimeOfDay;
            var duration = EndDate.Value - StartDate.Value - now.TimeOfDay;

            if (SendReminder && !ReminderSent)
            {
                TimeoutDuration = TimeoutDuration - duration - ReminderTimeoutDuration;
            }
            else
            {
                TimeoutDuration = TimeoutDuration - duration;
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


            if (StartDate.Value > now.Date)
            {
                StartDelayTimeoutDurationy = StartDate.Value - now;
                SetTimeoutDuration(default(TimeSpan));
            }
            else
            {
                var timeOfDay = now.TimeOfDay;
                SetTimeoutDuration(timeOfDay);
            }


            if (TimeoutDuration < TimeSpan.Zero)
            {
                throw new InvalidOperationException("Timeoutduration is negative");
            }

        }

        private void SetTimeoutDuration(TimeSpan timeOfDay)
        {
            if (null != EndDate)
            {
                // sanity check just in case. End date has time part
                EndDate = EndDate.Value.Date;

                TotalTimeoutDuration = EndDate.Value.AddDays(1) - StartDate.Value;
            }
            else
            {
                var now = SystemTime.Now();
                EndDate = now.Add(TotalTimeoutDuration + StartDelayTimeoutDurationy + timeOfDay).Date;
            }

            if (SendReminder)
            {

                TimeoutDuration = TotalTimeoutDuration - timeOfDay - ReminderTimeoutDuration;
            }
            else
            {
                TimeoutDuration = TotalTimeoutDuration - timeOfDay;
            }
        }
    

        public bool IsStarted
        {
            get
            {
                var now = SystemTime.Now();
                return StartDate.Value <= now.Date;
            }
        }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TimeSpan ReminderTimeoutDuration { get; set; }// = TimeSpan.FromHours(12);

        public TimeSpan TotalTimeoutDuration { get; set; }// = TimeSpan.FromDays(1);

        public TimeSpan TimeoutDuration { get; set; }

        public bool SendReminder { get; set; }
        
        public bool ReminderSent { get; set; }

        public TimeSpan StartDelayTimeoutDurationy { get; set; }
    }
}

