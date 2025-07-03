using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

namespace UIAutomationTests
{
    public class CreateEmployeeTests
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
        public void CreateEmployee_AsEmployer_FullFlow()
        {
            // 1) LOGIN
            _driver.Navigate().GoToUrl($"{BaseUrl}/login");
            _driver.Manage().Window.Maximize();
            _wait.Until(d => d.FindElement(By.Id("email"))).SendKeys("nathalia@empresapi.com");
            _driver.FindElement(By.Id("password")).SendKeys("Nathalia10#");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            _wait.Until(d => d.Url.Contains("/home-view"));

            // 2) LISTA DE EMPLEADOS
            _driver.Navigate().GoToUrl($"{BaseUrl}/employees-list");
            _wait.Until(d => d.FindElement(By.XPath("//h2[contains(text(),'Empleados')]")));

            // 3) NUEVO EMPLEADO
            _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[contains(normalize-space(.),'Nuevo Empleado')]")
            )).Click();
            _wait.Until(d => d.FindElement(By.XPath("//h2[contains(text(),'Nuevo empleado')]")));

            // 4) LLENAR FORMULARIO
            const string FIRST_NAME = "TestMarta";
            const string FIRST_SURNAME = "Perez";
            const string SECOND_SURNAME = "Gomez";
            const string LEGAL_ID = "7-0045-6189";
            const string WORKER_ID = "EMP-075";
            const string EMAIL = "test.marta@gmail.com";
            const string PHONE = "2494-0000";
            const string SALARY = "100000";

            _driver.FindElement(By.Id("firstName")).SendKeys(FIRST_NAME);
            _driver.FindElement(By.Id("firstSurname")).SendKeys(FIRST_SURNAME);
            _driver.FindElement(By.Id("secondSurname")).SendKeys(SECOND_SURNAME);
            _driver.FindElement(By.Id("legalId")).SendKeys(LEGAL_ID);
            _driver.FindElement(By.Id("workerId")).SendKeys(WORKER_ID);
            _driver.FindElement(By.Id("email")).SendKeys(EMAIL);
            _driver.FindElement(By.Id("phone")).SendKeys(PHONE);
            _driver.FindElement(By.Id("grossSalary")).SendKeys(SALARY);

            new SelectElement(_driver.FindElement(By.Id("contractType")))
                .SelectByText("Jornada Completa");
            new SelectElement(_driver.FindElement(By.Id("role")))
                .SelectByText("Colaborador");
            new SelectElement(_driver.FindElement(By.Id("gender")))
                .SelectByValue("M");

            // 5) GUARDAR
            var btnAgregar = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//button[contains(normalize-space(.),'Agregar')]")
            ));
            ((IJavaScriptExecutor)_driver)
                .ExecuteScript("arguments[0].scrollIntoView(true);", btnAgregar);
            ((IJavaScriptExecutor)_driver)
                .ExecuteScript("arguments[0].click();", btnAgregar);

            // 6) ALERT 
            _wait.Until(ExpectedConditions.AlertIsPresent()).Accept();
            _wait.Until(d => d.Url.Contains("/employees-list"));
            var found = _wait.Until(d =>
                d.FindElements(By.CssSelector("table tbody tr"))
                 .FirstOrDefault(r => r.Text.Contains(FIRST_NAME))
            );
            Assert.IsNotNull(found, "New employee not found");
            StringAssert.Contains(FIRST_SURNAME, found.Text);
            StringAssert.Contains(SECOND_SURNAME, found.Text);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}