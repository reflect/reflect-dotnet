using Jose;
using Newtonsoft.Json;
using System;

namespace Reflect.Tokens
{
    class NewtonsoftMapper : IJsonMapper
    {
        public T Parse<T>(string json)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }
    }
}
