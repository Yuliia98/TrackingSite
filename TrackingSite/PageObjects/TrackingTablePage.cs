using OpenQA.Selenium;

namespace TrackingSite.PageObjects
{
    public class TrackingTablePage : BasePageUtils
    {
        private By Table => By.CssSelector("#trckTable");

        public TrackingTablePage(IWebDriver driver, string url, string title) : base(driver, url, title)
        {
            _url = "https://qa.sorted.com/newtrack/trackingTable";
            _title = "Tracking";
        }

        public bool IsTrackingTableDisplayed() => _driver.FindElement(Table).Displayed;
    }
}
