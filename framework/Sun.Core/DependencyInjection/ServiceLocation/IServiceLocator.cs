using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Sun.Core.DependencyInjection.ServiceLocation
{
    public interface IServiceLocator
    {

        object Create(Type serviceType);
        TService Create<TService>();

        IEnumerable<object> Creates(Type serviceType);
        IEnumerable<TService> Creates<TService>();

        IServiceScope CreateScope();
    }
}
