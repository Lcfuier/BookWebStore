using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium.Interactions;

namespace BookWebStore.Automation
{
    public class BookControllerTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5160"; // Adjust based on your app's URL
        private readonly WebDriverWait _wait;
        public BookControllerTests()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void Dispose()
        {
            _driver.Quit();
        }
        [Fact]
        public void AddBookToCart_ShouldUpdateCart_WhenUserIsLoggedIn()
        {
            // Navigate to the login page and log in
            _driver.Navigate().GoToUrl($"{_baseUrl}/identity/account/login/");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Fill in login form
            _driver.FindElement(By.Id("Input_Email")).SendKeys("duyduydz13+1@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Duyvip@13");
            _driver.FindElement(By.Id("login-submit")).Click();

            // Wait for login to complete by checking for a logout button or redirect
            wait.Until(driver => driver.FindElement(By.CssSelector("form#logoutForm button#logout")));

            // Navigate to the book detail page
            _driver.Navigate().GoToUrl($"{_baseUrl}/Book/Detail/9c6a9b18-b6ed-48ee-f5dd-08dce9e9840f/"); // Use the correct book ID or URL
             string expectedBookTitle = "TOI DI CODE DAO";
            // Verify we're on the right page
            Assert.Equal(expectedBookTitle, _driver.FindElement(By.CssSelector("h3.text-uppercase")).Text);

            // Set quantity
            var quantityInput = wait.Until(driver => driver.FindElement(By.CssSelector("#CartItemDTO_Quantity")));
            quantityInput.Clear();
            quantityInput.SendKeys("2"); // Example quantity

            var addToCartButton = wait.Until(driver => driver.FindElement(By.CssSelector("button[type='submit'][formaction='/user/cart/addcartitemforbookdetails/']")));

            // Scroll the button into view
            Actions actions = new Actions(_driver);
            actions.MoveToElement(addToCartButton).Perform();

            // Click the 'Add to Cart' button
            addToCartButton.Click();


            // Wait until the cart updates (or navigate to the cart page to verify)
            _driver.Navigate().GoToUrl($"{_baseUrl}/user/cart");

            // Verify the cart has been updated
            var cartItemTitle = wait.Until(driver => driver.FindElement(By.CssSelector("h5.text-uppercase.text-secondary a")));
            Assert.Equal(expectedBookTitle, cartItemTitle.Text);

           /* var cartItemQuantity = wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class, 'align-self-center')]/span[contains(@class, 'fw-bold')]")));
            Assert.Equal("2", cartItemQuantity.Text);*/

        }
        [Fact]
        public void CreatePublisher_ShouldUpdateDB_WhenUserIsLoggedIn()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/identity/account/login/");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Fill in login form
            _driver.FindElement(By.Id("Input_Email")).SendKeys("duyduydz13@gmail.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Duyvip@13");
            _driver.FindElement(By.Id("login-submit")).Click();

            // Navigate to the book detail page
            _driver.Navigate().GoToUrl($"{_baseUrl}/librarian/publisher/");
            var createNewButton = _driver.FindElement(By.CssSelector(".btn.btn-primary"));
            

            // Scroll the button into view
            Actions actions = new Actions(_driver);
            actions.MoveToElement(createNewButton).Perform();

            // Click the 'Add to Cart' button
            createNewButton.Click();
            _wait.Until(driver => driver.Title.Contains("Thêm Nhà Xuất Bản"));
            // Check if the form title is correct
            var formTitle = _driver.FindElement(By.TagName("h2")).Text;
            Assert.Equal("Thêm Nhà Xuất Bản", formTitle);

            // Input valid Publisher name
            var nameInput = _driver.FindElement(By.Name("Name"));
            nameInput.SendKeys("New Publisher");

            // Submit the form
            var submitButton = _driver.FindElement(By.CssSelector("button[type='submit'][class='btn btn-primary form-control']"));

            // Scroll the button into view
            // Click the 'Add to Cart' button
            submitButton.Click();
            // Verify if the Publisher was added (check for Publisher list page or success message)
            var pageTitle = _driver.FindElement(By.TagName("h2")).Text;
            Assert.Equal("Quản lý nhà xuất bản", pageTitle);
        }
        }
    }
