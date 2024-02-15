//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using HtmlAgilityPack;
//using static System.Runtime.InteropServices.JavaScript.JSType;


//namespace Application_moodle
//{
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            InitializeComponent();
//        }

//        private void BtnLogin_Click(object sender, RoutedEventArgs e)
//        {
//            // Code à exécuter lors du clic sur le bouton
//            string username1 = txtUsername1.Text;
//            string username2 = txtUsername2.Text;
//            string password1 = pwdPassword1.Password;
//            string password2 = pwdPassword2.Password;



//class Program
//        {
//            static void Main()
//            {
//                using (IWebDriver driver = new ChromeDriver())
//                {
//                    AuthentifierSurSite(driver, "https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS");
//                    AuthentifierSurSite(driver, "https://www.telecom-st-etienne.fr/intranet/login.php?referer=https%3A%2F%2Fwww.telecom-st-etienne.fr%2Fintranet%2Findex.php");
//                }
//            }

//            static void AuthentifierSurSite(IWebDriver driver, string url)
//            {
//                // Demander à l'utilisateur de saisir le nom d'utilisateur
//                Console.Write("Nom d'utilisateur : ");
//                string username = Console.ReadLine();

//                // Demander à l'utilisateur de saisir le mot de passe (masqué)
//                Console.Write("Mot de passe : ");
//                string password = Fonctions.GetHiddenConsoleInput();

//                // Accéder à la page d'authentification
//                driver.Navigate().GoToUrl(url);

//                // Remplir le formulaire de connexion
//                IWebElement usernameField = driver.FindElement(By.Name("username"));
//                IWebElement passwordField = driver.FindElement(By.Name("password"));

//                usernameField.SendKeys(username);
//                passwordField.SendKeys(password);

//                // Soumettre le formulaire
//                passwordField.Submit();

//                // Attendre que la page se charge (peut varier en fonction du site)
//                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
//                // Ajout d'une pause 
//                Console.WriteLine("Appuyez sur une touche pour fermer le navigateur.");
//                Console.ReadKey();

//            }
//        }


//        // Exemple de message
//        MessageBox.Show($"Nom d'utilisateur 1: {username1}\nNom d'utilisateur 2: {username2}\nMot de passe 1: {password1}\nMot de passe 2: {password2}");
//        }
//}
//}
//using System.Windows;

//namespace Application_moodle
//{
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            InitializeComponent();
//        }

//        private void BtnLogin_Click(object sender, RoutedEventArgs e)
//        {
//            string username1 = txtUsername1.Text;
//            string username2 = txtUsername2.Text;
//            string password1 = pwdPassword1.Password;
//            string password2 = pwdPassword2.Password;

//            Authentification.AuthentifierSurSite("https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS", username1, password1);
//            Authentification.AuthentifierSurSite("https://www.telecom-st-etienne.fr/intranet/login.php?referer=https%3A%2F%2Fwww.telecom-st-etienne.fr%2Fintranet%2Findex.php", username2, password2);

//            // Ouvrir la nouvelle interface après l'authentification
//            OuvrirNouvelleInterface();
//        }

//        private void OuvrirNouvelleInterface()
//        {
//            // Créer et afficher la nouvelle fenêtre
//            DeuxiemeWindow nouvelleInterface = new DeuxiemeWindow();
//            nouvelleInterface.Show();

//            // Fermer la fenêtre actuelle si nécessaire
//            this.Close();
//        }
//    }
//}



using System.Windows;

namespace Application_moodle
{
    public partial class MainWindow : Window
    {
        private bool site1Authenticated = false;
        private bool site2Authenticated = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLoginSite1_Click(object sender, RoutedEventArgs e)
        {
            string username1 = txtUsername1.Text;
            string password1 = pwdPassword1.Password;

            // Authentifier sur le premier site
            site1Authenticated = Authentification.AuthentifierSurSite("https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS", username1, password1);

            if (site1Authenticated)
            {
                MessageBox.Show("Authentification réussie sur le site 1.");
            }
            else
            {
                MessageBox.Show("L'authentification sur le site 1 a échoué. Veuillez vérifier vos informations de connexion.");
            }

            OuvrirDeuxiemeWindowSiAuthentificationReussie();
        }

