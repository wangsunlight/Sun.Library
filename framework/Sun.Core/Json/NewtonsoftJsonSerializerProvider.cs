using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Sun.Core.Json
{
    /// <summary>
    ///  NewtonsoftJson 序列化提供器
    /// </summary>
    public class NewtonsoftJsonSerializerProvider : IJsonSerializerProvider
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        public string Serialize(object value, object jsonSerializerOptions = null)
        {
            var jsonSettings = new JsonSerializerSettings();
            if (jsonSerializerOptions != null)
            {
                jsonSettings = jsonSerializerOptions as JsonSerializerSettings;
            }
            jsonSettings.Converters.Add(new LongToStringConverter());
            return JsonConvert.SerializeObject(value, jsonSettings);
        }

        /// <summary>
        /// 反序列化字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json, object jsonSerializerOptions = null)
        {
            var jsonSettings = new JsonSerializerSettings();
            if (jsonSerializerOptions != null)
            {
                jsonSettings = jsonSerializerOptions as JsonSerializerSettings;
            }
            return JsonConvert.DeserializeObject<T>(json, jsonSettings);

        }
    }
}
