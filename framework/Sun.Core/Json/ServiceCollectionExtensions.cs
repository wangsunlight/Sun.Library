using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace Sun.Core.Json
{
    /// <summary>
    /// JSON扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 系统统一注入基类相关
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTextJson(this IServiceCollection services)
        {
            services.AddSingleton<IJsonSerializerProvider, SystemTextJsonSerializerProvider>();
            return services;
        }

        /// <summary>
        /// 系统统一注入基类相关
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNewtonsoftJson(this IServiceCollection services)
        {
            services.AddSingleton<IJsonSerializerProvider, NewtonsoftJsonSerializerProvider>();
            return services;
        }
    }
}
