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
            Dictionary<string, string> testCases = new Dictionary<string, string>();

            testCases.Add("a1b2c3d4", "=2=afgHPDF/2TtEHeeqw2bzne2LiFhG/7ptGpoaRZB5XBE=");
            testCases.Add("some-much-longer-token", "=2=2JEyVUrokBKMzFLm5CVsnnyg4sYxeT6hjHft5iEbr7Q=");

            Parameter[] parameters = new Parameter[2]{
                new Parameter("Region", Parameter.Op.Equals, "Northwest"),
                new Parameter("EmployeeType", Parameter.Op.Equals, "Human Resources"),
            };
            
            foreach (KeyValuePair<string, string> testCase in testCases)
            {
                Assert.Equal(testCase.Value, TokenGenerator.Generate(testCase.Key, parameters));
            }

            Dictionary<string, string> testCases2 = new Dictionary<string, string>();

            Parameter[] parameters2 = new Parameter[2]{
                new Parameter("Name", Parameter.Op.NotEquals, "Bill"),
                new Parameter("Hobbies", Parameter.Op.Contains, new string[1]{"Fishing"}),
            };

            testCases2.Add("a1b2c3d4", "=2=wKwx8xIARl4CFVw1+nvPo/XmgrB+N7Fh6p5EkfwjQa0=");
            testCases2.Add("some-much-longer-token", "=2=aWWfIEUJBeP3Pz2Sd8EyyLkQ2q6Lx06v0mEXNMk68ls=");

            foreach (KeyValuePair<string, string> testCase in testCases2)
            {
                Assert.Equal(testCase.Value, TokenGenerator.Generate(testCase.Key, parameters2));
            }
        }
    }
}