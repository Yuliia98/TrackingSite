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
            _loginPage = new LoginPage(_driver);
            _mainSitePage = new MainSitePage(_driver);
            _trackingTablePage = new TrackingTablePage(_driver);
        }


        [Given("I log in as (.*) user")]
        public void LoginAs(string userAlias)
        {
            User user = GetUserEntityByName(userAlias);
            _loginPage.Login(user.UserName, user.Password);
        }

        [Given("I am on login screen")]
        public void OpenLoginPage() =>
            _loginPage.NavigateTo();

        [When("I navigate to (.*) page from main site")]
        public void NavigateToPageFromMainSite(string pageAlias) =>
            _mainSitePage.ClickOnElementByAlias(pageAlias);

        [When("I enter a credentials for (.*) user and submit")]
        public void EnterCredentialsForUserAndClickSubmit(string userAlias)
        {
            User user = GetUserEntityByName(userAlias);
            _loginPage.EnterCredentials(user.UserName, user.Password);
            _loginPage.ClickOnLoginButton();
        }

        [Then("Login page is displayed")]
        public void VerifyLoginPageIsDisplayed()
        {
            _loginPage.WaitForOpened();
            Assert.IsTrue(_loginPage.IsUserNameFieldDisplayed(), $"Login element is not prsent on page!");
        }

        [Then("Tracking page is displayed")]
        public void VerifyTrackingTablePageIsDisplayed()
        {
            _trackingTablePage.WaitForOpened();
             Assert.IsTrue(_trackingTablePage.IsTrackingTableDisplayed(), $"Table is not prsent on Tracking page!");
        }

        [Then("I am logged in successfully")]
        public void VerifySuccessfullLogin()
        {
            _mainSitePage.WaitForOpened();
            _mainSitePage.ClickOnElementByAlias("Tracking");
            VerifyTrackingTablePageIsDisplayed();
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