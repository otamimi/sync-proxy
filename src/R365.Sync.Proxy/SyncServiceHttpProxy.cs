using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using R365.Caching;
using R365.Context;
using R365.Context.Extensions;
using R365.Exceptions;
using R365.Sync.Proxy.Models;
using R365.Sync.Proxy.Models.Sync;

namespace R365.Sync.Proxy
{
    /// <summary>
    /// Service responsible for the implementation of the proxy
    /// </summary>
    public class SyncServiceHttpProxy : ISyncServiceProxy
    {
        private readonly IContext _context;
        private readonly HttpClient _httpClient;
        private readonly ICachingService<List<DataProviderSubscription>> _dataProviderSubscriptionCache;

        /// <summary>
        /// Initializes a new instance of <see cref="SyncServiceHttpProxy"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpClient"></param>
        /// <param name="dataProviderSubscriptionCache"></param>
        public SyncServiceHttpProxy(
            IContext context,
            HttpClient httpClient,
            ICachingService<List<DataProviderSubscription>> dataProviderSubscriptionCache)
        {
            this._context = context;
            this._httpClient = httpClient;
            this._dataProviderSubscriptionCache = dataProviderSubscriptionCache;
        }

        public async Task DeleteDataProviderAsync(int dataProviderId)
        {
            if (dataProviderId == 0)
            {
                throw new ArgumentNullException(nameof(dataProviderId));
            }

            var response = await this
                .EnrichHttpClient()
                .DeleteAsync($"{DataProvider.RouteName}/{dataProviderId}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }
        }

        public async Task DeleteDataProviderSubscriptionAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await this._dataProviderSubscriptionCache.Pop(this._context.GetTenantId());

            var response = await this
                .EnrichHttpClient()
                .DeleteAsync($"{DataProviderSubscription.RouteName}/{id}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }
        }

        public async Task DeleteRecordMapPropertyMapAsync(long recordMapId, string sourcePropertyValue)
        {
            if (recordMapId == 0)
            {
                throw new ArgumentNullException(nameof(recordMapId));
            }

            if (string.IsNullOrWhiteSpace(sourcePropertyValue))
            {
                throw new ArgumentNullException(nameof(sourcePropertyValue));
            }

            var response = await this
                .EnrichHttpClient()
                .DeleteAsync($"{RecordMap.RouteName}/{recordMapId}/propertyMap/{sourcePropertyValue}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }
        }

        public async Task<List<DataProvider>> GetAllDataProvidersAsync()
        {

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"{DataProvider.RouteName}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<List<DataProvider>>();
        }

        public async Task<DataProvider> GetDataProviderAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"{DataProvider.RouteName}/{name}");

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }
            return await response.Content.ReadAsAsync<DataProvider>();
        }

