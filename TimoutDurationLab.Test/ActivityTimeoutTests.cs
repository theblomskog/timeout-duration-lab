using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TimoutDurationLab.Test
{
    // Add CollectionAttribute to run test in sequence
    [Collection("MyCollection")]
    public class ActivityTimeoutTests
    {
        private readonly ITestOutputHelper _testOutput;

        public ActivityTimeoutTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public void TimeS_12Hour_NoRem()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromHours(12),
            };
            a.Run();

            _testOutput.WriteLine($"Duration: {a.TimeoutDuration}");
            _testOutput.WriteLine($"StartDate: {a.StartDate}");
            _testOutput.WriteLine($"EndDate: {a.EndDate}");

            Assert.Equal(TimeSpan.FromHours(12), a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void TimeS_12Hour_SendRem_6Hour()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromHours(12),
                ReminderTimeoutDuration = TimeSpan.FromHours(6),
                SendReminder = true
            };

            a.Run();

            _testOutput.WriteLine($"Duration: {a.TimeoutDuration}");
            _testOutput.WriteLine($"StartDate: {a.StartDate}");
            _testOutput.WriteLine($"EndDate: {a.EndDate}");

            Assert.Equal(TimeSpan.FromHours(6), a.TimeoutDuration);

            SystemTime.Reset();
        }


        [Fact]
        public void TimeS_7Day_SendRem_3Day_00()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 00:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
                ReminderTimeoutDuration = TimeSpan.FromDays(3),
                SendReminder = true
            };
            a.Run();


            var expected = TimeSpan.FromDays(4);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-05 00:00:00"));

            // Reminder should have been sent and sat to true at 2017-01-05 00:00:00
            // update ReminderSent
            Assert.True(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromDays(3);
            Assert.Equal(expected, a.TimeoutDuration);


            SystemTime.Reset();
        }

        [Fact]
        public void TimeS_3Day_SendRem_12Hour()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(3),
                ReminderTimeoutDuration = TimeSpan.FromHours(12),
                SendReminder = true
            };
            a.Run();


            var expected = TimeSpan.FromHours(4).Add(TimeSpan.FromDays(2));
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-03 18:00:00"));

            // Reminder should have been sent and sat to true at 2017-01-03 12:00:00
            // update ReminderSent
            Assert.True(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromHours(6);
            Assert.Equal(expected, a.TimeoutDuration);


            SystemTime.Reset();
        }

        [Fact]
        public void TimeS_1Day_SendRem_12Hour()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 11:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(1),
                ReminderTimeoutDuration = TimeSpan.FromHours(12),
                SendReminder = true
            };
            a.Run();

            var expected = TimeSpan.FromHours(1);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-01 20:00:00"));

            // Reminder should have been sent and sat to true at 12:00:00
            // update ReminderSent
            Assert.True(a.ShouldSendReminder());
            a.Update();


            expected = TimeSpan.FromHours(4);
            Assert.Equal(expected, a.TimeoutDuration);


            SystemTime.Reset();
        }

        [Fact]
        public void StartDT_EndDateDT_NoRem()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                StartDate = SystemTime.Now().Date,
                EndDate = SystemTime.Now().Date.AddDays(1)
            };

            a.Run();

            var expected = TimeSpan.FromHours(40);

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Reset();
        }

        [Fact]
        public void StartDT_EndDT_SendRem_12Hour()
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

            var expected = TimeSpan.FromHours(28);
            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);

            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            Assert.False(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromHours(23);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-02 13:00:00"));

            // update ReminderSent
            Assert.True(a.ShouldSendReminder());
            a.Update();

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
        public void TimeS_7Day_SendRem_3Day()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
                ReminderTimeoutDuration = TimeSpan.FromDays(3),
                SendReminder = true
            };

            a.Run();

            var expected = TimeSpan.FromHours(11).Add(TimeSpan.FromDays(3));
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-04 13:00:00"));

            Assert.False(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromHours(11);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-05 13:00:00"));

            // update ReminderSent
            Assert.True(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromDays(2).Add(TimeSpan.FromHours(11));
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void TimeS_7Day_NoRem()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
            };

            a.Run();

            var expected = TimeSpan.FromDays(6).Add(TimeSpan.FromHours(11));
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-07 13:00:00"));

            Assert.False(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromHours(11);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void TimeS_7Day_SendRem_0Hour()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                TotalTimeoutDuration = TimeSpan.FromDays(7),
                ReminderTimeoutDuration = default(TimeSpan),
                SendReminder = true
            };

            a.Run();

            var expected = TimeSpan.FromDays(6).Add(TimeSpan.FromHours(11));

            Assert.Equal(expected, a.TimeoutDuration);
            Assert.Equal(default(TimeSpan), a.StartDelayTimeoutDurationy);
            Assert.Equal(default(TimeSpan), a.ReminderTimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-07 13:00:00"));

            Assert.False(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromHours(11);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void StartDT_1_EndDT_2_SendRem_12Hour()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                StartDate = DateTime.Parse("2017-01-01 00:00:00"),
                EndDate = DateTime.Parse("2017-01-02 00:00:00"),
                ReminderTimeoutDuration = TimeSpan.FromHours(12),
                SendReminder = true
            };

            a.Run();


            var expected = TimeSpan.FromDays(1).Add(TimeSpan.FromHours(4));
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-02 08:00:00"));

            // Reminder should not yet have been sent
            Assert.False(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromHours(4);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void StartDT_in3Day_TimeS_7Day_SendRem_3Day()
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

            var expected = TimeSpan.FromDays(2).Add(TimeSpan.FromHours(16));
            Assert.Equal(expected, a.StartDelayTimeoutDurationy);

            expected = TimeSpan.FromDays(4);
            Assert.Equal(expected, a.TimeoutDuration);

            expected = TimeSpan.FromDays(3);
            Assert.Equal(expected, a.ReminderTimeoutDuration);


            SystemTime.Set(DateTime.Parse("2017-01-04 06:00:00"));

            a.Update();

            Assert.True(a.IsStarted);

            expected = TimeSpan.FromDays(3).Add(TimeSpan.FromHours(18));
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-08 08:00:00"));

            // update ReminderSent
            Assert.True(a.ShouldSendReminder());
            a.Update();

            expected = TimeSpan.FromDays(2).Add(TimeSpan.FromHours(16));
            Assert.Equal(expected, a.TimeoutDuration);

            //_testOutput.WriteLine($"StartDate: {a.StartDate}");
            //_testOutput.WriteLine($"EndDate: {a.EndDate}");

            SystemTime.Set(DateTime.Parse("2017-01-10 12:00:00"));

            a.Update();

            expected = TimeSpan.FromHours(12);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }
    }
}
