using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows;

namespace Application_moodle
{
    public static class AuthenticationHelper
    {
        public static void AuthentifierSurSite(string url, string username, string password)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(url);

                IWebElement usernameField = driver.FindElement(By.Name("username"));
                IWebElement passwordField = driver.FindElement(By.Name("password"));

                usernameField.SendKeys(username);
                passwordField.SendKeys(password);

                passwordField.Submit();

                MessageBox.Show("Attendez que la page se charge, puis cliquez sur OK pour fermer le navigateur.");
            }
        }
    }
}
