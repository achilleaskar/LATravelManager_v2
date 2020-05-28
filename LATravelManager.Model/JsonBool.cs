using System;
using Newtonsoft.Json;

namespace LaWeb.Shared.Helpers
{
    public class JsonBool
    {
        public class NumericStringToBooleanConverter : JsonConverter
        {
            public override bool CanRead => true;
            public override bool CanWrite => false;

            public override bool CanConvert(Type objectType) => objectType == typeof(int);

            public override object ReadJson(JsonReader reader, Type objectType,
                object existingValue, JsonSerializer serializer)
            {
                return reader.Value.Equals(1);
            }

            public override void WriteJson(JsonWriter writer, object value,
                JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}