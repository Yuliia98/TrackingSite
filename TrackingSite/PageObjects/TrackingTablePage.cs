using OpenQA.Selenium;

namespace TrackingSite.PageObjects
{
    public class TrackingTablePage : BasePageUtils
    {
        private By Table => By.CssSelector("#trckTable");
        private const string PageUrl = "/main";
        private const string PageTitle = "Main";

        public TrackingTablePage(IWebDriver driver) : base(driver, PageUrl, PageTitle)
        {

        }

        public bool IsTrackingTableDisplayed() => _driver.FindElement(Table).Displayed;

        public bool IsTrackingPageOpened() =>
            IsPageOpenedByUrl(_pageUrl);
    }
}
