using System;
using Xunit;

namespace Reflect.Tests
{
    public class ParameterTest
    {
        [Fact]
        public void TestParameterString()
        {
            Parameter p = new Parameter();
            Assert.Equal(p.ToString(), "string");

            string s = Reflect.Generator.GenerateToken("foo", new Parameter[]{});
            Assert.Equal(s, "string");
        }
    }
}
