using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Linq;
using System.Threading;

namespace UIAutomationTests
{
    public class BenefitTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private const string BaseUrl = "http://localhost:8080";

        [SetUp]
        public void Setup()
        {
            var options = new EdgeOptions { AcceptInsecureCertificates = true };
            _driver = new EdgeDriver(options);
            _wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
            _driver.Manage().Window.Maximize();
        }

        [Test]
        public void AddBenefit_AsEmployer_FullFlow()
        {
            _driver.Navigate().GoToUrl($"{BaseUrl}/login");
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("email"))).SendKeys("nathalia@prueba.com");
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("password"))).SendKeys("Nathalia10#");
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[type='submit']"))).Click();
            _wait.Until(d => d.Url.Contains("/home-view"));
            Thread.Sleep(100);

            _driver.Navigate().GoToUrl($"{BaseUrl}/benefits");
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[contains(text(),'Beneficios')]")));
            Thread.Sleep(1000);

            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.btn.btn-dark"))).Click();
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h3[text()='Nuevo Beneficio']")));
            Thread.Sleep(300);

            var typeLocator = By.XPath("//label[normalize-space()='Tipo']/following-sibling::select");
            var typeSelectElem = _wait.Until(ExpectedConditions.ElementToBeClickable(typeLocator));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", typeSelectElem);
            new SelectElement(typeSelectElem).SelectByText("Monto Fijo");
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//label[normalize-space()='Seleccionar API']")));
            Thread.Sleep(2000);

            var nameLocator = By.XPath("//label[normalize-space()='Nombre']/following-sibling::input");
            var nameInput = _wait.Until(ExpectedConditions.ElementToBeClickable(nameLocator));
            nameInput.Clear();
            nameInput.SendKeys("RIGOR");
            Thread.Sleep(500);

            var descLocator = By.XPath("//label[normalize-space()='Descripción']/following-sibling::textarea");
            var descTextarea = _wait.Until(ExpectedConditions.ElementToBeClickable(descLocator));
            descTextarea.Clear();
            descTextarea.SendKeys("Descripción automatizada de prueba");
            Thread.Sleep(500);

            var amountLocator = By.XPath("//label[normalize-space()='Monto fijo']/following-sibling::div//input");
            var amountInput = _wait.Until(ExpectedConditions.ElementToBeClickable(amountLocator));
            amountInput.Clear();
            amountInput.SendKeys("7000");
            Thread.Sleep(500);

            var monthsLocator = By.XPath("//label[normalize-space()='Meses requeridos trabajados']/following-sibling::input");
            var monthsInput = _wait.Until(ExpectedConditions.ElementToBeClickable(monthsLocator));
            monthsInput.Clear();
            monthsInput.SendKeys("0");
            Thread.Sleep(500);

            var checkboxes = _driver.FindElements(By.CssSelector("input.form-check-input")).Take(2).ToList();
            foreach (var cb in checkboxes)
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(cb));
                if (!cb.Selected) cb.Click();
                Thread.Sleep(500);
            }

            var saveBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btn.btn-success")));
            saveBtn.Click();
            var alert = _wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.AreEqual("Beneficio guardado exitosamente", alert.Text);
            alert.Accept();
            Thread.Sleep(2000);

            _wait.Until(d => d.Url.Contains("/benefits"));
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[contains(text(),'Beneficios')]")));
            Thread.Sleep(2000);

            var row = _wait.Until(d =>
                d.FindElements(By.CssSelector("table tbody tr"))
                 .FirstOrDefault(r => r.Text.Contains("BeneficioTest"))
            );
            Assert.IsNotNull(row);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}