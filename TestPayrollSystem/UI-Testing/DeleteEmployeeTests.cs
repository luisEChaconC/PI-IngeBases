using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Threading;

namespace UIAutomationTests
{
    public class DeleteEmployeeTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private const string BaseUrl = "http://localhost:8080";

        [SetUp]
        public void Setup()
        {
            var options = new EdgeOptions { AcceptInsecureCertificates = true };
            _driver = new EdgeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void DeleteEmployee_AsEmployer_FullFlow()
        {
            _driver.Navigate().GoToUrl($"{BaseUrl}/login");
            _driver.Manage().Window.Maximize();
            _wait.Until(d => d.FindElement(By.Id("email"))).SendKeys("nathalia@prueba.com");
            _driver.FindElement(By.Id("password")).SendKeys("Nathalia10#");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            _wait.Until(d => d.Url.Contains("/home-view"));

            _driver.Navigate().GoToUrl($"{BaseUrl}/employees-list");
            _wait.Until(d => d.FindElement(By.XPath("//h2[contains(text(),'Empleados')]")));

            var viewBtn = _wait.Until(d => d.FindElement(By.CssSelector("table tbody tr:first-child a.btn-outline-dark.btn-sm")));
            viewBtn.Click();

            _wait.Until(d => d.FindElement(By.XPath("//h2[text()='Perfil de Empleado']")));
            _wait.Until(d => !d.FindElements(By.CssSelector(".spinner-border")).Any());

            Thread.Sleep(2000); 

            var deleteBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='Eliminar']")));
            deleteBtn.Click();

            var confirmBtn = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".swal2-confirm")));
            Thread.Sleep(1500);
            confirmBtn.Click();

            _wait.Until(d => d.Url.Contains("/employees-list"));

            Thread.Sleep(2000); 
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}