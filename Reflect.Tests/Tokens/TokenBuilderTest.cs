using Newtonsoft.Json;
using Reflect.Extensions;
using Reflect.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Reflect.Tests.Tokens
{
    public class TokenBuilderTest
    {
        // This can be kept around for debugging.
        private readonly ITestOutputHelper output;

        public TokenBuilderTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Simple()
        {
            var kp = new KeyPair();

            var token = new TokenBuilder(kp.AccessKey).Build(kp.SecretKey);

            var headers = Jose.JWT.Headers(token);
            Assert.Equal(kp.AccessKey, headers["kid"]);

            Jose.JWT.Decode(token, kp.SecretKeyBytes);
        }

        [Fact]
        public void ExpirationApplies()
        {
            var kp = new KeyPair();
            var expiration = DateTime.Now.AddMinutes(15);

            var token = new TokenBuilder(kp.AccessKey)
                .Expiration(expiration)
                .Build(kp.SecretKey);

            var headers = Jose.JWT.Headers(token);
            Assert.Equal(kp.AccessKey, headers["kid"]);

            var payload = Jose.JWT.Decode<Dictionary<string, object>>(token, kp.SecretKeyBytes);
            Assert.InRange((Int32)payload["iat"], 0, DateTimeOffset.Now.ToUnixTimeSeconds());
            Assert.InRange((Int32)payload["nbf"], 0, DateTimeOffset.Now.ToUnixTimeSeconds());
            Assert.Equal(
                expiration.AddTicks(-(expiration.Ticks % TimeSpan.TicksPerSecond)),
                DateTimeOffset.FromUnixTimeSeconds((Int32)payload["exp"]).DateTime.ToLocalTime());
        }

        [Fact]
        public void ClaimsIncluded()
        {
            var kp = new KeyPair();
            var parameter = new Parameter("user-id", Parameter.Op.Equal, "1234");

            var token = new TokenBuilder(kp.AccessKey)
                .AddViewIdentifier("SecUr3View1D")
                .SetAttribute("user-id", 1234)
                .SetAttribute("user-name", "Billy Bob")
                .AddParameter(parameter)
                .Build(kp.SecretKey);

            var headers = Jose.JWT.Headers(token);
            Assert.Equal(kp.AccessKey, headers["kid"]);

            var payload = Jose.JWT.Decode<Dictionary<string, object>>(token, kp.SecretKeyBytes);
            Assert.Equal(new string[] { "SecUr3View1D" }, payload[TokenBuilder.ViewIdentifiersClaimName]);
            Assert.Collection(
                (payload[TokenBuilder.ParametersClaimName] as ArrayList).Cast<object>().ToList(),
                p1 => Assert.Equal(new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(JsonConvert.SerializeObject(parameter)), p1));
            Assert.Equal(new Dictionary<string, object>()
            {
                { "user-id", 1234 },
                { "user-name", "Billy Bob" },
            }, payload[TokenBuilder.AttributesClaimName]);
        }

        class KeyPair
        {
            public readonly string AccessKey = new Guid().ToString("D");
            public readonly string SecretKey = new Guid().ToString("D");
            public readonly byte[] SecretKeyBytes;

            public KeyPair()
            {
                SecretKeyBytes = Guid.ParseExact(SecretKey, "D").ToRFC4122ByteArray();
            }
        }
    }
}
