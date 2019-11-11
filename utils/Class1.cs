using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using YandexMarket.browser;
using YandexMarket.utils;
using YandexMarket.pages;


namespace YandexMarket.utils
{
    class Class1
    {
        public static FileUtils file = new FileUtils("\\config.txt");
        public static List<string> options = file.GetOptions();
        public Browser browser = Browser.GetInstance(file.GetBrowser(), options);
        public string YMUrl = file.GetUrl();
        public string email = file.GetEmail();
        public string password = file.GetPassword();
        public IWebDriver driver = Browser.Driver;
        public static void Main(string[] args)
        {

        }

    }

}


