﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Sun.Core.Json
{
    /// <summary>
    /// System.Text.Json 序列化提供器
    /// </summary>
    public class SystemTextJsonSerializerProvider : IJsonSerializerProvider
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        public string Serialize(object value, object jsonSerializerOptions = null)
        {
            return JsonSerializer.Serialize(value, (jsonSerializerOptions as JsonSerializerOptions)
                ?? GetDefaultJsonSerializerOptions());
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
            return JsonSerializer.Deserialize<T>(json, (jsonSerializerOptions as JsonSerializerOptions)
                ?? GetDefaultJsonSerializerOptions());
        }

        /// <summary>
        /// 默认配置
        /// </summary>
        /// <returns></returns>
        private static JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,   // 缩进
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,  // 中文乱码
                PropertyNameCaseInsensitive = true  // 忽略大小写
            };
        }
    }
}
