using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexMarket.browser
{
    public class BrowserFactory
    {
        private static readonly IDictionary<string, IWebDriver> Drivers = new Dictionary<string, IWebDriver>();
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

        public static void InitElements(string BrowserName, List<string> Settings)
        {
            switch (BrowserName)
            {
                case "Firefox":
                    if (Driver == null)
                    {
                        FirefoxOptions options = new FirefoxOptions();
                        options.AddArguments(Settings);
                        driver = new FirefoxDriver(options);
                        Drivers.Add("Firefox", Driver);
                    }
                    break;
                case "Chrome":
                    if (Driver == null)
                    {
                        ChromeOptions options = new ChromeOptions();
                        options.AddArguments(Settings);
                        driver = new ChromeDriver(options);
                        Drivers.Add("Chrome", Driver);
                    }
                    break;
            }
        }

    }
}

