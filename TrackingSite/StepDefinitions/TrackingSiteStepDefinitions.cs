using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using TrackingSite.Entities;
using TrackingSite.PageObjects;

namespace TrackingSite.StepDefinitions
{
    [Binding]
    public class TrackingSiteStepDefinitions
    {
        public IWebDriver _driver;
        public string _url;
        public string _title;
        public LoginPage _loginPage;
        public MainSitePage _mainSitePage;
        public TrackingTablePage _trackingTablePage;

        [BeforeScenario]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _loginPage = new LoginPage(_driver, _url, _title);
            _mainSitePage = new MainSitePage(_driver, _url, _title);
            _trackingTablePage = new TrackingTablePage(_driver, _url, _title);
        }


        [Given("I log in as (.*) user")]
        public void LoginAs(string userAlias)
        {
            User user = GetUserEntityByName(userAlias);
            _loginPage.Login(user.UserName, user.Password);
        }

        [When("I navigate to (.*) page from main site")]
        public void NavigateToPageFromMainSite(string pageAlias) =>
            _mainSitePage.ClickOnElementByAlias(pageAlias);

        [Then("Login page is displayed")]
        public void VerifyLoginPageIsDisplayed()
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(_loginPage.IsPageOpenedByUrl(_loginPage._url), $"Current URL is not as expected!");
                Assert.IsTrue(_loginPage.IsUserNameFieldDisplayed(), $"Login element is not prsent on page!");
            });
        }

        [Then("Tracking page is displayed")]
        public void VerifyTrackingTablePageIsDisplayed()
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(_trackingTablePage.IsPageOpenedByUrl(_trackingTablePage._url), $"Current URL is not as expected!");
                Assert.IsTrue(_trackingTablePage.IsTrackingTableDisplayed(), $"Table is not prsent on Tracking page!");
            });
        }

        [Then("'([^']*)' error message is displayed on Login page")]
        public void VerifyErrorMessageIsDisplayedOnLoginPage(string message)
        {
            var actualError = _loginPage.GetErrorMessageText();
            Assert.That(actualError, Is.EqualTo(message), "Incorrect error message is displayed on Login page!");
        }

        [AfterScenario]
        public void TearDownDriver() =>
            _driver.Quit();

        private User GetUserEntityByName(string user)
        {

            string jsonFilePath = Path.Combine(Environment.CurrentDirectory, "Credentials.json");
            string jsonData = File.ReadAllText(jsonFilePath);

            Dictionary<string, User> users = JsonConvert.DeserializeObject<Dictionary<string, User>>(jsonData);

            return users[user];
        }
    }
}