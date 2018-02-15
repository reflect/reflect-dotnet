using Jose;
using Reflect.Extensions;
using System;
using System.Collections.Generic;

namespace Reflect.Tokens
{
    public class TokenBuilder
    {
        public const string ViewIdentifiersClaimName = "http://reflect.io/s/v3/vid";
        public const string ParametersClaimName = "http://reflect.io/s/v3/p";
        public const string AttributesClaimName = "http://reflect.io/s/v3/a";

        private readonly JwtSettings settings = new JwtSettings()
        {
            JsonMapper = new NewtonsoftMapper(),
        };

        private string accessKey;
        private DateTime? expiration = null;

        private IList<string> viewIdentifiers = new List<string>();
        private IList<Parameter> parameters = new List<Parameter>();
        private IDictionary<string, object> attributes = new Dictionary<string, object>();

        public TokenBuilder(string accessKey)
        {
            this.accessKey = accessKey;
        }

        public TokenBuilder Expiration(DateTime when)
        {
            expiration = when;
            return this;
        }

        public TokenBuilder AddViewIdentifier(string id)
        {
            viewIdentifiers.Add(id);
            return this;
        }

        public TokenBuilder AddParameter(Parameter parameter)
        {
            parameters.Add(parameter);
            return this;
        }

        public TokenBuilder SetAttribute(string name, object value)
        {
            attributes[name] = value;
            return this;
        }

        public string Build(string secretKey)
        {
            var secretKeyBytes = Guid.ParseExact(secretKey, "D").ToRFC4122ByteArray();

            var now = DateTimeOffset.Now;

            var payload = new Dictionary<string, object>() {
                {"iat", now.ToUnixTimeSeconds()},
                {"nbf", now.ToUnixTimeSeconds()},
            };

            if (expiration.HasValue)
            {
                payload["exp"] = new DateTimeOffset(expiration.Value).ToUnixTimeSeconds();
            }

            if (viewIdentifiers.Count > 0)
            {
                payload[ViewIdentifiersClaimName] = viewIdentifiers;
            }

            if (parameters.Count > 0)
            {
                payload[ParametersClaimName] = parameters;
            }

            if (attributes.Count > 0)
            {
                payload[AttributesClaimName] = attributes;
            }

            var headers = new Dictionary<string, object>() {
                {"cty", "JWT"},
                {"kid", accessKey},
            };

            return JWT.Encode(
                payload: payload,
                key: secretKeyBytes,
                alg: JweAlgorithm.DIR,
                enc: JweEncryption.A128GCM,
                compression: JweCompression.DEF,
                extraHeaders: headers,
                settings: settings);
        }
    }
}
