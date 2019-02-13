using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AutomationPOC
{
    public class LoginUtility
    {
        
        string executionStartTime = DateTime.Now.ToString("ddmmyyyyhhmmsstt");

        public static IWebDriver driver = null;
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["URL"]);
            Thread.Sleep(5000);
        }
       
        public static void WebDriver_WaitFunction()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(2)); 
        }

        public static void ClickOnHomePageLink()
        {
            driver.FindElement(By.XPath("/html/body/div[2]/a")).Click();
            Thread.Sleep(5000);
        }
        public static void EnterUserName()
        {
            driver.FindElement(By.XPath("//*[@id='i0116']")).SendKeys(ConfigurationManager.AppSettings["UserName"]);
        }
        public static void ClickOnNext()
        {
            driver.FindElement(By.XPath("//*[@id='idSIButton9']")).Click();
            Thread.Sleep(5000);

        }
        public static void EnterPassword()
        {
            driver.FindElement(By.XPath("//*[@id='i0118']")).SendKeys(ConfigurationManager.AppSettings["Password"]);
        }
        public static void ClickOn_No_forStaySignedIn()
        {
            driver.FindElement(By.XPath("//*[@id='idBtn_Back']")).Click();
            Thread.Sleep(5000);
            
        }
        public static void LogOutButton_Validation()
        {
            Boolean logOUtButton = driver.FindElement(By.XPath("/html/body/div[2]/h5/a")).Displayed;
            Assert.True(logOUtButton);
            //Assert.That(logOUtButton, Does.Match("Sign out"));
        }
        public static void EnterInvalidUserName()
        {
            driver.FindElement(By.XPath("//*[@id='i0116']")).SendKeys(ConfigurationManager.AppSettings["InvalidUserName"]);
        }
        public static void EnterInvalidPassword()
        {
            driver.FindElement(By.XPath("//*[@id='i0118']")).SendKeys(ConfigurationManager.AppSettings["InvalidPassword"]);
        }

        public static void InvalidUserName_Validation()
        {
            String invalidUsername_text = driver.FindElement(By.XPath("//*[@id='usernameError']")).Text;
            Assert.That(invalidUsername_text, Does.Match("We couldn't find an account with that username."));
        }
        public static void InvalidPassword_Validation()
        {
            String invalidPassword_text = driver.FindElement(By.XPath("//*[@id='passwordError']")).Text;
            Assert.That(invalidPassword_text, Does.Match("Your account or password is incorrect. If you don't remember your password, "));

        }
        public static void ClickOnLogOutButton()
        {
            driver.FindElement(By.XPath("/html/body/div[2]/h5[1]/a")).Click();
            Thread.Sleep(20000);

        }


        public static void ValidateLogOutElementNotFound()
        {
            Boolean logOUtButton= false;
            try
            {
              
                logOUtButton = driver.FindElement(By.XPath("/html/body/div[2]/h5/a")).Displayed;
            }
            catch
            {

            }
            
           Assert.False(logOUtButton);

        }

        public static void LogOutValidation()
        {
            String logout_text = driver.FindElement(By.XPath("//*[@id='login_workload_logo_text']")).Text;
            Assert.That(logout_text, Does.Match("You signed out of your account"));
        }

        public static void About_HomePage()
        {
            driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/ul/li[1]/a")).Click();
            
        }

        public static void ClickandValidate_Contact_HomePage()
        {
            driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/ul/li[2]/a")).Click();
            String invalidPassword_text = driver.FindElement(By.XPath("//*[@id='login_workload_logo_text']")).Text;
            Assert.That(invalidPassword_text, Does.Match("You signed out of your account"));

        }

        public void WriteToLog(String logText, string executionStartTime)
        {

            try
            {
                
                //string startupPath = System.IO.Directory.GetCurrentDirectory();
                string executionLogFolder = @"C:\Users\singarapud\Desktop\Sample Project\AutoamtionTestPOC\AutomationPOC" + "//ExecutionLogs";

                if (!Directory.Exists(executionLogFolder))
                    Directory.CreateDirectory(executionLogFolder);

                string executionLogFileName = "ExecutionLog" + executionStartTime + ".txt";

                StreamWriter log;

                string logFilePath = executionLogFolder + "\\" + executionLogFileName;

                if (!File.Exists(logFilePath))
                {
                    log = new StreamWriter(logFilePath, true);
                    log.WriteLine("#######################################");
                    log.WriteLine(DateTime.Now.ToString("F"));
                    log.WriteLine("#######################################");
                }
                else
                {

                    log = File.AppendText(logFilePath);
                }

                log.WriteLine(logText);
                log.WriteLine();
                log.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        [TearDown]
        public void CleanUp()
        {
            if(driver!= null)
                driver.Quit();
            //if (TestContext.CurrentContext.Result. == NUnit.Framework.Interfaces.TestStatus.Failed)
            //{
            //    testFail++;
            //}
            //else
            //{
            //    testPass++;
            //}
        }

        [OneTimeTearDown]
        public void finalCleanUp()
        {
            WriteToLog("Total Executed=" + (TestContext.CurrentContext.Result.PassCount + TestContext.CurrentContext.Result.FailCount),executionStartTime);
            WriteToLog("Total Passed=" + TestContext.CurrentContext.Result.PassCount, executionStartTime);
            WriteToLog("Total Failed=" + TestContext.CurrentContext.Result.FailCount, executionStartTime);
        }



        //public static void Browser_Quit()
        //{
        //    driver.Quit();
        //}


        //public static void WriteToLog(String logText, string executionStartTime)
        //{

        //try
        //    {

        //    string startupPath = System.IO.Directory.GetCurrentDirectory();
        //    string executionLogFolder = startupPath + "ExecutionLogs";

        //        if (!Directory.Exists(executionLogFolder))
        //            Directory.CreateDirectory(executionLogFolder);

        //        string executionLogFileName = "ExecutionLog" + executionStartTime + ".txt";

        //        StreamWriter log;

        //        string logFilePath = executionLogFolder + "\\" + executionLogFileName;

        //        if (!File.Exists(logFilePath))
        //        {
        //            log = new StreamWriter(logFilePath, true);
        //            log.WriteLine("#######################################");
        //            log.WriteLine(DateTime.Now.ToString("F"));
        //            log.WriteLine("#######################################");
        //        }
        //        else
        //        {

        //            log = File.AppendText(logFilePath);
        //        }

        //        log.WriteLine(logText);
        //        log.WriteLine();
        //        log.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //}

    }

}

