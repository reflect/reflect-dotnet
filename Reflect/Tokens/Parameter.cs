using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflect.Tokens
{
    class ParameterJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Parameter).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var parameter = value as Parameter;
            writer.WriteStartObject();

            writer.WritePropertyName("field");
            serializer.Serialize(writer, parameter.Field);

            writer.WritePropertyName("op");
            serializer.Serialize(writer, parameter.Operation.Value);

            var values = parameter.Values.ToList();
            if (values.Count > 1)
            {
                writer.WritePropertyName("any");
                serializer.Serialize(writer, values);
            }
            else
            {
                writer.WritePropertyName("value");
                serializer.Serialize(writer, values.FirstOrDefault());
            }
        }
    }

    [JsonConverter(typeof(ParameterJsonConverter))]
    public class Parameter
    {
        public class Op
        {
            public static readonly Op Equal = new Op("=");
            public static readonly Op NotEqual = new Op("!=");
            public static readonly Op GreaterThan = new Op(">");
            public static readonly Op GreaterThanOrEqual = new Op(">=");
            public static readonly Op LessThan = new Op("<");
            public static readonly Op LessThanOrEqual = new Op("<=");
            public static readonly Op Contains = new Op("~=");
            public static readonly Op NotContains = new Op("!~");

            private Op(string value)
            {
                Value = value;
            }

            public string Value
            {
                get;
                private set;
            }

            public override string ToString()
            {
                return Value;
            }
        }

        private readonly string field;
        private readonly Op operation;
        private readonly IEnumerable<string> values;

        public Parameter(string field, Op operation, string value)
            : this(field, operation, new string[] { value }) { }

        public Parameter(string field, Op operation, IEnumerable<string> values)
        {
            this.field = field;
            this.operation = operation;
            this.values = values;
        }

        public string Field { get { return field; } }
        public Op Operation { get { return operation; } }
        public IEnumerable<string> Values { get { return values; } }
    }
}
