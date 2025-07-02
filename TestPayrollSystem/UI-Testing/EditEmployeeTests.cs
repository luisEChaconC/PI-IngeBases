using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace UIAutomationTests
{
    public class SeleniumTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private const string BaseUrl = "http://localhost:8080";

        [SetUp]
        public void Setup()
        {
            var options = new EdgeOptions
            {
                AcceptInsecureCertificates = true
            };
            _driver = new EdgeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void EditEmployee_AsEmployer_FullFlow()
        {
            // — login: IMPORTANT: Here you´ll need to change for real credentials in you DB, in my case, employer credentials —
            _driver.Navigate().GoToUrl($"{BaseUrl}/login");
            _driver.Manage().Window.Maximize();
            _wait.Until(d => d.FindElement(By.Id("email"))).SendKeys("nathalia@prueba.com");
            _driver.FindElement(By.Id("password")).SendKeys("Nathalia10#");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            _wait.Until(d => d.Url.Contains("/home-view"));

            // — employee list
            _driver.Navigate().GoToUrl($"{BaseUrl}/employees-list");
            _wait.Until(d => d.FindElement(By.XPath("//h2[contains(text(),'Empleados')]")));
            _wait.Until(d => d.FindElements(By.CssSelector("table tbody tr")).Any());

            // — open profile
            _wait.Until(d =>
                d.FindElement(By.CssSelector("table tbody tr:first-child a.btn-outline-dark.btn-sm"))).Click();
            _wait.Until(d => d.FindElement(By.XPath("//h2[text()='Perfil de Empleado']")));

            // — edit mode on
            _wait.Until(d =>
                d.FindElement(By.XPath("//button[contains(normalize-space(.),'Editar')]"))).Click();
            _wait.Until(d => d.FindElement(By.Id("firstName")).Enabled);

            // — modifier
            _driver.FindElement(By.Id("firstName")).Clear();
            _driver.FindElement(By.Id("firstName")).SendKeys("Juanmodificado");

            _driver.FindElement(By.Id("firstSurname")).Clear();
            _driver.FindElement(By.Id("firstSurname")).SendKeys("Varela");

            _driver.FindElement(By.Id("secondSurname")).Clear();
            _driver.FindElement(By.Id("secondSurname")).SendKeys("Sanabria");

            _driver.FindElement(By.Id("grossSalary")).Clear();
            _driver.FindElement(By.Id("grossSalary")).SendKeys("7856467");

            new SelectElement(_driver.FindElement(By.Id("contractType")))
                .SelectByText("Jornada Completa");
            new SelectElement(_driver.FindElement(By.Id("gender")))
                .SelectByValue("M");

           
            var btnGuardar = _driver.FindElement(By.CssSelector("button.btn.btn-primary"));
            btnGuardar.Click();
            Thread.Sleep(1000); 

            // — ASSERTS —
            Assert.AreEqual("Juanmodificado",
                _driver.FindElement(By.Id("firstName")).GetAttribute("value"));

            Assert.AreEqual("Varela",
                _driver.FindElement(By.Id("firstSurname")).GetAttribute("value"));

            Assert.AreEqual("Sanabria",
                _driver.FindElement(By.Id("secondSurname")).GetAttribute("value"));

            Assert.AreEqual("7856467",
                _driver.FindElement(By.Id("grossSalary")).GetAttribute("value"));

            Assert.AreEqual("Jornada Completa",
                new SelectElement(_driver.FindElement(By.Id("contractType"))).SelectedOption.Text);

            Assert.AreEqual("M",
                new SelectElement(_driver.FindElement(By.Id("gender"))).SelectedOption.GetAttribute("value"));
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
