using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TimoutDurationLab.Test
{
    public class TestCollectionOrderer : ITestCaseOrderer
    {
        public const string TypeName = "TimoutDurationLab.Test.TestCollectionOrderer";
        public const string AssemblyName = "TimoutDurationLab.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var sortedMethods = new SortedDictionary<int, TTestCase>();


            throw new NotImplementedException();
        }

        
    }
}
