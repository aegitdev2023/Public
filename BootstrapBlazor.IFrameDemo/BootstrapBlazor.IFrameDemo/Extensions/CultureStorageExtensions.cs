using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    internal static class CultureStorageExtensions
    {
        /// <summary>
        /// 添加本地化持久化策略服务
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCultureStorage(this IServiceCollection services)
        {
            services.TryAddSingleton<ICultureStorage, DefaultCultureStorage>();
            return services;
        }

        internal class DefaultCultureStorage : ICultureStorage
        {
            public CultureStorageMode Mode { get; set; }
        }
    }
}
