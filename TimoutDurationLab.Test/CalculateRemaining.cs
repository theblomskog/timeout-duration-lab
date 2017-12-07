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
    public class CalculateRemaining
    {
        private readonly ITestOutputHelper _testOutput;

        public CalculateRemaining(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public void Test_08()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 08:00:00"));

            var a = new Activity
            {
                SendReminder = true
            };
            a.InitializeDuration();

            var expected = TimeSpan.FromHours(28);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-01 18:00:00"));

            a.CalculateRemainingTimeoutDuration();

            expected = TimeSpan.FromHours(18);
            Assert.Equal(expected, a.TimeoutDuration);


            SystemTime.Reset();
        }

        [Fact]
        public void Test_13()
        {
            SystemTime.Set(DateTime.Parse("2017-01-01 13:00:00"));

            var a = new Activity
            {
                SendReminder = true
            };
            a.InitializeDuration();

            var expected = TimeSpan.FromHours(23);
            Assert.Equal(expected, a.TimeoutDuration);

            SystemTime.Set(DateTime.Parse("2017-01-01 20:00:00"));

            a.CalculateRemainingTimeoutDuration();

            expected = TimeSpan.FromHours(16);
            Assert.Equal(expected, a.TimeoutDuration);


            SystemTime.Reset();
        }
    }
}
