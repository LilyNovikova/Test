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
using YandexMarket.utils;

namespace YandexMarket.pages
{
    public class CategoriesPage
    {
        private IWebDriver driver;
        private List<IWebElement> popular;
        private static String[] NotCategoryName =
        {
            "Скидки и акции",
            "Журнал Маркета"
        };


        private const string CategoryTitleLocator = ".main h1";
        private const string AllCategoriesButtonLocator = ".n-w-tab_type_navigation-menu-grouping span";
        private const string MarketLogoLocator = ".logo_part_market span";
        private const string RegionNotificationButtonLocator = "Да, спасибо";
        private const string PopularCategoriesLocator = "//span[@class='n-w-tab__control-caption']";
        private const string AllCategoriesLocator = "//div[contains(@class, 'n-w-tabs__vertical-tabs')]//span[@class='n-w-tab__control-caption']";


        [FindsBy(How = How.CssSelector, Using = CategoryTitleLocator)]
        public IWebElement CategoryTitle { get; set; }

        [FindsBy(How = How.CssSelector, Using = AllCategoriesButtonLocator)]
        public IWebElement AllCategoriesButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = MarketLogoLocator)]
        public IWebElement MarketLogo { get; set; }

        [FindsBy(How = How.LinkText, Using = RegionNotificationButtonLocator)]
        public IWebElement RegionNotificationButton { get; set; }


        public CategoriesPage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
            this.driver = driver;
        }

        /**
         *Closes region notification if it appears 
         */
        private void CloseRegionNotification()
        {
            try
            {
                (new WebDriverWait(driver, new TimeSpan(0, 0, 20)))
                .Until(ExpectedConditions.ElementToBeClickable(RegionNotificationButton));
                RegionNotificationButton.Click();
            }
            catch (WebDriverTimeoutException)
            {

            }
        }

        /**
         * returns a List<string> list of popular categories names
         */
        public List<string> GetPopular()
        {
            List<IWebElement> categories = driver.FindElements(By.XPath(PopularCategoriesLocator)).ToList();
            var PopularCategoryNames = new List<string>();
            popular = new List<IWebElement>();

            foreach (IWebElement element in categories)
            {
                string title = element.Text;
                if (element.Displayed && !NotCategoryName.Contains(element.Text))
                {
                    PopularCategoryNames.Add(element.Text);
                    popular.Add(element);
                }
            }
            return PopularCategoryNames;
        }

        /**
         * goes to a random category from the list of popular
         */
        public string GoToRandomCategory()
        {
            if (popular == null)
            {
                GetPopular();
            }
            IWebElement RandomElement = popular.ElementAt((new Random()).Next() % popular.Count);
            string title = RandomElement.GetAttribute("innerText");
 
            CloseRegionNotification();
            (new WebDriverWait(driver, new TimeSpan(0, 0, 20)))
                .Until(ExpectedConditions.ElementToBeClickable(RandomElement));
            Actions actions = new Actions(driver);
            actions.MoveToElement(RandomElement, 1, 1).Click().Build().Perform();
            //RandomElement.Click();
            return title;
        }


        /**
         * returns a List<string> list of all categories names
         */
        public List<string> GetAllCategories(FileUtils file)
        {
            (new WebDriverWait(driver, new TimeSpan(0, 0, 10)))
                .Until(ExpectedConditions.ElementToBeClickable(AllCategoriesButton));
            AllCategoriesButton.Click();
            (new WebDriverWait(driver, new TimeSpan(0, 0, 20)))
                .Until(ExpectedConditions.ElementToBeClickable(By.XPath(AllCategoriesLocator)));
            List<IWebElement> all = driver.FindElements(By.XPath(AllCategoriesLocator)).ToList();
            var AllNames = new List<string>();
            foreach (IWebElement element in all)
            {
                AllNames.Add(element.Text);
            }
            file.Write(ListUtils<string>.ToString(AllNames));

            return AllNames;
        }

    }
}
