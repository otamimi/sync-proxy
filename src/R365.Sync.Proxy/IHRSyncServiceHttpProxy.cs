using System.Collections.Generic;
using System.Threading.Tasks;
using R365.Sync.Proxy.Models;

namespace R365.Sync.Proxy
{
    public interface IHRSyncServiceHttpProxy
    {
        /// <summary>
        /// Get a list of all employees within the given provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        Task<List<Employee>> GetProviderEmployeesAsync(string providerName, IEnumerable<string> location);

        /// <summary>
        /// Get a list of all Jobs within the given provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        Task<List<Job>> GetProviderJobsAsync(string providerName);

        /// <summary>
        /// Get a list of all locations within the given provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        Task<List<Location>> GetProviderLocationsAsync(string providerName);
    }
}