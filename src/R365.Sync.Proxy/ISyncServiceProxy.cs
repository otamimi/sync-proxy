using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using R365.Sync.Proxy.Models;
using R365.Sync.Proxy.Models.Sync;

namespace R365.Sync.Proxy
{
    public interface ISyncServiceProxy
    {
        // authorization
        Task<System.Uri> GetAuthorizationUrl(string provider, string customer);

        Task<string> GetAuthorizationCallback(string queryString);

        Task<bool> UpsertBasicAuth(string provider, Dictionary<string, string> credentialsDictionary);

        //data provider
        Task<DataProvider> UpsertDataProviderAsync(DataProvider dataProvider);

        Task<List<DataProvider>> GetAllDataProvidersAsync();

        Task<DataProvider> GetDataProviderAsync(string name);

        Task DeleteDataProviderAsync(int dataProviderId);

        //data provider subscription
        Task<DataProviderSubscription> GetDataProviderSubscriptionAsync(long id);

        Task<List<DataProviderSubscription>> GetAllDataProviderSubscriptionsAsync();

        Task<List<DataProviderSubscription>> GetDataProviderSubscriptionByNameAsync(string name);

        Task<DataProviderSubscription> UpsertDataProviderAsync(DataProviderSubscription subscription);

        Task DeleteDataProviderSubscriptionAsync(long id);

        //record map
        Task<List<RecordMap>> GetRecordMapAsync(string providerName, string recordMapTypeName);

        Task<RecordMap> UpsertRecordMapAsync(string providerName, RecordMap recordMap);

        Task DeleteRecordMapPropertyMapAsync(long recordMapId, string sourcePropertyValue);

        //change event
        Task SendChangeEventAsync(ChangeEvent changeEvent);

        //accounts
        Task<IEnumerable<GeneralLedgerAccount>> GetAccountsAsync(string dataProviderName, int? startingPosition, int? maxCount);

        Task<GeneralLedgerAccountCount> GetAccountCountAsync(string dataProviderName);

        Task<GeneralLedgerAccount> UpsertAccountAsync(string dataProviderName, GeneralLedgerAccount newAccount);

        Task<IEnumerable<Vendor>> GetVendorsAsync(string dataProviderName, int? startingPosition, int? maxCount);

        Task<VendorCount> GetVendorCountAsync(string dataProviderName);
        Task<List<string>> RemoveJournalEntry(string journalEntryId);

        /// <summary>
        /// Executes a batch sync for the specified subscription, with the specified parameter
        /// payload.
        /// </summary>
        /// <remarks>
        /// Currently this always completes execution synchronously and returns the result. A future
        /// implementation may start the sync and continue it as a background process on the Sync
        /// Service.
        /// </remarks>
        /// <param name="subscriptionId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        Task<IEnumerable<SubscriptionSyncResult>> StartDataProviderSubscriptionSyncAsync<TPayload>(long subscriptionId, TPayload payload);

        /// <summary>
        /// Searches recent sync history data for the specified subscription.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="sieveModel"></param>
        /// <returns></returns>
        Task<SearchResult<SyncHistory>> SearchDataProviderSubscriptionSyncHistoryAsync(long subscriptionId, SyncSieveModel sieveModel);

        /// <summary>
        /// Searches entity state for a specific sync history entry. Entity data includes errors for
        /// each entity that failed to sync.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="syncHistoryId"></param>
        /// <param name="sieveModel"></param>
        /// <returns></returns>
        Task<SearchResult<SyncHistoryEntity>> SearchDataProviderSubscriptionSyncHistoryEntitiesAsync(long subscriptionId, string syncHistoryId, SyncSieveModel sieveModel);
    }
}