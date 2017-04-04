using Xunit;
using System;
using Reflect;

namespace Reflect.Tests
{
    public class ParameterTest
    {
        [Fact]
        public void TestParameterString()
        {
            Parameter p = new Parameter("Region", Parameter.Op.Equals, "Northwest");
            Console.WriteLine(p.ToString());
        }
    }
}