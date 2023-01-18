using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using R365.Caching;
using R365.Context;
using R365.Context.Extensions;
using R365.Sync.Proxy.Models;

namespace R365.Sync.Proxy
{
    public class HRSyncServiceHttpProxy : IHRSyncServiceHttpProxy
    {
        private readonly HttpClient _httpClient;
        private readonly IContext _context;

        /// <summary>
        /// Initializes a new instance of <see cref="HRSyncServiceHttpProxy"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpClient"></param>
        /// <param name="locationsCache"></param>
        /// <param name="employeeCache"></param>
        /// <param name="jobCache"></param>
        public HRSyncServiceHttpProxy(
            IContext context,
            HttpClient httpClient,
            ICachingService<List<Location>> locationsCache,
            ICachingService<List<Employee>> employeeCache,
            ICachingService<List<Job>> jobCache)
        {
            this._httpClient          = httpClient.EnrichHttpClient(context);
            this._context             = context;

        }

        /// <summary>
        /// Get a list of all employees within the given provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task<List<Employee>> GetProviderEmployeesAsync(string providerName, IEnumerable<string> location)
        {

            var response = await this
                                 ._httpClient.EnrichHttpClient(_context)
                                 .GetAsync($"hr/{providerName}/employee/{location}");
            if (!response.IsSuccessStatusCode)
            {
                await Utils.ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<List<Employee>>();
        }

        /// <summary>
        /// Get a list of all Jobs within the given provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public async Task<List<Job>> GetProviderJobsAsync(string providerName)
        {
            var response = await this
                                 ._httpClient.EnrichHttpClient(_context)
                                 .GetAsync($"hr/{providerName}/job");
            if (!response.IsSuccessStatusCode)
            {
                await Utils.ThrowExceptionForNonSuccessResponseAsync(response);
            }
            return await response.Content.ReadAsAsync<List<Job>>();

        }

        /// <summary>
        /// Get a list of all locations within the given provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public async Task<List<Location>> GetProviderLocationsAsync(string providerName)
        {
            var response = await this
                                 ._httpClient.EnrichHttpClient(_context)
                                 .GetAsync($"hr/{providerName}/location");

            if (!response.IsSuccessStatusCode)
            {
                await Utils.ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<List<Location>>();
        }
    }
}