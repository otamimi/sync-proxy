using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using R365.Context;
using R365.Context.Extensions;
using R365.Exceptions;
using R365.Sync.Proxy.Models;

namespace R365.Sync.Proxy
{
    internal static class Utils
    {
        public static async Task ThrowExceptionForNonSuccessResponseAsync(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            var SyncServiceException = JsonConvert.DeserializeObject<SyncServiceExceptionMetaData>(result);

            var finalToThrow = new R365Exception(SyncServiceException.Message)
            {
                ErrorCode = SyncServiceException.ErrorCode
            };
            throw finalToThrow;
        }

        internal static HttpClient EnrichHttpClient(this HttpClient httpClient, IContext context)
        {
            var tenantId = context.GetTenantId();
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId), $"R365.Context has no {nameof(tenantId)} set, this value is required");
            }

            var userId = context.GetSecurityId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId), $"R365.Context has no {nameof(userId)} set, this value is required");
            }

            const string CustomerIdHeaderName = "x-r365-customer";
            const string UserIdHeaderName = "x-r365-user-id";

            if (httpClient.DefaultRequestHeaders.Contains(CustomerIdHeaderName))
            {
                httpClient.DefaultRequestHeaders.Remove(CustomerIdHeaderName);
            }

            httpClient.DefaultRequestHeaders.Add(CustomerIdHeaderName, tenantId);

            if (httpClient.DefaultRequestHeaders.Contains(UserIdHeaderName))
            {
                httpClient.DefaultRequestHeaders.Remove(UserIdHeaderName);
            }

            httpClient.DefaultRequestHeaders.Add(UserIdHeaderName, userId);

            return httpClient;
        }

        /// <summary>
        /// appends forward slash to uri if needed
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public static string HandleUrlSlash(string baseUri) => !baseUri.EndsWith("/") ? baseUri += "/" : baseUri;
    }
}