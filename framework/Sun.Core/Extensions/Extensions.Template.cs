﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Sun.Core.Extensions
{
    /// <summary>
    /// 模板操作静态类
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 模板正则表达式
        /// </summary>
        private const string templatePattern = @"\{(?<p>.+?)\}";

        /// <summary>
        /// 渲染模板
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateData"></param>
        /// <param name="encode"></param>
        public static void Render(this string template, object templateData, bool encode = false)
        {
            template.Render(templateData.ToDictionary<object>(), encode);
        }

        /// <summary>
        /// 渲染模板
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateData"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Render(this string template, Dictionary<string, object> templateData, bool encode = false)
        {
            // 如果模板为空，则跳过
            if (templateData == null || templateData.Count == 0) return template;

            // 判断请求地址是否包含模板
            if (!Regex.IsMatch(template, templatePattern)) return template;

            // 获取所有匹配的模板
            var templateValues = Regex.Matches(template, templatePattern)
                                                       .Select(u => new
                                                       {
                                                           Template = u.Groups["p"].Value,
                                                           Value = MatchTemplateValue(u.Groups["p"].Value, templateData)
                                                       });

            // 循环替换模板
            foreach (var item in templateValues)
            {
                template = template.Replace($"{{{item.Template}}}", encode ? HttpUtility.UrlEncode(item.Value?.ToString() ?? string.Empty) : item.Value?.ToString());
            }

            return template;
        }

        /// <summary>
        /// 匹配模板值
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateData"></param>
        /// <returns></returns>
        private static object MatchTemplateValue(string template, Dictionary<string, object> templateData)
        {
            string tmpl;
            if (!template.Contains(".", StringComparison.CurrentCulture)) tmpl = template;
            else tmpl = template.Split('.', StringSplitOptions.RemoveEmptyEntries).First();

            var templateValue = templateData.ContainsKey(tmpl) ? templateData[tmpl] : default;
            return ResolveTemplateValue(template, templateValue);
        }

        /// <summary>
        /// 解析模板的值
        /// </summary>
        /// <param name="template"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static object ResolveTemplateValue(string template, object data)
        {
            // 根据 . 分割模板
            var propertyCrumbs = template.Split('.', StringSplitOptions.RemoveEmptyEntries);
            return GetValue(propertyCrumbs, data);

            // 静态本地函数
            static object GetValue(string[] propertyCrumbs, object data)
            {
                if (data == null || propertyCrumbs == null || propertyCrumbs.Length <= 1) return data;
                var dataType = data.GetType();

                // 如果是基元类型则直接返回
                if (dataType.IsRichPrimitive()) return data;
                object value = null;

                // 递归获取下一级模板值
                for (int i = 1; i < propertyCrumbs.Length; i++)
                {
                    var propery = dataType.GetProperty(propertyCrumbs[i]);
                    if (propery == null) break;

                    value = propery.GetValue(data);
                    if (i + 1 < propertyCrumbs.Length)
                    {
                        value = GetValue(propertyCrumbs.Skip(i).ToArray(), value);
                    }
                    else break;
                }

                return value;
            }
        }
    }
}
