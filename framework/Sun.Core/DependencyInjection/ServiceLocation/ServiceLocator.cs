using System;

namespace Sun.Core.DependencyInjection.ServiceLocation
{
    /// <summary>
    /// 服务定位器
    /// </summary>
    public static class ServiceLocator
    {
        private static ServiceLocatorProvider currentProvider;


        public static IServiceLocator Current
        {
            get
            {
                if (currentProvider == null) throw new InvalidOperationException("ServiceLocationProvider必须初始化");

                return currentProvider();
            }
        }


        public static void SetLocatorProvider(ServiceLocatorProvider newProvider)
        {
            currentProvider = newProvider;
        }


    }
}
