using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Sun.Core.DependencyInjection
{
    /// <summary>
    /// 作用域
    /// </summary>
    public static class ServiceScopeExtensions
    {

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        public static T Create<T>(this IServiceScope scope)
        {
            return scope.ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type">对象类型</param>
        public static object Create(this IServiceScope scope, Type type)
        {
            return scope.ServiceProvider.GetService(type);
        }
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type">对象类型</param>
        public static IEnumerable<object> Creates(this IServiceScope scope, Type type)
        {
            return scope.ServiceProvider.GetServices(type);
        }
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type">对象类型</param>
        public static IEnumerable<T> Creates<T>(this IServiceScope scope)
        {
            return scope.ServiceProvider.GetServices<T>();
        }
    }
}
