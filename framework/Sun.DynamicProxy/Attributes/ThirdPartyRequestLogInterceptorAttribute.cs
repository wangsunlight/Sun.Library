using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sun.Core.Extensions;
using Sun.DynamicProxy.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.DynamicProxy.Attributes
{
    /// <summary>
    /// 调用第三方接口耗时等日志记录
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ThirdPartyRequestLogInterceptorAttribute : AbstractInterceptorAttribute
    {
        public ThirdPartyRequestLogInterceptorAttribute(string httpMethod, string formBodyParamName = "")
        {
            this.Method = httpMethod;
            this.FormBodyParamName = formBodyParamName;
        }
        public string Method { get; set; }
        public string FormBodyParamName { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            DataApiStatisticsEntity model = new DataApiStatisticsEntity();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                model.Function = $"{context.ProxyMethod.DeclaringType?.Name}.{context.ProxyMethod.Name}";
                model.Url = context.Parameters.ToJson(); ;
                model.HttpType = Method;
                var formBodyParamName = FormBodyParamName;
                if (model.HttpType.ToLower() == "post" && !string.IsNullOrEmpty(formBodyParamName))
                {
                    model.Parameter = context.Parameters.ToJson();
                }
                else
                {
                    model.Parameter = "url地址上";
                }
                await next(context);

                model.HttpState = 200;
                model.Message = "成功";
            }
            catch (Exception ex)
            {
                model.HttpState = 500;
                model.Message = $"[报错]:{ex.Message},[Stack]:{ex.StackTrace}";
                throw;
            }
            sw.Stop();
            model.Time = sw.ElapsedMilliseconds;

            var logger = context.ServiceProvider.GetService<ILogger>();
            logger?.LogDebug($"[DATA-API-WEB]:{model.ToJson()}");

        }
    }
}
