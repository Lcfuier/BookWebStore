using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWebStore.Automation
{
    public class LoginTests
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5160"; // Adjust based on your app's URL
        private readonly WebDriverWait _wait;
        public LoginTests()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        [Fact]
        public void Login_WithInvalidCredentials_ShouldShowValidationMessage()
        {
            // Arrange
            _driver.Navigate().GoToUrl($"{_baseUrl}/identity/account/login/");


            // Fill in login form
            _driver.FindElement(By.Id("Input_Email")).SendKeys("duyduydz1@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Duyvip@13");
            _driver.FindElement(By.Id("login-submit")).Click();
            // Assert
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.ClassName("text-danger")).Displayed);

            // Capture validation message
            var validationMessage = _driver.FindElement(By.ClassName("text-danger")).Text;

            // Assert that a validation message is shown
            Assert.Contains("Invalid login attempt", validationMessage); // Replace with the actual error message text expected

            // Alternatively, check the validation summary
            var validationSummary = _driver.FindElement(By.CssSelector("div[role='alert']")).Text;
            Assert.Contains("Invalid login attempt", validationSummary); // Adjust the expected text accordingly
        }
        [Fact]
        public void Login_WithValidCredentials_ShouldShowNoValidationMessage()
        {
            // Locate input fields and submit button
            // Arrange
            _driver.Navigate().GoToUrl($"{_baseUrl}/identity/account/login/");


            // Fill in login form
            _driver.FindElement(By.Id("Input_Email")).SendKeys("duyduydz13+1@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Duyvip@13");
            //submit
            _driver.FindElement(By.Id("login-submit")).Click();

            // Wait for the page to redirect (we assume the redirect is to the home page)
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/") || d.Url.Contains("Home/Index"));

            // Assert that the URL is the home page or base URL after a successful login
            Assert.True(_driver.Url.Contains("/") || _driver.Url.Contains("Home/Index"),
                "User should be redirected to the home page or the base URL after successful login.");

            // Optionally, you can verify page elements on the home page to ensure it's the correct page
            var title = _driver.FindElement(By.TagName("h1")).Text;
            Assert.Equal("Tất cả sách", title); // Assuming the title on the home page is "Tất cả sách"
        }
    }
}
