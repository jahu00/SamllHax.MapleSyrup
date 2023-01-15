using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SamllHax.MapleSyrup.Components;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public class ObjectFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ObjectFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Create<T>()
        {
            var instance = ActivatorUtilities.CreateInstance<T>(_serviceProvider);
            return instance;
        }
    }
}
