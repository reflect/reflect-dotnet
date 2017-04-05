using Xunit;
using Reflect;

namespace Reflect.Tests
{
    public class ParameterTest
    {
        private class TestCase
        {
            public string secretKey;
            public Parameter[] parameters;
            public string expectedTokenValue;

            public TestCase(string sk, Parameter[] ps, string etv)
            {
                secretKey = sk;
                parameters = ps;
                expectedTokenValue = etv;
            }
        }

        private static Parameter[] parameters1 = new Parameter[2]
        {
            new Parameter("Region", Parameter.Op.Equals, "Northwest"),
            new Parameter("Employee Type", Parameter.Op.Equals, "Human Resources"),
        };

        private static Parameter[] parameters2 = new Parameter[2]
        {
            new Parameter("Name", Parameter.Op.NotEquals, "Bill"),
            new Parameter("Hobbies", Parameter.Op.Contains, new string[1]{"Fishing"}),
        };

        private static TestCase[] testCases = new TestCase[4]
        {
            new TestCase("a1b2c3d4", parameters1, "=2=mD8u93SxbcwoZfqtYrNNlf6vGxLWW/TyCQ3Pj5gI+Bk="),
            new TestCase("some-much-longer-token", parameters1, "=2=yGGBNT5ADaZlWJlQcut8GeR5SZ7oSaw+4vkG8XsXntE="),
            new TestCase("a1b2c3d4", parameters2, "=2=wKwx8xIARl4CFVw1+nvPo/XmgrB+N7Fh6p5EkfwjQa0="),
            new TestCase("some-much-longer-token", parameters2, "=2=aWWfIEUJBeP3Pz2Sd8EyyLkQ2q6Lx06v0mEXNMk68ls="),
        };

        [Fact]
        public void TestParameterString()
        {
            foreach (TestCase testCase in testCases)
            {
                Assert.Equal(
                    testCase.expectedTokenValue,
                    TokenGenerator.Generate(testCase.secretKey, testCase.parameters)
                );
            }
        }
    }
}