        public async Task<DataProviderSubscription> GetDataProviderSubscriptionAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"{DataProviderSubscription.RouteName}/{id}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<DataProviderSubscription>();
        }

        public async Task<List<DataProviderSubscription>> GetAllDataProviderSubscriptionsAsync()
        {
             var dataProviderSubscription = await this._dataProviderSubscriptionCache.Get(this._context.GetTenantId(), async (key) => {
                var response = await this
                 .EnrichHttpClient()
                 .GetAsync($"{DataProviderSubscription.RouteName}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<DataProviderSubscription>();
                }
                if (!response.IsSuccessStatusCode)
                {
                    await ThrowExceptionForNonSuccessResponseAsync(response);
                }

                return await response.Content.ReadAsAsync<List<DataProviderSubscription>>();
            });

            return dataProviderSubscription;
        }

        public async Task<List<DataProviderSubscription>> GetDataProviderSubscriptionByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"{DataProviderSubscription.RouteName}?name={name}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<List<DataProviderSubscription>>();
        }

        public async Task<List<RecordMap>> GetRecordMapAsync(string providerName, string recordMapTypeName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentNullException(nameof(providerName));
            }

            if (string.IsNullOrWhiteSpace(recordMapTypeName))
            {
                throw new ArgumentNullException(nameof(recordMapTypeName));
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"{RecordMap.RouteName}/{providerName}/{recordMapTypeName}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<List<RecordMap>>();
        }

        public async Task SendChangeEventAsync(ChangeEvent changeEvent)
        {
            var response = await this
                .EnrichHttpClient()
                .PutAsJsonAsync($"{ChangeEvent.RouteName}", changeEvent);

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }
        }

        public async Task<IEnumerable<GeneralLedgerAccount>> GetAccountsAsync(string dataProviderName, int? startingPosition, int? maxCount)
        {
            if (string.IsNullOrWhiteSpace(dataProviderName))
            {
                throw new ArgumentNullException(nameof(dataProviderName));
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"accounting/{GeneralLedgerAccount.RouteName}/{dataProviderName}?startingPosition={startingPosition}&maxCount={maxCount}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<IEnumerable<GeneralLedgerAccount>>();
        }

        public async Task<GeneralLedgerAccountCount> GetAccountCountAsync(string dataProviderName)
        {
            if (string.IsNullOrWhiteSpace(dataProviderName))
            {
                throw new ArgumentNullException(nameof(dataProviderName));
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"accounting/{GeneralLedgerAccount.RouteName}/{dataProviderName}/count");

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<GeneralLedgerAccountCount>();
        }

        public async Task<GeneralLedgerAccount> UpsertAccountAsync(string dataProviderName, GeneralLedgerAccount newAccount)
        {
            if (string.IsNullOrWhiteSpace(dataProviderName))
            {
                throw new ArgumentNullException(nameof(dataProviderName));
            }

            if (string.IsNullOrWhiteSpace(newAccount.Name))
            {
                throw new ArgumentNullException(nameof(newAccount.Name));
            }

            if (string.IsNullOrWhiteSpace(newAccount.Type))
            {
                throw new ArgumentNullException(nameof(newAccount.Type));
            }

            var response = await this
                .EnrichHttpClient()
                .PutAsJsonAsync($"accounting/{GeneralLedgerAccount.RouteName}/{dataProviderName}/account", newAccount);

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<GeneralLedgerAccount>();
        }


        public async Task<DataProvider> UpsertDataProviderAsync(DataProvider dataProvider)
        {
            var response = await this
                .EnrichHttpClient()
                .PutAsJsonAsync($"{DataProvider.RouteName}", dataProvider);

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<DataProvider>();
        }

        public async Task<DataProviderSubscription> UpsertDataProviderAsync(DataProviderSubscription subscription)
        {
            await this._dataProviderSubscriptionCache.Pop(this._context.GetTenantId());

            var response = await this
                .EnrichHttpClient()
                .PutAsJsonAsync($"{DataProviderSubscription.RouteName}", subscription);

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<DataProviderSubscription>();
        }

        public async Task<RecordMap> UpsertRecordMapAsync(string providerName, RecordMap recordMap)
        {
            if (string.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentNullException(nameof(providerName), $"Cannot upsert {nameof(RecordMap)} without a {nameof(providerName)}");
            }

            var response = await this
                .EnrichHttpClient()
                .PutAsJsonAsync($"{RecordMap.RouteName}/{providerName}", recordMap);

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<RecordMap>();
        }


        public async Task<Uri> GetAuthorizationUrl(string provider, string customer)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(customer))
            {
                throw new ArgumentNullException(nameof(customer));
            }

            var response =  await this
                .EnrichHttpClient()
                .GetAsync($"authorize/oauth?provider={provider}&customer={customer}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return response.RequestMessage.RequestUri;
        }

        /// <summary>
        /// Proxy http call for when authorization is being completed and the callback needs to be invoked
        /// </summary>
        /// <param name="queryString">Required: Full escaped query string value. Likely read using HttpContext.Request.QueryString.ToString()</param>
        public async Task<string> GetAuthorizationCallback(string queryString)
        {
            if (string.IsNullOrWhiteSpace(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            if (!queryString.StartsWith("?"))
            {
                queryString = $"?{queryString}";
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"authorize/oauth/callback{queryString}");

            if (!response.IsSuccessStatusCode)
            {
               await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<Vendor>> GetVendorsAsync(string dataProviderName, int? startingPosition, int? maxCount)
        {
            if (string.IsNullOrWhiteSpace(dataProviderName))
            {
                throw new ArgumentNullException(nameof(dataProviderName));
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"accounting/{Vendor.RouteName}/{dataProviderName}?startingPosition={startingPosition}&maxCount={maxCount}");

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<IEnumerable<Vendor>>();
        }

        public async Task<VendorCount> GetVendorCountAsync(string dataProviderName)
        {
            if (string.IsNullOrWhiteSpace(dataProviderName))
            {
                throw new ArgumentNullException(nameof(dataProviderName));
            }

            var response = await this
                .EnrichHttpClient()
                .GetAsync($"accounting/{Vendor.RouteName}/{dataProviderName}/count");

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<VendorCount>();
        }

        public async Task<List<string>> RemoveJournalEntry(string journalEntryId)
        {

            if (!(await this.HasDataProviderSubscriptionsAsync()))
            {
                return new List<string>();
            }

            if (string.IsNullOrWhiteSpace(journalEntryId))
            {
                throw new ArgumentNullException(nameof(journalEntryId));
            }

            var response = await this
                .EnrichHttpClient()
                .DeleteAsync($"accounting/{"journalentry"}/{journalEntryId}");

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<List<string>>();
        }

        public async Task<bool> UpsertBasicAuth(string provider, Dictionary<string, string> credentialsDictionary)
        {

            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var response = await this
                .EnrichHttpClient()
                .PostAsJsonAsync($"authorize/{"basicAuth"}?provider={provider}", credentialsDictionary);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<bool>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SubscriptionSyncResult>> StartDataProviderSubscriptionSyncAsync<TPayload>(long subscriptionId, TPayload payload)
        {
            var response = await this
                .EnrichHttpClient()
                .PostAsJsonAsync($"dataprovider/subscription/{subscriptionId}/sync", payload);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<List<SubscriptionSyncResult>>();
        }

        /// <inheritdoc/>
        public async Task<SearchResult<SyncHistory>> SearchDataProviderSubscriptionSyncHistoryAsync(long subscriptionId, SyncSieveModel sieveModel)
        {
            var response = await this
                .EnrichHttpClient()
                .PostAsJsonAsync($"dataprovider/subscription/{subscriptionId}/syncHistory/search", sieveModel);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<SearchResult<SyncHistory>>();
        }

        /// <inheritdoc/>
        public async Task<SearchResult<SyncHistoryEntity>> SearchDataProviderSubscriptionSyncHistoryEntitiesAsync(long subscriptionId, string syncHistoryId, SyncSieveModel sieveModel)
        {
            var response = await this
                .EnrichHttpClient()
                .PostAsJsonAsync($"dataprovider/subscription/{subscriptionId}/syncHistory/{syncHistoryId}/entities/search", sieveModel);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForNonSuccessResponseAsync(response);
            }

            return await response.Content.ReadAsAsync<SearchResult<SyncHistoryEntity>>();
        }

        async private Task<bool> HasDataProviderSubscriptionsAsync()
        {
            var dataProviderSubscriptions = await this.GetAllDataProviderSubscriptionsAsync();

            return dataProviderSubscriptions.Count > 0;
        }

        /// <summary>
        /// Enrich http client headers from the data found in <see cref="IContext"/>
        /// </summary>
        /// <returns></returns>
        private HttpClient EnrichHttpClient()
        {
            var tenantId = this._context.GetTenantId();
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId), $"R365.Context has no {nameof(tenantId)} set, this value is required");
            }

            var userId = this._context.GetSecurityId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId), $"R365.Context has no {nameof(userId)} set, this value is required");
            }

            const string CustomerIdHeaderName = "x-r365-customer";
            const string UserIdHeaderName = "x-r365-user-id";

            if (this._httpClient.DefaultRequestHeaders.Contains(CustomerIdHeaderName))
                this._httpClient.DefaultRequestHeaders.Remove(CustomerIdHeaderName);

            this._httpClient.DefaultRequestHeaders.Add(CustomerIdHeaderName, tenantId);

            if (this._httpClient.DefaultRequestHeaders.Contains(UserIdHeaderName))
                this._httpClient.DefaultRequestHeaders.Remove(UserIdHeaderName);

            this._httpClient.DefaultRequestHeaders.Add(UserIdHeaderName, userId);

            return this._httpClient;
        }

        private async Task ThrowExceptionForNonSuccessResponseAsync(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            var SyncServiceException = JsonConvert.DeserializeObject<SyncServiceExceptionMetaData>(result);

            var finalToThrow = new R365Exception(SyncServiceException.Message)
            {
                ErrorCode = SyncServiceException.ErrorCode
            };
            throw finalToThrow;
        }

    }
}