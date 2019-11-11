using System;
using NUnit.Framework;
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

namespace Market
{
    [TestFixture]
    class TestScript
    {
        public IWebDriver driver;
        public Browser browser;
        public FileUtils file = new FileUtils("config.csv");
        public string ExpectedFileName = "expected.csv";
        public string OutputFileName;

        public string YMUrl;
        public string email;
        public string password;

        public HomePage HomePage;
        public LoginPage LoginPage;
        public CategoriesPage CategoriesPage;

        List<string> popular;
        List<string> all;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            List<string> options = file.GetOptions();
            YMUrl = file.GetUrl();
            browser = Browser.GetInstance(file.GetBrowser(), options);
            driver = Browser.Driver;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            email = file.GetEmail();
            password = file.GetPassword();
            OutputFileName = file.GetOutputFile();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
        }


        [Test]
        public void Test0OpenHomePage()
        {
            driver.Navigate().GoToUrl(YMUrl);
            Assert.AreEqual(YMUrl, driver.Url, "Opened not homepage URL");
        }

        [Test]
        public void Test1LogIn()
        {
            HomePage = new HomePage(driver);
            HomePage.ClickLoginButton();

            LoginPage = new LoginPage(driver);
            driver.SwitchTo().Window(driver.WindowHandles[driver.WindowHandles.Count - 1]);
            bool IsLogin = LoginPage.LogIn(email, password);
            Assert.IsTrue(IsLogin, "Log in failed");

            driver.SwitchTo().Window(driver.WindowHandles[0]);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                return HomePage.UserIcon.Displayed;
            });
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.PollingInterval = TimeSpan.FromSeconds(2);
            wait.Until(waitForElement);

            Assert.IsTrue(HomePage.UserIcon.Displayed, "UserIcon is not displayed");
        }

        [Test]
        public void Test2GoToRandomCategory()
        {
            driver.Manage().Window.Maximize();
            CategoriesPage cat = new CategoriesPage(driver);
            string ExpectedTitle = cat.GoToRandomCategory();
            string title = cat.CategoryTitle.Text;

            cat.MarketLogo.Click();
            if (ExpectedTitle.Equals("Авто"))
            {
                Assert.IsTrue(title.ToLower().Contains(ExpectedTitle.ToLower()), "Wrong title of the page");
            }
            else
            {
                Assert.AreEqual(ExpectedTitle, title, "Wrong title of the page");
            }
        }

        [Test]
        public void Test3AllCategoriesFile()
        {
            CategoriesPage = new CategoriesPage(driver);
            popular = CategoriesPage.GetPopular();
            all = CategoriesPage.GetAllCategories(file);
            Assert.IsTrue(file.CompareFiles(file.GetPath() + file.GetPathSeparator() + ExpectedFileName,
                OutputFileName), "Output file is incorrect");
        }

        [Test]
        public void Test4PopularInAllCategories()
        {
            Assert.IsTrue(ListUtils<string>.Contains(all, popular), "Not all popular categories are in list of all categories");
        }

        [Test]
        public void Test5LogOut()
        {
            Assert.IsTrue(HomePage.LogOut(), "Logout failed");
        }
    }
}
