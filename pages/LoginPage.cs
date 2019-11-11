using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Support.UI;


namespace YandexMarket.pages
{
    public class LoginPage
    {
        private IWebDriver driver;

        private const string DomikLoginLocator = "#login";
        private const string DomikPasswdLocator = "#passwd";
        private const string DomikButtonLocator = "button.nb-button";
        private const string LoginFieldLocator = "passp-field-login";
        private const string LoginButtonLocator = "//button[contains(@class, 'button2_type_submit')]";
        private const string PasswordFieldLocator = "//*[@id=\"passp-field-passwd\"]";
        private const string PasswButtonLocator = "//button[contains(@class, 'passp-form-button')]";

        [FindsBy(How = How.CssSelector, Using = DomikLoginLocator)]
        private IWebElement DomikLogin { get; set; }

        [FindsBy(How = How.CssSelector, Using = DomikPasswdLocator)]
        private IWebElement DomikPasswd { get; set; }

        [FindsBy(How = How.CssSelector, Using = DomikButtonLocator)]
        private IWebElement DomikButton { get; set; }

        [FindsBy(How = How.Id, Using = LoginFieldLocator)]
        public IWebElement LoginField { get; set; }

        [FindsBy(How = How.XPath, Using = LoginButtonLocator)]
        public IWebElement LoginButton { get; set; }


        [FindsBy(How = How.XPath, Using = PasswordFieldLocator)]
        public IWebElement PasswordField { get; set; }


        [FindsBy(How = How.XPath, Using = PasswButtonLocator)]
        public IWebElement PasswButton { get; set; }

        public LoginPage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
            this.driver = driver;
        }

        /*
         *logins if "domik" login form appears
         * (it can happen if connection is really bad) 
         */
        private void Domik(string email, string password)
        {

            DomikLogin.SendKeys(email);
            DomikPasswd.SendKeys(password);
            DomikButton.Submit();
        }

        /*
         *logins
         * if everythings OK, returns true 
         */
        public bool LogIn(string email, string password)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.PollingInterval = TimeSpan.FromSeconds(2);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            Func<IWebDriver, bool> waitForLogin = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                LoginField.GetHashCode();
                return true;
            }
            );

            Func<IWebDriver, bool> waitForPasswd = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                PasswordField.GetHashCode();
                return true;
            }
            );

            try
            {
                wait.Until(waitForLogin); LoginField.SendKeys(email);
                LoginButton.Click();

                wait.Until(waitForPasswd);
                PasswordField.SendKeys(password);
                PasswButton.Submit();
            }
            catch (WebDriverTimeoutException)
            {
                Domik(email, password);
            }
            return true;
        }

    }
}
