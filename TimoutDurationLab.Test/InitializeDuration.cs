using System;
using Xunit;
using Xunit.Abstractions;

namespace TimoutDurationLab.Test
{
    // Add CollectionAttribute to run test in sequence
    [Collection("MyCollection")]
    public class InitializeDuration
    {
        private readonly ITestOutputHelper _testOutput;

        public InitializeDuration(ITestOutputHelper testOutput)
        {

            _testOutput = testOutput;
        }

        [Fact]
        public void StartDate_EndDate_NoReminder_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                StartDate = SystemTime.Now().Date,
                EndDate = SystemTime.Now().Date.AddDays(1)
            };

            a.Run();

            //_testOutput.WriteLine("StartDate: {0}", a.StartDate);
            //_testOutput.WriteLine("EndDate: {0}", a.EndDate);
            //_testOutput.WriteLine("TotalTimeoutDuration: {0}", a.TotalTimeoutDuration);
            //_testOutput.WriteLine("ReminderTimeoutDuration: {0}", a.ReminderTimeoutDuration);

            var expected = TimeSpan.FromHours(40);

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Reset();
        }

        [Fact]
        public void StartDate_EndDate_SendReminder_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                StartDate = SystemTime.Now().Date,
                EndDate = SystemTime.Now().Date.AddDays(1),
                SendReminder = true,
                ReminderTimeoutDuration = TimeSpan.FromHours(12)
                
            };

            a.Run();

            var expected = TimeSpan.FromHours(4);
            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            a.ReminderSent = true;
            a.Call();

            expected = TimeSpan.FromHours(11);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void NoTimeoutOrDate_InvalidOperation()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {

            };

            var ex = Assert.Throws<InvalidOperationException>(() => a.Run());

            Assert.Equal("Timeoutduration is negative", ex.Message);

            SystemTime.Reset();

        }

        [Fact]
        public void NoStartDateEndDate_SendReminder_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                SendReminder = true,
                TotalTimeoutDuration = TimeSpan.FromDays(4),
                ReminderTimeoutDuration = TimeSpan.FromDays(1)
            };

            a.Run();

            var expected = TimeSpan.FromHours(28);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void StartDate_EndDate_NoReminder_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                StartDate = SystemTime.Now().Date,
                EndDate = SystemTime.Now().Date.AddDays(1)
            };

            a.Run();

            var expected = TimeSpan.FromHours(35);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-01 18:00:00"));

            a.Call();

            expected = TimeSpan.FromHours(30);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void StartDate_EndDate_SendReminder_11()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 11:00:00"));

            var a = new Activity
            {
                StartDate = SystemTime.Now().Date,
                EndDate = SystemTime.Now().Date.AddDays(1),
                SendReminder = true,
                ReminderTimeoutDuration = TimeSpan.FromHours(12)
            };

            a.Run();

            var expected = TimeSpan.FromHours(1);

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            a.ReminderSent = true;
            a.Call();

            expected = TimeSpan.FromHours(11);

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);


            SystemTime.Reset();
        }

        [Fact]
        public void NoStartDateEndDate_NoReminder_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity();

            a.Run();

            var expected = TimeSpan.FromHours(35);

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Reset();
        }

        [Fact]
        public void NoStartDateEndDate_SendReminder_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                SendReminder = true
            };

            a.Run();

            var expected = TimeSpan.FromHours(23);

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Reset();
        }

        [Fact]
        public void Days7_Reminder3_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
                ReminderTimeoutDuration = TimeSpan.FromDays(3),
                SendReminder = true
            };

            a.Run();

            var expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(4));

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Reset();
        }

        [Fact]
        public void Days7_Reminder3_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
                ReminderTimeoutDuration = TimeSpan.FromDays(3),
                SendReminder = true
            };

            a.Run();

            var expected = TimeSpan.FromHours(11).Add(TimeSpan.FromDays(4));

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Reset();
        }

        [Fact]
        public void Days7_NoReminder_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
            };

            a.Run();

            var expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(7));

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Reset();
        }

        [Fact]
        public void Days7_NoReminder_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
            };

            a.Run();

            var expected = TimeSpan.FromHours(11).Add(TimeSpan.FromDays(7));

            Assert.Equal(expected, a.TimeoutDuration);
          
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);
            Assert.Equal(TimeSpan.FromHours(12), a.ReminderTimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void Days7_Reminder_0_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
                ReminderTimeoutDuration = default(TimeSpan),
                SendReminder = true
            };

            a.Run();

            var expected = TimeSpan.FromHours(11).Add(TimeSpan.FromDays(7));

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);
            Assert.Equal(default(TimeSpan), a.ReminderTimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void Days7_Reminder_0_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
                ReminderTimeoutDuration = default(TimeSpan),
                SendReminder = true
            };

            a.Run();

            var expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(7));

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);
            Assert.Equal(default(TimeSpan), a.ReminderTimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void Start3_Duration7_Reminder3_08_StartDelay()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                StartDate = DateTime.Parse("2017-01-04 00:00:00"),
                TotalTimeoutDuration = TimeSpan.FromDays(7),
                ReminderTimeoutDuration = TimeSpan.FromDays(3),
                SendReminder = true
            };

            a.Run();

            var expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(2));
            Assert.Equal(expected, a.StartDelayTimeoutDurationy);

            expected = TimeSpan.FromDays(4);
            Assert.Equal(expected, a.TimeoutDuration);

            expected = TimeSpan.FromDays(3);
            Assert.Equal(expected, a.ReminderTimeoutDuration);


            SystemTime.Set(DateTime.Parse("2017-01-04 06:00:00"));

            a.Call();

            Assert.True(a.IsStarted);

            _testOutput.WriteLine("StartDate: {0}", a.StartDate);
            _testOutput.WriteLine("EndDate: {0}", a.EndDate);

            expected = TimeSpan.FromDays(4) - TimeSpan.FromHours(6);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }
    }
}



