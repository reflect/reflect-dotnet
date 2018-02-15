using Newtonsoft.Json;
using Reflect.Tokens;
using Xunit;

namespace Reflect.Tests.Tokens
{
    public class ParameterTest
    {
        [Fact]
        public void SerializesToJSONWithOneValue()
        {
            var parameter = new Parameter("Test", Parameter.Op.Equal, "Value");
            var serialized = JsonConvert.SerializeObject(parameter);

            var expected = JsonConvert.SerializeObject(new
            {
                field = "Test",
                op = "=",
                value = "Value",
            });
            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void SerializesToJSONWithManyValues()
        {
            var parameter = new Parameter("Test", Parameter.Op.Equal, new string[] { "Value1", "Value2" });
            var serialized = JsonConvert.SerializeObject(parameter);

            var expected = JsonConvert.SerializeObject(new
            {
                field = "Test",
                op = "=",
                any = new string[] { "Value1", "Value2" },
            });
            Assert.Equal(expected, serialized);
        }
    }
}
