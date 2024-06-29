using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace TrackingSite.PageObjects
{
    public class BasePageUtils
    {
        public IWebDriver _driver;
        public WebDriverWait _wait;
        public string _url;
        public string _title;

        public BasePageUtils(IWebDriver driver, string url, string title)
        {
            _driver = driver;
            _url = url;
            _title = title;
        }

        public bool IsPageOpenedByTitle(string expectedTitle) =>
            _driver.Title.Equals(expectedTitle);

        public bool IsPageOpenedByUrl(string expectedUrl) =>
            _driver.Url.Equals(expectedUrl);

        public void WaitForOpened()
        {
            if (!(IsPageOpenedByUrl(_url) && IsPageOpenedByTitle(_title)))
                throw new TimeoutException($"Page with matching title or URL not found. Title: {_title}, Url: {_url}");
        }
    }
}
