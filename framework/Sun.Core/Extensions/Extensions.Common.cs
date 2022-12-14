using System.Collections.Generic;
using System.Linq;
using Sun.Core.Helper;

namespace Sun.Core.Extensions
{
    /// <summary>
    /// 系统扩展 - 公共
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 安全获取值，当值为null时，不会抛出异常
        /// </summary>
        /// <param name="value">可空值</param>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <param name="instance">枚举实例</param>
        public static int Value(this System.Enum instance)
        {
            if (instance == null)
                return 0;
            return EnumHelper.GetValue(instance.GetType(), instance);
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="instance">枚举实例</param>
        public static TResult Value<TResult>(this System.Enum instance)
        {
            if (instance == null)
                return default(TResult);
            return ConvertHelper.To<TResult>(Value(instance));
        }

        /// <summary>
        /// 获取枚举描述,使用System.ComponentModel.Description特性设置描述
        /// </summary>
        /// <param name="instance">枚举实例</param>
        public static string Description(this System.Enum instance)
        {
            if (instance == null)
                return string.Empty;
            return EnumHelper.GetDescription(instance.GetType(), instance);
        }

        /// <summary>
        /// 转换为Base64
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            return System.Convert.ToBase64String(bytes);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? list)
        {
            if (list != null)
            {
                return !list.Any();
            }

            return true;
        }

    }

  
}
