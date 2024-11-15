using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace BookWebStore.Automation
{
    public class RegisterTests
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5160"; // Adjust based on your app's URL
        private readonly WebDriverWait _wait;
        public RegisterTests()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        [Fact]
        public void Register_WhenUserAlreadyExist_ShouldShowErrorMessage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/identity/account/register/");
            _driver.Manage().Window.Maximize();
            // Locate input fields and submit button
            var firstNameField = _driver.FindElement(By.Name("Input.FirstName"));
            var lastNameField = _driver.FindElement(By.Name("Input.LastName"));
            var emailField = _driver.FindElement(By.Name("Input.Email"));
            var passwordField = _driver.FindElement(By.Name("Input.Password"));
            var confirmPasswordField = _driver.FindElement(By.Name("Input.ConfirmPassword"));
            

            // Fill in the registration form with an existing email (replace with a real existing email for testing)
            firstNameField.SendKeys("Test");
            lastNameField.SendKeys("User");
            string email = "duyduydz13+1@gmail.com";
            emailField.SendKeys(email); // Use an email that already exists in your database
            passwordField.SendKeys("Duyvip@13");
            confirmPasswordField.SendKeys("Duyvip@13");

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            // Submit the registration form

            var submitButton = _driver.FindElement(By.Id("registerSubmit"));
            Actions actions = new Actions(_driver);
            actions.MoveToElement(submitButton).Perform();
            submitButton.Click();
            // Wait for validation message (adjust the wait time if needed)

            wait.Until(driver =>
            {
                var validationMessages = driver.FindElements(By.ClassName("text-danger"));
                return validationMessages.Count > 0;
            });

            // Assert that the validation message indicating the email already exists is displayed
            var validationMessages = _driver.FindElements(By.ClassName("text-danger"));
            var emailValidationMessage = validationMessages.FirstOrDefault(msg => msg.Text.Contains($"Username '{email}' is already taken.")); // Adjust the validation message text

            Assert.NotNull(emailValidationMessage); // Ensure the validation message is displayed
        }


        [Fact]
        public void Register_WhenSuccessful_ShouldRedirectToConfirmationPage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/identity/account/register/");
            _driver.Manage().Window.Maximize();
            // Locate input fields and submit button
            var firstNameField = _driver.FindElement(By.Name("Input.FirstName"));
            var lastNameField = _driver.FindElement(By.Name("Input.LastName"));
            var emailField = _driver.FindElement(By.Name("Input.Email"));
            var passwordField = _driver.FindElement(By.Name("Input.Password"));
            var confirmPasswordField = _driver.FindElement(By.Name("Input.ConfirmPassword"));


            // Fill in the registration form with an existing email (replace with a real existing email for testing)
            firstNameField.SendKeys("Test");
            lastNameField.SendKeys("User");
            string email = "duyduydz13+5@gmail.com";
            emailField.SendKeys(email); // Use an email that already exists in your database
            passwordField.SendKeys("Duyvip@13");
            confirmPasswordField.SendKeys("Duyvip@13");

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            // Submit the registration form

            var submitButton = _driver.FindElement(By.Id("registerSubmit"));
            Actions actions = new Actions(_driver);
            actions.MoveToElement(submitButton).Perform();
            submitButton.Click();
            // Wait for validation message (adjust the wait time if needed)
            bool isRedirectedToConfirmationPage = false;
            try
            {
                wait.Until(d => d.Url.Contains("RegisterConfirmation") );
                isRedirectedToConfirmationPage = true;
            }
            catch (WebDriverTimeoutException)
            {
                // Check if the confirmation message is displayed on the page as a fallback
                var confirmationText = _driver.FindElement(By.TagName("h1")).Text;  
                isRedirectedToConfirmationPage = confirmationText.Contains("Register confirmation");
            }

            Assert.True(isRedirectedToConfirmationPage, "The registration should redirect to the confirmation page.");
        }
    }
}
