using System;
using System.Configuration;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationPOC
{
    
    [TestFixture]
    public class Login : LoginUtility
    {
        //string projBaseDir = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;

        [Test, Order(1)]
        public void ValidLogin()
        {
            WebDriver_WaitFunction();
            ClickOnHomePageLink();
            EnterUserName();
            ClickOnNext();
            EnterPassword();
            ClickOnNext();
            ClickOn_No_forStaySignedIn();
            LogOutButton_Validation();
            ClickOnLogOutButton();
            ValidateLogOutElementNotFound();


        }
    [Test, Order(2)]
        public void InvalidUserNameLogin()
        {
            //WebDriver_WaitFunction();
            ClickOnHomePageLink();
            EnterInvalidUserName();
            ClickOnNext();
            InvalidUserName_Validation();
                           

        }
    [Test, Order(3)]
        public void InvalidPasswordLogin()
        {
            WebDriver_WaitFunction();
            ClickOnHomePageLink();
            EnterUserName();
            ClickOnNext();
            EnterInvalidPassword();
            ClickOnNext();
            InvalidPassword_Validation();
        }
    
        public void LogOut_Validation()
        {
            WebDriver_WaitFunction();
            ClickOnHomePageLink();
            EnterUserName();
            ClickOnNext();
            EnterPassword();
            ClickOnNext();
            ClickOn_No_forStaySignedIn();
            ClickOnLogOutButton();
            LogOutValidation();
            
        }
    //[Test]
    //public void ContactDetails()
    //    {
    //        ClickandValidate_Contact_HomePage();
    //    }

    }
}
