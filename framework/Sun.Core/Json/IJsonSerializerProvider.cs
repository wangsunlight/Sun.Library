﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sun.Core.Json
{
    /// <summary>
    /// Json 序列化提供器
    /// </summary>
    public interface IJsonSerializerProvider
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        string Serialize(object value, object jsonSerializerOptions = default);

        /// <summary>
        /// 反序列化字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        T Deserialize<T>(string json, object jsonSerializerOptions = default);
    }
}
