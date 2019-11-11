using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexMarket.browser
{
    public class Browser
    {
        private static Browser browser;
        private static IWebDriver driver;

        public static IWebDriver Driver
        {
            get
            {
                return driver;
            }
            private set
            {
                driver = value;
            }
        }

        private Browser(string Name, List<string> Settings)
        {
            BrowserFactory.InitElements(Name, Settings);
            driver = BrowserFactory.Driver;
        }

        public static Browser GetInstance(string Name, List<string> Settings)
        {
            if(browser == null)
            {
                browser = new Browser(Name, Settings);
            }
            return browser;
        }
    }
}
