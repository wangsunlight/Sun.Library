using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sun.Core.Extensions;
using Sun.Core.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.DynamicProxy.Attributes
{
    /// <summary>
    /// 通用的service层异常捕捉,返回值固定 ExecutionResultOup或者ExecutionResultOup<T>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ExceptionInterceptorAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var logger = context.ServiceProvider.GetService<ILogger>();
                var serviceResult = context.ServiceProvider.GetService<IServiceResult>();
                if (serviceResult != null)
                {
                    serviceResult.code = 999;
                    serviceResult.msg = ex.Message;
                    context.ReturnValue = serviceResult;
                }
                else
                {
                    context.ReturnValue = new ServiceResult()
                    {
                        code = 999,
                        msg = ex.Message
                    };
                }
                logger?.LogError(ex, $"[方法报错]:[方法名]:{context.ProxyMethod.DeclaringType?.Name}.{context.ProxyMethod.Name} \r\n [参数]:{context.Parameters.ToJson()}");
            }
        }
    }
}
