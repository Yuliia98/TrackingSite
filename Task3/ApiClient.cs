using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Task3
{
    public class ApiClient
    {
        private readonly RestClient _client;

        public ApiClient(string baseUrl) =>
            _client = new RestClient(baseUrl);

        public void SetAuthenticationHeader(string token) =>
            _client.AddDefaultHeader("Authorization", $"Bearer {token}");

        public List<FaileLoginResponseEntity> GetFailedLoginHistoriesAsync(string userName = null, int? failCount = null, int? fetchLimit = null)
        {
            var response = CallGetFailedLoginHistoriesAsync(userName, failCount, fetchLimit);
            VerifyResponseIsOK(response);
                
            return JsonConvert.DeserializeObject<List<FaileLoginResponseEntity>>(response.Result.Content);
        }

        public async Task<RestResponse> CallGetFailedLoginHistoriesAsync(string userName = null, int? failCount = null, int? fetchLimit = null)
        {
            var request = new RestRequest("loginfailtotal", Method.Get);

            if (!string.IsNullOrEmpty(userName))
                request.AddParameter("user_name", userName);
            if (failCount.HasValue)
                request.AddParameter("fail_count", failCount.Value);
            if (fetchLimit.HasValue)
                request.AddParameter("fetch_limit", fetchLimit.Value);

            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> ResetFailedLoginCountAsync(string username)
        {
            var request = new RestRequest("resetloginfailtotal", Method.Put);
            request.AddJsonBody(new { Username = username });
            return await _client.ExecuteAsync(request);
        }

        private void VerifyResponseIsOK(Task<RestResponse> response)
        {
            if (response.Result.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Status code:'{response.Result.StatusCode}', " +
                    $"Error message:'{response.Result.ErrorMessage ?? "No Message"}', Content:'{response.Result.Content}'");
        }
    }
}
