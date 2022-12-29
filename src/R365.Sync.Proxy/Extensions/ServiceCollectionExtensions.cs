using System;
using Microsoft.Extensions.DependencyInjection;
using R365.Caching.Extensions;

namespace R365.Sync.Proxy.Extensions
{
    /// <summary>
    /// Extensions methods for adding support for R365.Sync.Service to your application
    ///  Note: This package requires that R365.Context and Microsoft.Extensions.Logging are already bootstrapped by your application
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services for communicating with R365 Sync Service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IServiceCollection AddR365SyncServiceProxy(this IServiceCollection services,
            Action<SyncServiceProxyOptions> optionsBuilder)
        {
            if (optionsBuilder == null)
			{
				throw new ArgumentNullException(nameof(optionsBuilder));
			}

			var options = new SyncServiceProxyOptions();
			optionsBuilder.Invoke(options);

			services.AddOptions();

			services.Configure<SyncServiceProxySettings>(o =>
			{
				o.BaseUri = options.BaseUri;
            });

			services.AddHttpClient<ISyncServiceProxy, SyncServiceHttpProxy>(c =>
            {
                c.BaseAddress = new Uri(Utils.HandleUrlSlash(options.BaseUri));
            });

            services.AddHttpClient<IHRSyncServiceHttpProxy, HRSyncServiceHttpProxy>(c =>
            {
                c.Timeout = TimeSpan.FromMinutes(10);
                c.BaseAddress = new Uri(Utils.HandleUrlSlash(options.BaseUri));
            });

            return services;
        }



        /// <summary>
        /// Adding R365 Caching lib - need to be called if you don't have R365 Caching registered in your project.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddR365SyncServiceCache(this IServiceCollection services)
        {
            services.AddCaching(o =>
            {
                o.EnableCacheStoreSynchronization = true;
            });
            return services;
        }
    }
}
