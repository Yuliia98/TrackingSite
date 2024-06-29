using NUnit.Framework;
using System.Net;

namespace Task3
{
    [TestFixture]
    public class APITests
    {
        private ApiClient _apiClient;
        private const string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ";

        [OneTimeSetUp]
        public void Setup()
        {
            _apiClient = new ApiClient("https://qa.sorted.com/newtrack/api/");

            //if it's required
            _apiClient.SetAuthenticationHeader(Token);
        }

        #region PositiveScenarios

        [Test]
        [TestCase(null, null, null)]
        [TestCase("valid_user", null, null)]
        [TestCase("valid_user", 5, null)]
        [TestCase(null, 6, null)]
        [TestCase(null, 7, 10)]
        [TestCase("valid_user", 8, 99)]
        public void GetFailedLoginHistories_ValidParameterCombinations(string userName, int? failCount, int? fetchLimit)
        {
            var listOfEntites = _apiClient.GetFailedLoginHistoriesAsync(userName, failCount, fetchLimit);

            if (fetchLimit != null)
                Assert.IsTrue(listOfEntites.Count <= fetchLimit, $"More than {fetchLimit} results were returned in response!");

            if (userName != null)
            {
                //we can't have duplicated users in the system
                Assert.IsTrue(listOfEntites.Count.Equals(1), $"More than 1 result was returned for {userName} user!");
                Assert.IsTrue(listOfEntites.FirstOrDefault().UserName.Equals(userName), $"Incorrect user name was returned for request with following parameters: UserName - {userName}, FailCount - {failCount}, FetchLimit - {fetchLimit}");
            }

            if (failCount != null)
            {
                foreach (var entity in listOfEntites)
                {
                    Assert.IsTrue(entity.FailCount >= failCount, $"FailCount value should be above '{failCount}' for entity with {entity.UserName} user name!");
                }
            }
        }

        //scenario to check that after resetting the failedlogincount value for user we receive 0 in response
        [Test]
        public async Task ResetFailedLoginCount_ValidateGetFailedLoginHistories()
        {
            //added condition for valid_user as it's used in other tests and need to have history
            var entityToReset = _apiClient.GetFailedLoginHistoriesAsync().FirstOrDefault(ent => ent.UserName != "valid_user" && ent.FailCount > 0);

            if (entityToReset == null)
                throw new Exception("There is no user to reset failed login count!");

            var response = await _apiClient.ResetFailedLoginCountAsync(entityToReset.UserName);
            Assert.IsTrue(response.IsSuccessful, $"Request failed! Error messages: {response.ErrorMessage}");

            var expectedEntites = _apiClient.GetFailedLoginHistoriesAsync(entityToReset.UserName);
            Assert.IsTrue(expectedEntites.FirstOrDefault().FailCount.Equals(0), $"Incorrect FailCount value for {entityToReset.UserName} user");

        }

        #endregion

        #region NegativeScenarios

        //in case we know maximum value of fetchLimit parameter (e.g. max is 99 results)
        [Test]
        [TestCase(-1)]
        [TestCase(100)]
        public async Task GetFailedLoginHistories_InvalidFetchLimit(int fetchLimit)
        {
            var response = await _apiClient.CallGetFailedLoginHistoriesAsync(fetchLimit: fetchLimit);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        [TestCase("", null, "abc")]
        [TestCase("<script>", 4, 10)]
        public async Task GetFailedLoginHistories_InvalidParameterCombination(string userName, int? failCount, string fetchLimit)
        {
            int? limit = int.TryParse(fetchLimit, out var parsedLimit) ? parsedLimit : null;
            var response = await _apiClient.CallGetFailedLoginHistoriesAsync(userName, failCount, limit);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        [TestCase("/invalid_user!")]
        public async Task ResetFailedLoginCount_InvalidParameter(string userName)
        {
            var response = await _apiClient.ResetFailedLoginCountAsync(userName);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        [TestCase("")]
        public async Task ResetFailedLoginCount_EmptyParameter(string userName)
        {
            var response = await _apiClient.ResetFailedLoginCountAsync(userName);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task ResetFailedLoginCount_DoubleAction()
        {
            //added condition for valid_user as it's used in other tests and need to have history
            var entityToReset = _apiClient.GetFailedLoginHistoriesAsync().FirstOrDefault(ent => ent.UserName != "valid_user");

            if (entityToReset == null)
                throw new Exception("There is no user to reset failed login count!");

            var response1 = await _apiClient.ResetFailedLoginCountAsync(entityToReset.UserName);
            Assert.IsTrue(response1.IsSuccessful, $"Request failed! Error messages: {response1.ErrorMessage}");

            var response2 = await _apiClient.ResetFailedLoginCountAsync(entityToReset.UserName);
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.NoContent).Or.EqualTo(HttpStatusCode.NotFound), $"Wrong status code while performing reset call for {entityToReset.UserName} user");

        }

        #endregion
    }
}
