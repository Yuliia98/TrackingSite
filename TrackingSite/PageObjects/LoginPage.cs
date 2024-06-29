using OpenQA.Selenium;

namespace TrackingSite.PageObjects
{
    public class LoginPage : BasePageUtils
    {
        private By UserNameField => By.CssSelector("#usrNmInput");
        private By PasswordField => By.Id("pswdInput");
        private By LoginButton => By.XPath(".//button[@id='loginBtn']");
        private By ErrorMessageLabel => By.Id("errMsgLbl");

        public LoginPage(IWebDriver driver, string url, string title) : base(driver, url, title)
        {
            _url = "https://qa.sorted.com/newtrack/login";
            _title = "Login";
        }

        public void GoToLoginPage()
        {
            _driver.Navigate().GoToUrl(_url);
            WaitForOpened();
        }

        public void EnterCredentials(string username, string password)
        {
            _driver.FindElement(UserNameField).SendKeys(username);
            _driver.FindElement(PasswordField).SendKeys(password);
        }

        public void ClickOnLoginButton() =>
            _driver.FindElement(LoginButton).Click();

        public void Login(string username, string password)
        {
            GoToLoginPage();
            EnterCredentials(username, password);
            ClickOnLoginButton();
        }

        public bool IsUserNameFieldDisplayed() =>
            _driver.FindElement(UserNameField).Displayed;

        public string GetErrorMessageText() =>
            _driver.FindElement(ErrorMessageLabel).Text;
    }
}
