using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TimoutDurationLab.Test
{
    public class Class1
    {
        private readonly ITestOutputHelper _testOutput;

        public Class1(ITestOutputHelper testOutput)
        {

            _testOutput = testOutput;
        }

        //[Fact]
        //public void Test1()
        //{
        //    _testOutput.WriteLine(typeof(TestCollectionOrderer).FullName);
        //    _testOutput.WriteLine(typeof(TestCollectionOrderer).Assembly.FullName);
        //}
    }
}
