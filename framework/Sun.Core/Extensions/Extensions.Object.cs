using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sun.Core.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 把对象类型转换为指定类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object CastTo(this object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }
            if (conversionType.IsNullableType())
            {
                conversionType = conversionType.GetUnNullableType();
            }
            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString());
            }
            if (conversionType == typeof(Guid))
            {
                return Guid.Parse(value.ToString());
            }
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// 把对象类型转化为指定类型
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败引发异常。 </returns>
        public static T CastTo<T>(this object value)
        {
            if (value == null || default(T) == null)
            {
                return default(T);
            }
            if (value.GetType() == typeof(T))
            {
                return (T)value;
            }
            object result = CastTo(value, typeof(T));
            return (T)result;
        }

        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回指定的默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="defaultValue"> 转化失败返回的指定默认值 </param>
        /// <returns> 转化后的指定类型对象，转化失败时返回指定的默认值 </returns>
        public static T CastTo<T>(this object value, T defaultValue)
        {
            try
            {
                return CastTo<T>(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将对象[主要是匿名对象]转换为dynamic
        /// </summary>
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            Type type = value.GetType();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
            foreach (PropertyDescriptor property in properties)
            {
                var val = property.GetValue(value);
                if (property.PropertyType.FullName != null && property.PropertyType.FullName.StartsWith("<>f__AnonymousType"))
                {
                    dynamic dval = val.ToDynamic();
                    expando.Add(property.Name, dval);
                }
                else
                {
                    expando.Add(property.Name, val);
                }
            }
            return (ExpandoObject)expando;
        }

        /// <summary>
        /// 合并两个字典
        /// </summary>
        /// <param name="dic">字典</param>
        /// <param name="newDic">新字典</param>
        /// <returns></returns>
        public static Dictionary<string, object> AddOrUpdate(this Dictionary<string, object> dic, Dictionary<string, object> newDic)
        {
            foreach (var key in newDic.Keys)
            {
                if (dic.ContainsKey(key))
                    dic[key] = newDic[key];
                else
                    dic.Add(key, newDic[key]);
            }

            return dic;
        }

        /// <summary>
        /// 合并两个字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic">字典</param>
        /// <param name="newDic">新字典</param>
        /// <returns></returns>
        public static Dictionary<string, T> AddOrUpdate<T>(this Dictionary<string, T> dic, Dictionary<string, T> newDic)
        {
            foreach (var key in newDic.Keys)
            {
                if (dic.ContainsKey(key))
                    dic[key] = newDic[key];
                else
                    dic.Add(key, newDic[key]);
            }

            return dic;
        }
        /// <summary>
        /// 获取字典值,没有则返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            if (key is null)
                return default;
            if (dic.TryGetValue(key, out var value))
                return value;

            return default;
        }
        /// <summary>
        /// 将对象转字典集合
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, TValue> ToDictionary<TValue>(this object obj)
        {
            var dic = new Dictionary<string, TValue>();

            // 如果对象为空，则返回空字典
            if (obj == null) return dic;

            // 如果不是类类型或匿名类型，则返回空字典
            var type = obj.GetType();
            if (!(type.IsClass || type.IsAnonymous())) return dic;

            // 获取所有属性
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 如果实例公开属性为空，则返回空字典
            if (properties.Length == 0) return dic;

            // 遍历公开属性
            foreach (var property in properties)
            {
                object value = property.GetValue(obj);
                if (null != value)
                {
                    dic.Add(property.Name, (TValue)value);
                }
            }

            return dic;

        }

        /// <summary>
        /// 判断是否是匿名类型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        internal static bool IsAnonymous(this object obj)
        {
            var type = obj.GetType();

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && type.Attributes.HasFlag(TypeAttributes.NotPublic);
        }

        /// <summary>
        /// 将对象拼接成QueryString中&=的字符串格式
        /// </summary>
        /// <param name="model">处理的对象</param>
        /// <returns></returns>
        public static string MakeUrlParameter(this object model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<string, object> dictionary = model.ToDictionary<object>();
            foreach (string key in dictionary.Keys)
            {
                object value = dictionary[key];
                if (value != null)
                {
                    if (value.GetType().IsGenericType)
                    {
                        var type = value.GetType().GetGenericArguments()[0];

                        var str = value.ListToUrlParameter(key, type);
                        stringBuilder.Append((stringBuilder.Length > 0 ? "&" : "") + str);
                    }
                    else
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={value}");
                    }
                }
            }

            return stringBuilder.ToString();
        }



        #region 将list集合转成URL参数形式

        /// <summary>
        /// 将list集合转成URL参数形式.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="key">参数key</param>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <returns></returns>
        public static string ListToUrlParameter(this object value, string key, System.Type conversionType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (null != value)
            {
                if (conversionType.FullName == typeof(string).FullName)
                {
                    foreach (var item in value as IList<string>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(int).FullName)
                {
                    foreach (var item in value as IList<int>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(byte).FullName)
                {
                    foreach (var item in value as IList<byte>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(short).FullName)
                {
                    foreach (var item in value as IList<short>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(long).FullName)
                {
                    foreach (var item in value as IList<long>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(DateTime).FullName)
                {
                    foreach (var item in value as IList<DateTime>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(Guid).FullName)
                {
                    foreach (var item in value as IList<Guid>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(bool).FullName)
                {
                    foreach (var item in value as IList<bool>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(char).FullName)
                {
                    foreach (var item in value as IList<char>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(decimal).FullName)
                {
                    foreach (var item in value as IList<decimal>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(double).FullName)
                {
                    foreach (var item in value as IList<double>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(float).FullName)
                {
                    foreach (var item in value as IList<float>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(sbyte).FullName)
                {
                    foreach (var item in value as IList<sbyte>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(uint).FullName)
                {
                    foreach (var item in value as IList<uint>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(ulong).FullName)
                {
                    foreach (var item in value as IList<ulong>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(ushort).FullName)
                {
                    foreach (var item in value as IList<ushort>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
                else if (conversionType.FullName == typeof(object).FullName)
                {
                    foreach (var item in value as IList<object>)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append($"{key}={item}");
                    }
                }
            }
            return stringBuilder.ToString();
        }

        #endregion 将list集合转成URL参数形式
    }
}