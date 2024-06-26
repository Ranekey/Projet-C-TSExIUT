﻿
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.IO;
using System.Numerics;
namespace Application_moodle
{
    public class Authentification
    {
        public static string filePath = "C:/Users/yoyof/OneDrive/Bureau/C#/Application_moodle/bin/Debug/net8.0-windows/page1.html";
        public static string filePath2 = "C:/Users/yoyof/OneDrive/Bureau/C#/Application_moodle/bin/Debug/net8.0-windows/page2.html";
        public delegate void AuthenticationCompletedEventHandler(object sender, DeuxiemeWindow deuxiemeWindow);
        public static event AuthenticationCompletedEventHandler AuthenticationCompleted;
        private readonly IWebDriver driver;

        private static bool Authentifier(IWebDriver driver, string url, string username, string password)
        {
            driver.Navigate().GoToUrl(url);

            // Récupérer les éléments pour le nom d'utilisateur et le mot de passe
            IWebElement usernameField = driver.FindElement(By.Name("username"));
            IWebElement passwordField = driver.FindElement(By.Name("password"));

            // Saisir les identifiants
            usernameField.SendKeys(username);
            passwordField.SendKeys(password);

            // Soumettre le formulaire
            passwordField.Submit();

            // Attendre jusqu'à ce que l'URL après l'authentification soit celle attendue
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            return wait.Until(driver =>
                driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/") ||
                driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php")
            );
        }
        public static bool AuthentifierSurSite(string url, string username, string password)
        {

            //string chromeDriverPath = @"C:\Users\yoyof\OneDrive\Bureau\C#\Application_moodle\chromedriver.exe";

            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless"); // Exécuter Chrome en mode headless
            //options.AddArgument("--disable-gpu"); // Désactiver l'accélération matérielle en mode headless
            //options.AddArgument("--silent"); // Exécuter le navigateur en mode silencieux
            //options.AddArgument("--log-level=3"); // Définir le niveau de journalisation minimal

            //// Rediriger la sortie standard et d'erreur vers un flux nul
            //options.AddArgument("--disable-logging");
            //options.AddArgument("--disable-logging-redirect");



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

                    if (driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/"))
                    {

                        driver.Navigate().GoToUrl("https://mood.univ-st-etienne.fr/course/view.php?id=1540");
                        string pageSource = driver.PageSource;


                        File.WriteAllText(filePath, pageSource);



                    }
                    if (driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php"))
                    {

                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/");
                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/course/index.php?categoryid=65");
                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/course/index.php?categoryid=157");
                        string pageSource2 = driver.PageSource;
                        File.WriteAllText(filePath2, pageSource2);
                    }

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





//using Newtonsoft.Json;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Support.UI;
//using System;
//using System.IO;

//namespace Application_moodle
//{
//    public class Authentification
//    {
//        public static string filePath = "C:/Users/yoyof/OneDrive/Bureau/C#/Application_moodle/bin/Debug/net8.0-windows/page1.html";
//        public static string filePath2 = "C:/Users/yoyof/OneDrive/Bureau/C#/Application_moodle/bin/Debug/net8.0-windows/page2.html";
//        public static IWebDriver driver;

//        public delegate void AuthenticationCompletedEventHandler(object sender, EventArgs e);
//        public static event AuthenticationCompletedEventHandler AuthenticationCompleted;

//        public static bool AuthentifierSurSite(string url, string username, string password)
//        {
//            try
//            {
//                // Initialiser le pilote ChromeDriver
              
//                driver = new ChromeDriver();

//                // Authentifier sur le site
//                bool authentificationReussie = Authentifier(driver, url, username, password);

//                // Sauvegarder la page source en fonction de l'URL
//                if (authentificationReussie)
//                {
//                    string pageSource = driver.PageSource;
//                    if (driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/"))
//                        File.WriteAllText(filePath, pageSource);
//                    else if (driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php"))
//                        File.WriteAllText(filePath2, pageSource);

//                    // Informer que l'authentification est terminée
//                    AuthenticationCompleted?.Invoke(null, EventArgs.Empty);
//                }

//                return authentificationReussie;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Erreur lors de l'authentification : " + ex.Message);
//                return false;
//            }
           
//        }

//        private static bool Authentifier(IWebDriver driver, string url, string username, string password)
//        {
//            driver.Navigate().GoToUrl(url);

//            // Récupérer les éléments pour le nom d'utilisateur et le mot de passe
//            IWebElement usernameField = driver.FindElement(By.Name("username"));
//            IWebElement passwordField = driver.FindElement(By.Name("password"));

//            // Saisir les identifiants
//            usernameField.SendKeys(username);
//            passwordField.SendKeys(password);

//            // Soumettre le formulaire
//            passwordField.Submit();

//            // Après avoir réussi l'authentification, récupérez les cookies ou les informations d'authentification
//            var cookies = driver.Manage().Cookies.AllCookies;

//            // Transmettez ces informations à votre application WPF
//            string serializedCookies = JsonConvert.SerializeObject(cookies);
//            File.WriteAllText("cookies.json", serializedCookies);

//            // Attendre jusqu'à ce que l'URL après l'authentification soit celle attendue
//            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
//            return wait.Until(driver =>
//                driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/") ||
//                driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php")
//            );
//        }
//        // Méthode pour récupérer l'instance du pilote
//        public IWebDriver GetWebDriver()
//        {
//            return driver;
//        }
//        public void AjouterCookie(string nom, string valeur, string domaine)
//        {
//            Cookie cookie = new Cookie(nom, valeur, domaine, "/", null);
//            driver.Manage().Cookies.AddCookie(cookie);

//        }
//    }
//}
