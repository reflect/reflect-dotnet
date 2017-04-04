using Xunit;
using System;
using Reflect;
using System.Collections.Generic;

namespace Reflect.Tests
{
    public class ParameterTest
    {
        [Fact]
        public void TestParameterString()
        {
            Console.WriteLine(TokenGenerator.Generate("something", new List<Parameter>{}));
        }
    }
}