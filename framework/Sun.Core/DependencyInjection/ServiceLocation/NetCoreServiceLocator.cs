using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Sun.Core.DependencyInjection.ServiceLocation
{
    public class NetCoreServiceLocator : IServiceLocator
    {
        private readonly IServiceProvider _provider;
        public NetCoreServiceLocator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public virtual IServiceScope CreateScope() => _provider.GetService<IServiceScopeFactory>().CreateScope();

        public IEnumerable<object> Creates(Type serviceType) => _provider.GetServices(serviceType);

        public IEnumerable<TService> Creates<TService>() => _provider.GetServices<TService>();

        public object Create(Type serviceType) => _provider.GetService(serviceType);

        public TService Create<TService>() => _provider.GetService<TService>();


    }
}
