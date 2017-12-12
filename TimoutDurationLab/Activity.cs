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

        public void Update()
        {
            SetRemainingTimeoutDuration();
        }

        public bool ShouldSendReminder()
        {
            if (SendReminder && !ReminderSent)
            {
                var result = RemainingDuration() <= ReminderTimeoutDuration;
                // TODO: in prod set outside this function
                ReminderSent = result;
                return result;
            }
            return false;
        }


        private TimeSpan RemainingDuration()
        {
            var now = SystemTime.Now();
            return EndDate.Value + IncludeTimespan - now;
        }

        private void SetRemainingTimeoutDuration()
        {
            var duration = RemainingDuration();

            if (SendReminder && !ReminderSent)
            {
                TimeoutDuration =  duration - ReminderTimeoutDuration;
            }
            else
            {
                TimeoutDuration =  duration;
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
                SetTimeoutDuration();
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

        private void SetTimeoutDuration(TimeSpan timeOfDay = default(TimeSpan))
        {
            if (null != EndDate)
            {
                // sanity check just in case. End date has time part
                EndDate = EndDate.Value.Date;
                TotalTimeoutDuration = EndDate.Value.AddDays(1) - StartDate.Value;

                // TODO: if end date is sat, calculate inclusive 1 day timespan in duration
                IncludeTimespan = TimeSpan.FromDays(1);
            }
            else
            {
                var now = SystemTime.Now();
                EndDate = now.Add(TotalTimeoutDuration + StartDelayTimeoutDurationy - timeOfDay).Date;
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

        protected TimeSpan IncludeTimespan { get; set; }

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

