using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace TrackingSite.PageObjects
{
    public class BasePageUtils
    {
        protected IWebDriver _driver;
        protected string _baseUrl;
        protected string _pageUrl;
        protected string _pageTitle;
        private WebDriverWait _wait;

        public BasePageUtils(IWebDriver driver, string pageUrl, string pageTitle)
        {
            _driver = driver;
            _baseUrl = ConfigHelper.GetBaseUrl();
            _pageUrl = _baseUrl + pageUrl;
            _pageTitle = pageTitle;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateTo() =>
            _driver.Navigate().GoToUrl(_pageUrl);

        public bool IsPageOpenedByTitle(string expectedTitle) =>
            _driver.Title.Equals(expectedTitle);

        public bool IsPageOpenedByUrl(string expectedUrl) =>
            _driver.Url.Equals(expectedUrl);

        public void WaitForOpened() =>
            _wait.Until(driver => IsPageOpenedByUrl(_pageUrl) && IsPageOpenedByTitle(_pageTitle));
    }
}
