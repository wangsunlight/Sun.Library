using Sun.Core.DependencyInjection.ServiceLocation;
using Sun.Core.Json;

namespace Sun.Core.Extensions
{
    /// <summary>
    /// json转换扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);
            return ServiceLocator.Current.Create<IJsonSerializerProvider>().Deserialize<T>(json);
        }

        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="isConvertToSingleQuotes">是否将双引号转成单引号</param>
        public static string ToJson(this object target, bool isConvertToSingleQuotes = false)
        {
            if (target == null)
                return string.Empty;
            var result =  ServiceLocator.Current.Create<IJsonSerializerProvider>()?.Serialize(target); 
            if (isConvertToSingleQuotes)
                result = result?.Replace("\"", "'");
            return result;
        }
    }
}
