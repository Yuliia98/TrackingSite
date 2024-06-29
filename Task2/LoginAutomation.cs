using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Task2
{
    [TestFixture]
    public class LoginAutomation
    {
        private IWebDriver driver;
        private const string loginUrl = "https://qa.sorted.com/newtrack";

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void Login(string userName, string password)
        {
            driver.Navigate().GoToUrl(loginUrl);

            var userField = driver.FindElement(By.XPath(".//form[@id='loginForm']/input[1]"));
            var passwordField = driver.FindElement(By.XPath(".//form[@id='loginForm']/input[2]"));
            var loginButton = driver.FindElement(By.Id("submit"));

            userField.SendKeys(userName);
            passwordField.SendKeys(password);
            loginButton.Click();
        }

        [Test]
        [TestCase("john.smith@sorted.com", "Pa55w0rd!", "https://qa.sorted.com/newtrack/loginSuccess")]
        public void LoginTest(string userName, string password, string expectedUrl)
        {
            Login(userName,password);

            Assert.That(driver.Url, Is.EqualTo(expectedUrl), $"Wrong url is displayed after login as '{userName}' user");
        }

        [TearDown]
        public void TearDownDriver() =>
            driver.Quit();
    }
}