        private void BtnLoginSite2_Click(object sender, RoutedEventArgs e)
        {
            string username2 = txtUsername2.Text;
            string password2 = pwdPassword2.Password;

            // Authentifier sur le deuxième site
            site2Authenticated = Authentification.AuthentifierSurSite("https://www.telecom-st-etienne.fr/intranet/login.php?referer=https%3A%2F%2Fwww.telecom-st-etienne.fr%2Fintranet%2Findex.php", username2, password2);

            if (site2Authenticated)
            {
                MessageBox.Show("Authentification réussie sur le site 2.");
            }
            else
            {
                MessageBox.Show("L'authentification sur le site 2 a échoué. Veuillez vérifier vos informations de connexion.");
            }

            OuvrirDeuxiemeWindowSiAuthentificationReussie();
        }

        private void OuvrirDeuxiemeWindowSiAuthentificationReussie()
        {
            // Ouvrir la fenêtre DeuxiemeWindow si les deux sites sont authentifiés
            if (site1Authenticated && site2Authenticated)
            {
                DeuxiemeWindow deuxiemeWindow = new DeuxiemeWindow();
                deuxiemeWindow.Show();
                this.Close();
            }
        }
    }
}




//namespace Application_moodle
//{
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            // Add the directory containing chromedriver.exe to the PATH
//            string chromeDriverPath = "C:\\Users\\yoyof\\OneDrive\\Bureau\\C#\\Application_moodle";
//            Environment.SetEnvironmentVariable("PATH", $"{Environment.GetEnvironmentVariable("PATH")};{chromeDriverPath}", EnvironmentVariableTarget.Process);

//            InitializeComponent();
//        }

//        private void BtnLogin_Click(object sender, RoutedEventArgs e)
//        {
//            string username1 = txtUsername1.Text;
//            string username2 = txtUsername2.Text;
//            string password1 = pwdPassword1.Password;
//            string password2 = pwdPassword2.Password;

//            AuthentifierSurSite("https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS", username1, password1);
//            AuthentifierSurSite("https://www.telecom-st-etienne.fr/intranet/login.php?referer=https%3A%2F%2Fwww.telecom-st-etienne.fr%2Fintranet%2Findex.php", username2, password2);
//        }

//        private void AuthentifierSurSite(string url, string username, string password)
//        {
//            using (IWebDriver driver = new ChromeDriver())
//            {
//                driver.Navigate().GoToUrl(url);

//                IWebElement usernameField = driver.FindElement(By.Name("username"));
//                IWebElement passwordField = driver.FindElement(By.Name("password"));

//                usernameField.SendKeys(username);
//                passwordField.SendKeys(password);

//                passwordField.Submit();

//                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
//                IWebElement someElementOnNextPage = wait.Until(ExpectedConditions.ElementExists(By.Id("someElementId")));

//                MessageBox.Show("Attendez que la page se charge, puis cliquez sur OK pour fermer le navigateur.");
//            }
//        }
//    }
//}

//using System;
//using System.Windows;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Support.UI;

//namespace Application_moodle
//{
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            // Assuming chromedriver.exe is in the same directory as your application
//            string chromeDriverPath = AppDomain.CurrentDomain.BaseDirectory;

//            // Add the directory containing chromedriver.exe to the PATH
//            Environment.SetEnvironmentVariable("PATH", $"{Environment.GetEnvironmentVariable("PATH")};{chromeDriverPath}", EnvironmentVariableTarget.Process);

//            InitializeComponent();
//        }

//        private void BtnLogin_Click(object sender, RoutedEventArgs e)
//        {
//            string username1 = txtUsername1.Text;
//            string username2 = txtUsername2.Text;
//            string password1 = pwdPassword1.Password;
//            string password2 = pwdPassword2.Password;

//            AuthentifierSurSite("https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS", username1, password1);
//            AuthentifierSurSite("https://www.telecom-st-etienne.fr/intranet/login.php?referer=https%3A%2F%2Fwww.telecom-st-etienne.fr%2Fintranet%2Findex.php", username2, password2);
//        }

//        private void AuthentifierSurSite(string url, string username, string password)
//        {
//            using (IWebDriver driver = new ChromeDriver())
//            {
//                driver.Navigate().GoToUrl(url);

//                IWebElement usernameField = driver.FindElement(By.Name("username"));
//                IWebElement passwordField = driver.FindElement(By.Name("password"));

//                usernameField.SendKeys(username);
//                passwordField.SendKeys(password);

//                passwordField.Submit();

//                // Replace 'someElementId' with the actual ID or other selector of the element you are waiting for
//                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
//                IWebElement someElementOnNextPage = wait.Until(ExpectedConditions.ElementExists(By.Id("someElementId")));

//                MessageBox.Show("Attendez que la page se charge, puis cliquez sur OK pour fermer le navigateur.");
//            }
//        }
//    }
//}


