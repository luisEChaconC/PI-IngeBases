using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace UIAutomationTests
{
    public class EditCompanyTests
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
        public void EditCompany_AsEmployer_FullFlow()
        {
            // Login
            _driver.Navigate().GoToUrl($"{BaseUrl}/login");
            _driver.Manage().Window.Maximize();
            _wait.Until(d => d.FindElement(By.Id("email"))).SendKeys("estebancc@empresapi.com");
            _driver.FindElement(By.Id("password")).SendKeys("pruebaPI3#");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            _wait.Until(d => d.Url.Contains("/home-view"));

            // Ir directo a la vista de información de empresa
            _driver.Navigate().GoToUrl($"{BaseUrl}/view-company-info");
            _wait.Until(d => d.FindElement(By.XPath("//h4[contains(text(),'Información empresa')]")));

            // Click en "Editar"
            var btnEditar = _wait.Until(d =>
                d.FindElements(By.CssSelector("button.btn.btn-dark"))
                .FirstOrDefault(b => b.Text.Contains("Editar")));
            Assert.IsNotNull(btnEditar, "No se encontró el botón de editar.");
            btnEditar.Click();

            // Esperar que los campos estén habilitados
            _wait.Until(d => d.FindElement(By.Id("name")).Enabled);

            _driver.FindElement(By.Id("name")).Clear();
            _driver.FindElement(By.Id("name")).SendKeys("CompanyModified10");

            _driver.FindElement(By.Id("legalId")).Clear();
            _driver.FindElement(By.Id("legalId")).SendKeys("5-643-123410");

            new SelectElement(_driver.FindElement(By.Id("paymentType")))
                .SelectByText("Mensual");

            _driver.FindElement(By.Id("maxBenefits")).Clear();
            _driver.FindElement(By.Id("maxBenefits")).SendKeys("10");

            // Dirección
            _wait.Until(d => d.FindElements(By.CssSelector("input[placeholder='Provincia']")).Any());
            var provinciaInput = _driver.FindElements(By.CssSelector("input[placeholder='Provincia']")).FirstOrDefault();
            Assert.IsNotNull(provinciaInput, "No se encontró el input de Provincia.");
            provinciaInput.Clear();
            provinciaInput.SendKeys("Alajuela10");

            _wait.Until(d => d.FindElements(By.CssSelector("input[placeholder='Cantón']")).Any());
            var cantonInput = _driver.FindElements(By.CssSelector("input[placeholder='Cantón']")).FirstOrDefault();
            Assert.IsNotNull(cantonInput, "No se encontró el input de Cantón.");
            cantonInput.Clear();
            cantonInput.SendKeys("Alajuela10");

            _wait.Until(d => d.FindElements(By.CssSelector("input[placeholder='Barrio']")).Any());
            var barrioInput = _driver.FindElements(By.CssSelector("input[placeholder='Barrio']")).FirstOrDefault();
            Assert.IsNotNull(barrioInput, "No se encontró el input de Barrio.");
            barrioInput.Clear();
            barrioInput.SendKeys("Carrizal10");

            _wait.Until(d => d.FindElements(By.CssSelector("input[placeholder='Detalles adicionales']")).Any());
            var detallesInput = _driver.FindElements(By.CssSelector("input[placeholder='Detalles adicionales']")).FirstOrDefault();
            Assert.IsNotNull(detallesInput, "No se encontró el input de Detalles adicionales.");
            detallesInput.Clear();
            detallesInput.SendKeys("Cerca del kiosko10");

            _driver.FindElement(By.Id("phoneNumber")).Clear();
            _driver.FindElement(By.Id("phoneNumber")).SendKeys("8777-5510");

            _driver.FindElement(By.Id("email")).Clear();
            _driver.FindElement(By.Id("email")).SendKeys("company@modifiedten.com");

            var btnGuardar = _driver.FindElements(By.CssSelector("button.btn.btn-dark"))
                .FirstOrDefault(b => b.Text.Contains("Guardar cambios"));
            Assert.IsNotNull(btnGuardar, "No se encontró el botón de 'Guardar cambios'.");
            btnGuardar.Click();


            Assert.AreEqual("CompanyModified10", _driver.FindElement(By.Id("name")).GetAttribute("value"));
            Assert.AreEqual("5643123410", _driver.FindElement(By.Id("legalId")).GetAttribute("value"));
            Assert.AreEqual("Mensual", new SelectElement(_driver.FindElement(By.Id("paymentType"))).SelectedOption.Text);
            Assert.AreEqual("10", _driver.FindElement(By.Id("maxBenefits")).GetAttribute("value"));
            var fullAddressInput = _driver.FindElements(By.CssSelector("input.form-control[disabled]"))
    .FirstOrDefault(i => i.GetAttribute("value") != null && i.GetAttribute("value").Contains("Alajuela10"));
            Assert.IsNotNull(fullAddressInput, "No se encontró el input de dirección completa para validación.");
            Assert.AreEqual("Alajuela10, Alajuela10, Carrizal10, Cerca del kiosko10", fullAddressInput.GetAttribute("value"));

            Assert.AreEqual("87775510", _driver.FindElement(By.Id("phoneNumber")).GetAttribute("value"));
            Assert.AreEqual("company@modifiedten.com", _driver.FindElement(By.Id("email")).GetAttribute("value"));

        }


        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}