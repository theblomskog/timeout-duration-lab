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

            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(40);
            Assert.Equal(expected, a.TimeoutDuration);
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
                SendReminder = true
            };

            a.Run();

            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            _testOutput.WriteLine(a.TimeoutDuration.ToString());
            var expected = TimeSpan.FromHours(28);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void NoStartDateEndDate_NoReminder_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity();

            a.Run();

            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            _testOutput.WriteLine(a.TimeoutDuration.ToString());

            var expected = TimeSpan.FromHours(40);
            Assert.Equal(expected, a.TimeoutDuration);
            SystemTime.Reset();
        }

        [Fact]
        public void NoStartDateEndDate_SendReminder_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                SendReminder = true
            };

            a.Run();

            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            _testOutput.WriteLine(a.TimeoutDuration.ToString());
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

            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            _testOutput.WriteLine(a.TimeoutDuration.ToString());

            var expected = TimeSpan.FromHours(35);
            Assert.Equal(expected, a.TimeoutDuration);
            SystemTime.Reset();
        }

        [Fact]
        public void StartDate_EndDate_SendReminder_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                StartDate = SystemTime.Now().Date,
                EndDate = SystemTime.Now().Date.AddDays(1),
                SendReminder = true
            };

            a.Run();

            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            _testOutput.WriteLine(a.TimeoutDuration.ToString());
            var expected = TimeSpan.FromHours(23);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Reset();
        }

        [Fact]
        public void NoStartDateEndDate_NoReminder_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity();

            a.Run();
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(35);
            Assert.Equal(expected, a.TimeoutDuration);
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
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(23);
            Assert.Equal(expected, a.TimeoutDuration);

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
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(4));
            Assert.Equal(expected, a.TimeoutDuration);

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
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(11).Add(TimeSpan.FromDays(4));
            Assert.Equal(expected, a.TimeoutDuration);

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
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(7));
            Assert.Equal(expected, a.TimeoutDuration);

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
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(11).Add(TimeSpan.FromDays(7));
            Assert.Equal(expected, a.TimeoutDuration);

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
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(11).Add(TimeSpan.FromDays(7));
            Assert.Equal(expected, a.TimeoutDuration);

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
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(7));
            Assert.Equal(expected, a.TimeoutDuration);

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
            _testOutput.WriteLine($"Start delay: {a.StartDelay}");

            var expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(7));
            Assert.Equal(expected, a.TimeoutDuration);

            expected = TimeSpan.FromHours(16).Add(TimeSpan.FromDays(2));
            Assert.Equal(expected, a.StartDelay);
            SystemTime.Reset();
        }
    }
}



