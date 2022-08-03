using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sun.Core.DependencyInjection.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSun.Core.Helper
{
    /// <summary>
    /// 配置文件帮助器
    /// </summary>
    public static partial class App
    {
        /// <summary>
        /// 获取服务名
        /// </summary>
        /// <returns></returns>
        public static string GetServiceName()
        {
            var configuration = ServiceLocator.Current.Create<IConfiguration>();
            return configuration["applicationName"]?.Replace(".", "-").ToLower();
        }

        /// <summary>
        /// 获取服务端口号
        /// </summary>
        /// <returns></returns>
        public static string GetPort()
        {
            var configuration = ServiceLocator.Current.Create<IConfiguration>();
            return configuration["urls"]?.Split(':').Last();
        }

        /// <summary>
        /// 获取环境名称
        /// </summary>
        /// <returns></returns>
        public static string GetEnvironmentName()
        {
            var hostEnvironment = ServiceLocator.Current.Create<IHostEnvironment>();
            return hostEnvironment.EnvironmentName;
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <returns>TOptions</returns>
        public static TOptions GetOptions<TOptions>()
            where TOptions : class, new()
        {
            return ServiceLocator.Current.Create<IOptions<TOptions>>()?.Value;
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <returns>TOptions</returns>
        public static TOptions GetOptionsMonitor<TOptions>()
            where TOptions : class, new()
        {
            return ServiceLocator.Current.Create<IOptionsMonitor<TOptions>>()?.CurrentValue;
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <returns>TOptions</returns>
        public static TOptions GetOptionsSnapshot<TOptions>()
            where TOptions : class, new()
        {
            return ServiceLocator.Current.Create<IOptionsSnapshot<TOptions>>()?.Value;
        }


    }
}
