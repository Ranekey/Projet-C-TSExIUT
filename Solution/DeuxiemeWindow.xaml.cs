using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Windows.Shapes;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shell;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Security.Policy;








namespace Application_moodle
{
    public partial class DeuxiemeWindow : Window
    {
        private string filePath;
    
        private string username;
        private string password;
        public event EventHandler<EventArgs> AuthenticationCompleted;
        // Déclarations des fonctions Windows API pour afficher le navigateur dans la fenêtre WPF
        [DllImport("user32.dll")]
        private static extern bool SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);





        


        // Constantes pour les fonctions Windows API
        private const int SW_SHOWMAXIMIZED = 3;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOMOVE = 0x0002;

        // Appeler cet événement lorsque l'authentification est réussie
        private void OnAuthenticationCompleted()
        {
            AuthenticationCompleted?.Invoke(this, EventArgs.Empty);

        }

        public DeuxiemeWindow(string filePath,string filePath2, string username, string password)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.filePath = filePath2;
            
            this.username = username;
            this.password = password;
            ScrapCoursesFromFile1(filePath);
            ScrapCoursesFromFile2(filePath2);
        }
        private void ScrapCoursesFromFile2(string filePath)
        {
            try
            {
                // Charger le contenu du fichier HTML
                string htmlContent = File.ReadAllText(filePath);

                // Charger le contenu HTML dans HtmlAgilityPack
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                
                   var courseBoxes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'coursebox')]");

                if (courseBoxes != null)
                {
                    // Parcourir chaque élément de la classe "coursebox" pour extraire les informations
                    foreach (var courseBox in courseBoxes)
                    {
                        // Récupérer le nom du cours et le lien associé
                        var courseNameNode = courseBox.SelectSingleNode(".//h3[@class='coursename']/a");
                        var courseName = courseNameNode?.InnerText.Trim();
                        var courseLink = courseNameNode?.GetAttributeValue("href", "");
                        
                        Button courseButton = new Button();
                        courseButton.Content = courseName;
                        courseButton.Click += async (sender, e) => await OpenCoursePage(courseLink);
                        CourseStackPanel.Children.Add(courseButton);

                    }
                }
                else
                {
                    MessageBox.Show("Aucun cours trouvé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
        }
        private void ScrapCoursesFromFile1(string filePath)
        {
            try
            {
                // Charger le contenu du fichier HTML
                string htmlContent = File.ReadAllText(filePath);

                // Charger le contenu HTML dans HtmlAgilityPack
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // Sélectionner tous les éléments span avec la classe media-body
                var nodes = doc.DocumentNode.SelectNodes("//a[@class='list-group-item list-group-item-action  ']");

                // Vérifier si des éléments ont été trouvés
                if (nodes != null)
                {
                    // Parcourir chaque élément trouvé
                    foreach (var node in nodes)
                    {
                        var spanNode = node.SelectSingleNode(".//span[@class='media-body ']");
                        if (spanNode != null)
                        {
                            // Récupérer le nom du cours
                            string courseName = spanNode.InnerText.Trim();

                            // Récupérer le lien du cours
                            string courseLink = node.GetAttributeValue("href", "");

                            // Créer un bouton pour chaque cours
                            Button courseButton = new Button();
                            courseButton.Content = courseName;
                            courseButton.Click += async (sender, e) => await OpenCoursePage(courseLink);
                            



                            // Ajouter le bouton à la fenêtre
                            CourseStackPanel.Children.Add(courseButton);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Aucun cours trouvé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
        }

        ////ouvrir dans DeuxiemeWindow une page web diriger vers le lien du cours
        //private async Task OpenCoursePage(string courseLink)
        //{
        //    Process.Start(new ProcessStartInfo(courseLink) { UseShellExecute = true });
        //}


        private async Task OpenCoursePage(string courseLink)
        {
            ChromeOptions options = new ChromeOptions();

            using (IWebDriver driver = new ChromeDriver(options))
            {
                try
                {

                    
                    courseLink = courseLink.Replace("&amp;", "&");
                    driver.Navigate().GoToUrl("https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS");

                    IWebElement usernameField = driver.FindElement(By.Name("username"));
                    IWebElement passwordField = driver.FindElement(By.Name("password"));

                    usernameField.SendKeys(username);
                    passwordField.SendKeys(password);

                    passwordField.Submit();

                    // Attendez que l'authentification soit terminée
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => d.Url.StartsWith("https://mood.univ-st-etienne.fr/my/") || d.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php"));
                    driver.Navigate().GoToUrl("https://mood.univ-st-etienne.fr/course/view.php?id=1540");

                    if (driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/"))
                    {

                        driver.Navigate().GoToUrl("https://mood.univ-st-etienne.fr/course/view.php?id=1540");

                    }
                    if (driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php"))
                    {

                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/");
                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/course/index.php?categoryid=65");
                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/course/index.php?categoryid=157");
                    }

                    driver.Navigate().GoToUrl(courseLink);


                    //ouvrir le navigateur dans une fenêtre séparée qui est pagecontent.xaml.cs

                    //PagecontentWindow pagecontentWindow = new PagecontentWindow();

                    //pagecontentWindow.LoadPageContent(courseLink);

                    //pagecontentWindow.Show();






                    //Attendre un certain délai avant d'afficher le navigateur
                    await Task.Delay(5000); // 5 secondes

                    //afficher le driver
                    driver.Manage().Window.Maximize();



                    //Afficher le navigateur en change

                    ////mettre le navigateur dans la page wpf

                    //SetBrowserInWpf((IntPtr)driver.CurrentWindowHandle, 1024, 768);
                    //creer la methode SetBrowserInWpf


                    //Attendre un certain délai avant de fermer le navigateur
                    /*await Task.Delay(5000); */// 5 secondes



                    //SetBrowserInWpf((IntPtr)driver.CurrentWindowHandle, 1024, 768);                         








                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur s'est produite : " + ex.Message);
                    // Gérer l'erreur d'une manière appropriée, comme la journalisation ou la prise d'autres mesures
                }
               

            }


        }
        private void SetBrowserInWpf(IntPtr browserHandle, int width, int height)
        {
            try
            {
                IntPtr wpfHandle = new WindowInteropHelper(this).Handle;

                // Configure the size and position of the browser
                SetWindowPos(browserHandle, IntPtr.Zero, 0, 0, width, height, SWP_NOZORDER | SWP_NOMOVE);

                // Set the browser as a child of the WPF window
                SetParent(browserHandle, wpfHandle);

                // Show the browser
                ShowWindow(browserHandle, SW_SHOWMAXIMIZED);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while configuring the browser window: " + ex.Message);
            }
        }
        

      

    }
}

