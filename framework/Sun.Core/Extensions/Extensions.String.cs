using System;
using System.Linq;
using System.Text;

namespace Sun.Core.Extensions
{
    /// <summary>
    /// 系统扩展 - 字符串
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 过滤开头字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="trim"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static string TrimStart(this string source, string trim, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (source == null)
            {
                return null;
            }
            string s = source;
            while (s.StartsWith(trim, stringComparison))
            {
                s = s.Substring(trim.Length);
            }
            return s;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string ToDefault(this string value, string def)
        {
            if (string.IsNullOrEmpty(value))
            {
                return def;
            }
            return value;
        }

        /// <summary>
        /// 判断字符串是否为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 判断字符串是否不为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool NotNull(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 与字符串进行比较，忽略大小写
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string s, string value)
        {
            return s.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToLower() + s.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToUpper() + s.Substring(1);
            return str;
        }

        /// <summary>
        /// 转为Base64，UTF-8格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBase64(this string s)
        {
            return s.ToBase64(Encoding.UTF8);
        }

        /// <summary>
        /// 转为Base64
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToBase64(this string s, Encoding encoding)
        {
            if (s.IsNull())
                return string.Empty;
            var bytes = encoding.GetBytes(s);
            return bytes.ToBase64();
        }

        public static string ToPascalCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            var builder = new StringBuilder();
            bool wordBreaked = true;
            foreach (char c in s)
            {
                if (char.IsWhiteSpace(c) || c == '.' || c == '-' || c == '_')
                {
                    wordBreaked = true;
                    continue;
                }

                if (wordBreaked)
                {
                    builder.Append(char.ToUpper(c));
                    wordBreaked = false;
                }
                else if (char.IsUpper(c))
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append(char.ToLower(c));
                }
            }

            return builder.ToString();
        }

        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }
            return char.ToLower(s[0]).ToString() + s.ToPascalCase().Remove(0, 1);
        }

        public static string ToSnakeCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }
            string replacement = s.ToPascalCase();
            var builder = new StringBuilder();
            foreach (char c in replacement)
            {
                if (char.IsUpper(c))
                {
                    builder.Append('_');
                }
                builder.Append(char.ToLower(c));
            }
            return builder.ToString().TrimStart('_');
        }

        /// <summary>
        /// 该方法适用于 对象被转成了Json字符串,这个时候需要将属性名称格式化为 Snake格式, 属性值保持不变
        /// Tip: 属性值不能包含逗号,否则会出错
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSnakeCaseForObject(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            StringBuilder sb = new StringBuilder();

            //将字符串按逗号分隔
            string[] strPros = s.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (strPros.Length <= 0)
            {
                return s;
            }
            foreach (string pro in strPros)
            {
                string[] pros = pro.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                sb.Append(SnakeCaseSub(pros[0]));

                for (int i = 1; i < pros.Length; i++)
                {
                    if (!pros[i].Contains("{\""))
                    {
                        sb.Append($":{pros[i]}");
                    }
                    else
                    {
                        sb.Append($":{SnakeCaseSub(pros[i])}");
                    }
                }
                sb.Append(",");
            }

            return sb.ToString().TrimEnd(',');
        }

        private static string SnakeCaseSub(string pro)
        {
            string sn = pro.ToSnakeCase();
            int snIndex = sn.IndexOf("_");
            return $"{sn.Substring(0, snIndex)}{sn.Substring(snIndex + 1, sn.Length - snIndex - 1)}";
        }
    }
}
