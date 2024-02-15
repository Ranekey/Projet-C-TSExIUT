//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System.Windows;

//namespace Application_moodle
//{
//    public static class Authentification
//    {
//        public static void AuthentifierSurSite(string url, string username, string password)
//        {
//            using (IWebDriver driver = new ChromeDriver())
//            {
//                driver.Navigate().GoToUrl(url);

//                IWebElement usernameField = driver.FindElement(By.Name("username"));
//                IWebElement passwordField = driver.FindElement(By.Name("password"));

//                usernameField.SendKeys(username);
//                passwordField.SendKeys(password);

//                passwordField.Submit();

//                MessageBox.Show("Attendez que la page se charge, puis cliquez sur OK pour fermer le navigateur.");
//            }
//        }
//    }
//}
//using OpenQA.Selenium;

//namespace Application_moodle
//{
//    public static class Authentification
//    {
//        public static void AuthentifierSurSite(string url, string username, string password)
//        {
//            using (IWebDriver driver = new ChromeDriver())
//            {
//                driver.Navigate().GoToUrl(url);

//                IWebElement usernameField = driver.FindElement(By.Name("username"));
//                IWebElement passwordField = driver.FindElement(By.Name("password"));

//                usernameField.SendKeys(username);
//                passwordField.SendKeys(password);

//                passwordField.Submit();

//                // Ajout de la méthode pour fermer la fenêtre
//                FermerFenetre(driver);
//            }
//        }

//        // Nouvelle méthode pour fermer la fenêtre
//        private static void FermerFenetre(IWebDriver driver)
//        {
//            // Utilisez les fonctionnalités de Selenium pour manipuler le navigateur
//            // Par exemple, pour fermer la fenêtre actuelle, vous pouvez utiliser driver.Close();
//            driver.Close();
//        }
//    }
//}




//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;

//namespace Application_moodle
//{
//    public static class Authentification
//    {
//        public static void AuthentifierSurSite(string url, string username, string password)
//        {
//            ChromeOptions options = new ChromeOptions();
//            options.AddArgument("--headless"); // Mode headless

//            using (IWebDriver driver = new ChromeDriver(options))
//            {
//                driver.Navigate().GoToUrl(url);

//                IWebElement usernameField = driver.FindElement(By.Name("username"));
//                IWebElement passwordField = driver.FindElement(By.Name("password"));

//                usernameField.SendKeys(username);
//                passwordField.SendKeys(password);

//                passwordField.Submit();

//                // Ne fermez pas la fenêtre ici, car cela fermerait également le navigateur headless.
//            }
//        }
//    }
//}
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Application_moodle
{
    public static class Authentification
    {
        public static bool AuthentifierSurSite(string url, string username, string password)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(url);

                    IWebElement usernameField = driver.FindElement(By.Name("username"));
                    IWebElement passwordField = driver.FindElement(By.Name("password"));

                    usernameField.SendKeys(username);
                    passwordField.SendKeys(password);

                    passwordField.Submit();

                    // Attente explicite pour vérifier si l'URL après l'authentification est celle attendue
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                    // Vérifier si l'URL commence par l'une des deux valeurs attendues
                    wait.Until(driver =>
                        driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/") ||
                        driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php")
                    );

                    // Si l'attente réussit, l'authentification est considérée comme réussie
                    return true;
                }
                catch (WebDriverTimeoutException)
                {
                    // L'attente a expiré, l'URL n'est pas celle attendue, l'authentification a échoué
                    return false;
                }
                finally
                {
                    // Fermez proprement le navigateur
                    driver.Quit();
                }
            }
        }
    }
}
