using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Sun.Core.Json
{
    /// <summary>
    /// bigint 的id 转字符串
    /// </summary>
    public class LongToStringConverter : JsonConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jt = JValue.ReadFrom(reader);
            long.TryParse(jt.Value<string>(), out long id);
            return id;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(System.Int64).Equals(objectType) || typeof(System.Nullable<Int64>).Equals(objectType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}