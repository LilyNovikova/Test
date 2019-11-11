using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace YandexMarket.pages
{
    public class HomePage
    {
        private IWebDriver driver;

        private const string LoginButtonNoJSLocator = "//a[contains(@class,'user__login')]";
        private const string LoginButtonLocator = "//a[contains(@class,'button2_js_inited')]";
        private const string UserIconLocator = "//div[@class='header2-nav__user']//span [@class='user__icon user__icon_loaded_yes']";
        private const string LogoutButtonLocator = "//a[contains(@class,'link user user__logout')]";

       [FindsBy(How = How.XPath, Using = LoginButtonNoJSLocator)]
        private IWebElement LoginButtonNoJS { get; set; }

        [FindsBy(How = How.XPath, Using = LoginButtonLocator)]
        public IWebElement LoginButton { get; set; }


        [FindsBy(How = How.XPath, Using = UserIconLocator)]
        public IWebElement UserIcon { get; set; }

        [FindsBy(How = How.XPath, Using = LogoutButtonLocator)]
        public IWebElement LogoutButton { get; set; }


        public HomePage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
            this.driver = driver;
        }

        /**
         *hovers to nonclickable login button 
         * (enables click the real login button) 
         */
        private void HoverToLoginButton()
        {
            Actions action = new Actions(driver);
            action.MoveToElement(LoginButtonNoJS);
            action.Perform();
        }


        /**
         *clicks login button 
         */
        public void ClickLoginButton()
        {
            HoverToLoginButton();
            LoginButton.Click();
        }

        /**
         * logs out and checks if log out was successful by finding login button
         */
        public bool LogOut()
        {
            (new WebDriverWait(driver, new TimeSpan(0, 0, 20))).Until(ExpectedConditions.ElementToBeClickable(UserIcon));
            UserIcon.Click();
            (new WebDriverWait(driver, new TimeSpan(0, 0, 20))).Until(ExpectedConditions.ElementToBeClickable(LogoutButton));
            LogoutButton.Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                HoverToLoginButton();
                return true;
            });
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.PollingInterval = TimeSpan.FromSeconds(2);
            wait.Until(waitForElement);
            return true;
        }

    }
}
