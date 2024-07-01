using OpenQA.Selenium;

namespace TrackingSite.PageObjects
{
    public class MainSitePage : BasePageUtils
    {
        private Dictionary<string, By> _elementAliases;
        private const string PageUrl = "/main";
        private const string PageTitle = "Main";
        public const string Dashboards = "Dashboards";
        public const string Charts = "Charts";
        public const string Tracking = "Tracking";

        public MainSitePage(IWebDriver driver) : base(driver, PageUrl, PageTitle)
        {
            //options of header menu in tracking site after login
            _elementAliases = new Dictionary<string, By>
            {
                { Dashboards, By.CssSelector("#tckDataBtn")},
                { Charts, By.Id("chartsBtn") },
                { Tracking, By.XPath(".//button[@id='trackTableBtn']") }
            };
        }

        public IWebElement GetElementByAlias(string alias)
        {
            if (_elementAliases.TryGetValue(alias, out By locator))
                return _driver.FindElement(locator);
            else throw new ArgumentException($"No element found with the alias '{alias}'");
        }
        public void ClickOnElementByAlias(string alias) => GetElementByAlias(alias).Click();
    }
}
