using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;

namespace SQ_ASSIGN_02
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver driver;

        [TestInitialize]                       // This line initialize the test
        public void testInt()
        {
            driver = new ChromeDriver();
        }

        [TestCleanup]                         //This line cleans the test and ends the driver

        public void testCleanUp()
        {
            if(driver!=null)
            {
                driver.Quit();
            }
                 
        }


        [TestMethod]
        public void T1_login_enterCredentials_logedIn()
        {
            //Entering correct credentials to login,
            //expected result = login
            //actual result = login
            //the test passed

            driver.Navigate().GoToUrl("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/login.html");
            IWebElement username = driver.FindElement(By.Id("username"));

            username.SendKeys("admin");

            IWebElement password = driver.FindElement(By.Id("password"));

            password.SendKeys("password");
            password.Submit();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Title.Contains("Vehicle Selling Store"));

        }
        [TestMethod]
        public void T2_login_enterUsernameOnly_errorMsg()
        {
            //Bug! loging in with username only, without password
            //expected result = not log in
            //actual result = logged in
            // Test fails

            driver.Navigate().GoToUrl("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/login.html");
            IWebElement username = driver.FindElement(By.Name("username"));
            username.SendKeys("sri");
            
            IWebElement login = driver.FindElement(By.Name("formSubmit"));
            login.Click();
           
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver=>driver.FindElement(By.Id("errorMessage")));
        }

        [TestMethod]
        public void T3_logout_whileInHomePage_loggedout()
        {
            //Cannot logout from the homepage
            //expected result = log out
            //actual result = not logged out
            //Test fails

            T1_login_enterCredentials_logedIn();
            IWebElement logout = driver.FindElement(By.Id("goLogout"));
            logout.Click();
            Assert.IsTrue(driver.Navigate().Equals("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/login.html"));
        
        }
        [TestMethod]
        public void T4_adminPage_ClickOn_VehicleSalePage()
        {
            //Bug! cannot move to vehicle for sale page while in admin page
            //expected result = moved to vehicle fo sales page
            //actual result = stayed in admin page
            // Test fails
            driver.Navigate().GoToUrl("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/admin.html");
            IWebElement VehicleSalePG = driver.FindElement(By.Id("goSale"));
            VehicleSalePG.Click();
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.Navigate().Equals("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/sale.html"));

        }

        [TestMethod]
        public void T5__PostNewVehicle_2letterName_errormsg()
        {
            //Checking error message shown when the seller's name is less or equal to 2 characters
            //Expected result = Show error message
            //actual result = Show error message
            // Test Pass
            T1_login_enterCredentials_logedIn();
            driver.Navigate().GoToUrl("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/new.html");
            driver.FindElement(By.Id("sellerName")).SendKeys("SH");
            driver.FindElement(By.Id("formSubmit")).Click();

            IWebElement errmsg = driver.FindElement(By.Id("errorMessage"));
            Assert.IsTrue(errmsg.Text.Contains("Seller Name length must be greater than 2"));
        }
        [TestMethod]
        public void T6_PostNewVehicle_sellerInfo_Infosubmitted()
        {
            //Seller info is filled and should submitted without any error in the formats
            //Expected result = Submitted Succcessfully
            //actual result = Submitted Succcessfully
            // Test Pass
            T1_login_enterCredentials_logedIn();
            driver.Navigate().GoToUrl("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/new.html");
            driver.FindElement(By.Id("sellerName")).SendKeys("Sri");
            driver.FindElement(By.Id("address")).SendKeys("Richy Street");
            driver.FindElement(By.Id("city")).SendKeys("Waterloo");
            driver.FindElement(By.Id("phone")).SendKeys("555-123-4567");
            driver.FindElement(By.Id("email")).SendKeys("sganeshbabu0808@def.kj");
            driver.FindElement(By.Id("make")).SendKeys("Sell");
            driver.FindElement(By.Id("model")).SendKeys("Civic");
            driver.FindElement(By.Id("year")).SendKeys("2006");

            driver.FindElement(By.Id("formSubmit")).Click();
            IWebElement Header = driver.FindElement(By.Id("header"));
            Assert.IsTrue(Header.Text.Contains("Posted Successfully!"));
        }


        [TestMethod]
        public void T7_AdminPage_EnterEmail_VehicleInfoCleared()
        {
            //To check entered email's info is deleted
            //Expected result = Details should be deleted
            //actual result = Details should be deleted
            // Test Pass
            T6_PostNewVehicle_sellerInfo_Infosubmitted();
            driver.Navigate().GoToUrl("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/admin.html");
            driver.FindElement(By.Id("itemEmail")).SendKeys("sganeshbabu0808@def.kj");
            driver.FindElement(By.Id("clearOne")).Click();
            //driver.FindElement(By.XPath("/html/body/div/div/section[1]")).Equals("$0");

            driver.Navigate().GoToUrl("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/sale.html");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div/div/section[1]")));
        }
       
        [TestMethod]
        public void T8_VehicleSalePage_ClickOn_PostNewVehicle()
        {
            //Bug! cannot move to post new vehicle page while in vehicle sale page
            //expected result = moved to post new vehicle page
            //actual result = stayed in vehicle sale page
            // Test fails
            driver.Navigate().GoToUrl("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/sale.html");
            IWebElement PostNewVehicle = driver.FindElement(By.Id("goSale"));
            PostNewVehicle.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.Navigate().Equals("file:///C:/Users/Sri%20Hari/OneDrive/Documents/SEM-3/Software%20Quality%20-%20ALLISON/VehicleStore/new.html"));

        }
    }
}

//[TestMethod]
//public void GoogleSearch_WhenGivenKeyword_ReturnPageWithKeywordInTitle()
//{
//    driver.Navigate().GoToUrl("https://google.com/");
//    IWebElement queryTextbox = driver.FindElement(By.Name("q"));

//    queryTextbox.SendKeys("conestoga college");
//    queryTextbox.Submit();

//    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
//    wait.Until(d => d.Title.ToLower().Contains("conestoga college"));
//}

//[TestMethod]
//public void DuckDuckGo_SearchHelloWorld_FirstResultIsMovie()
//{
//    driver.Navigate().GoToUrl("https://duckduckgo.com/");
//    IWebElement queryTextbox = driver.FindElement(By.Id("search_form_input_homepage"));

//    queryTextbox.Clear();
//    queryTextbox.SendKeys("Hello World");
//    queryTextbox.Submit();

//    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
//    wait.Until(d => d.FindElement(By.XPath("//*[@id=\"r1-0\"]/div/h2/a[1]")).GetAttribute("href") == "https://www.imdb.com/title/tt9418812/");

//}
//    }
//}

