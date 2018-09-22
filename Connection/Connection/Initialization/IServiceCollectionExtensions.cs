using Connection.Common;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Connection.Initialization
{
    internal static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddObserver<T>(this IServiceCollection services)
        {
            return services.AddSingleton<IObserverComposite<T>, ObserverComposite<T>>()
                           .AddSingleton<IObserver<T>>(provider => provider.GetService<IObserverComposite<T>>());
        }
    }
}
