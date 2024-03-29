﻿
//using System.Windows;

//namespace Application_moodle
//{
//    public partial class AutreInterface : Window
//    {
//        public AutreInterface()
//        {
//            InitializeComponent();
//        }
//    }
//}
//using HtmlAgilityPack;
//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Xml;

//namespace Application_moodle
//{
//    public partial class DeuxiemeWindow : Window
//    {
//        public DeuxiemeWindow()
//        {
//            InitializeComponent();
//            ScrapCourses();
//        }

//        private void ScrapCourses()
//        {
//            string url = "https://mood.univ-st-etienne.fr/course/view.php?id=1540";

//            try
//            {
//                // Télécharger le contenu de la page
//                WebClient client = new WebClient();
//                string htmlContent = client.DownloadString(url);

//                // Vérifier si le contenu HTML est valide
//                if (!string.IsNullOrEmpty(htmlContent))
//                {
//                    // Charger le contenu HTML dans HtmlAgilityPack
//                    HtmlDocument doc = new HtmlDocument();
//                    doc.LoadHtml(htmlContent);

//                    // Extraire les informations sur les cours
//                    HtmlNodeCollection courseNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'activityinstance')]");
//                    if (courseNodes != null)
//                    {
//                        foreach (HtmlNode courseNode in courseNodes)
//                        {
//                            // Extraire les détails du cours
//                            string courseName = courseNode.SelectSingleNode(".//span[contains(@class, 'instancename')]").InnerText;
//                            string courseLink = courseNode.SelectSingleNode(".//a").GetAttributeValue("href", "");

//                            // Afficher les détails (vous pouvez modifier cela en fonction de vos besoins)
//                            MessageBox.Show("Nom du cours : " + courseName + "\nLien du cours : " + courseLink);
//                        }
//                    }
//                    else
//                    {
//                        MessageBox.Show("Aucun cours trouvé.");
//                    }
//                }
//                else
//                {
//                    MessageBox.Show("La page n'a pas pu être téléchargée.");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
//            }
//        }

//    }
//}
//using HtmlAgilityPack;
//using System;
//using System.Diagnostics;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;

//namespace Application_moodle
//{
//    public partial class DeuxiemeWindow : Window
//    {
//        public DeuxiemeWindow()
//        {
//            InitializeComponent();
//            ScrapCourses();
//        }

//        private void ScrapCourses()
//        {
//            string url = "https://mood.univ-st-etienne.fr/course/view.php?id=1540";

//            try
//            {
//                // Télécharger le contenu de la page
//                WebClient client = new WebClient();
//                string htmlContent = client.DownloadString(url);

//                // Vérifier si le contenu HTML est valide
//                if (!string.IsNullOrEmpty(htmlContent))
//                {
//                    // Charger le contenu HTML dans HtmlAgilityPack
//                    HtmlDocument doc = new HtmlDocument();
//                    doc.LoadHtml(htmlContent);

//                    // Extraire les informations sur les cours
//                    //HtmlNodeCollection courseNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'activityinstance')]");
//                    HtmlNodeCollection courseNodes = doc.DocumentNode.SelectNodes("//h2[@class='course-title']");
//                    HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//a[contains(@class, 'list-group-item-action')]");
//                    if (linkNodes != null)
//                    {
//                        foreach (HtmlNode linkNode in linkNodes)
//                        {
//                            // Extraire les détails du lien
//                            string linkText = linkNode.InnerText;
//                            string linkUrl = linkNode.GetAttributeValue("href", "");

//                            // Créer un bouton pour chaque lien
//                            Button linkButton = new Button();
//                            linkButton.Content = linkText;
//                            linkButton.Click += (sender, e) => OpenLink(linkUrl);

//                            // Ajouter le bouton à la fenêtre
//                            LinkStackPanel.Children.Add(linkButton);
//                        }
//                    }
//                    else
//                    {
//                        MessageBox.Show("Aucun lien trouvé.");
//                    }

//                    if (courseNodes != null)
//                    {
//                        foreach (HtmlNode courseNode in courseNodes)
//                        {
//                            // Extraire les détails du cours
//                            string courseName = courseNode.SelectSingleNode(".//span[contains(@class, 'instancename')]").InnerText;
//                            string courseLink = courseNode.SelectSingleNode(".//a").GetAttributeValue("href", "");

//                            // Créer un bouton pour chaque cours
//                            Button courseButton = new Button();
//                            courseButton.Content = courseName;
//                            courseButton.Click += (sender, e) => OpenCourseLink(courseLink);

//                            // Ajouter le bouton à la fenêtre
//                            CourseStackPanel.Children.Add(courseButton);
//                        }
//                    }
//                    else
//                    {
//                        MessageBox.Show("Aucun cours trouvé.");
//                    }
//                }
//                else
//                {
//                    MessageBox.Show("La page n'a pas pu être téléchargée.");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
//            }
//        }

//        // Méthode pour ouvrir le lien du cours
//        private void OpenCourseLink(string link)
//        {
//            try
//            {
//                System.Diagnostics.Process.Start(link);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Une erreur s'est produite lors de l'ouverture du lien : " + ex.Message);
//            }
//        }
//    }
//}
//using System;
//using System.Collections.Generic;
//using System.Windows;
//using System.ComponentModel;
//using HtmlAgilityPack;
//using System.IO;
//using System.Collections.ObjectModel;

//namespace Application_moodle
//{
//    public partial class DeuxiemeWindow : Window
//    {
//        private string filePath;

//        public DeuxiemeWindow(string filePath)
//        {
//            InitializeComponent();
//            this.filePath = filePath;
//            ScrapCoursesFromFile();
//        }

//        public void ScrapCoursesFromFile()
//        {
//            try
//            {
//                // Charger le contenu du fichier HTML
//                string htmlContent = File.ReadAllText(filePath);

//                // Charger le contenu HTML dans HtmlAgilityPack
//                HtmlDocument doc = new HtmlDocument();
//                doc.LoadHtml(htmlContent);

//                // Sélectionner tous les éléments span avec la classe media-body
//                var nodes = doc.DocumentNode.SelectNodes("//a[@class='list-group-item list-group-item-action  ']");



//                // Vérifier si des éléments ont été trouvés
//                if (nodes != null)
//                {
//                    // Liste pour stocker les noms des cours
//                    List<string> courseNames = new List<string>();

//                    foreach (var node in nodes)
//                    {
//                        var spanNode = node.SelectSingleNode(".//span[@class='media-body ']");
//                        if (spanNode != null)
//                        {
//                            // Récupérer le nom du cours

//                            courseNames.Add(node.InnerText.Trim());

//                            // Récupérer le lien du cours
//                            string courseLink = node.GetAttributeValue("href", "");


//                        }
//                    }
//                }
//                else
//                {
//                    MessageBox.Show("Aucun élément trouvé avec la classe 'media-body'.");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
//            }
//        }

//        private void OnCourseLinkClick(object sender, RoutedEventArgs e)
//        {
//            // Ajoutez ici le code pour ouvrir le lien du cours lorsqu'il est cliqué
//        }
//    }

//    public class Course : INotifyPropertyChanged
//    {
//        private string _courseName;
//        private string _courseLink;

//        public string CourseName
//        {
//            get { return _courseName; }
//            set { _courseName = value; NotifyPropertyChanged("CourseName"); }
//        }

//        public string CourseLink
//        {
//            get { return _courseLink; }
//            set { _courseLink = value; NotifyPropertyChanged("CourseLink"); }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void NotifyPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}


//using System;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.IO;
//using System.Net.Http;
//using System.Windows;
//using System.Windows.Controls;
//using HtmlAgilityPack;

//namespace Application_moodle
//{
//    public partial class DeuxiemeWindow : Window
//    {
//        public ObservableCollection<Course> Courses { get; } = new ObservableCollection<Course>();

//        private string filePath;

//        public DeuxiemeWindow(string filePath)
//        {
//            InitializeComponent();
//            this.filePath = filePath;
//            ScrapCoursesFromFile();
//            DataContext = this; // Définir le DataContext sur cette instance de fenêtre
//        }

//        public void ScrapCoursesFromFile()
//        {
//            try
//            {
//                // Charger le contenu du fichier HTML
//                string htmlContent = File.ReadAllText(filePath);

//                // Charger le contenu HTML dans HtmlAgilityPack
//                HtmlDocument doc = new HtmlDocument();
//                doc.LoadHtml(htmlContent);

//                // Sélectionner tous les éléments <a> avec la classe list-group-item
//                var nodes = doc.DocumentNode.SelectNodes("//a[@class='list-group-item list-group-item-action  ']");

//                // Vérifier si des éléments ont été trouvés
//                if (nodes != null)
//                {
//                    foreach (var node in nodes)
//                    {
//                        // Récupérer le nom du cours
//                        var spanNode = node.SelectSingleNode(".//span[@class='media-body ']");
//                        if (spanNode != null)
//                        {
//                            string courseName = spanNode.InnerText.Trim();

//                            // Récupérer le lien du cours
//                            string courseLink = node.GetAttributeValue("href", "");

//                            // Ajouter le cours à la liste des cours
//                            Courses.Add(new Course { CourseName = courseName, CourseLink = courseLink });
//                        }
//                    }
//                }
//                else
//                {
//                    MessageBox.Show("Aucun élément trouvé avec la classe 'media-body'.");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
//            }
//        }
//    }

//    public class Course : INotifyPropertyChanged
//    {
//        private string _courseName;
//        private string _courseLink;

//        public string CourseName
//        {
//            get { return _courseName; }
//            set { _courseName = value; NotifyPropertyChanged("CourseName"); }
//        }

//        public string CourseLink
//        {
//            get { return _courseLink; }
//            set { _courseLink = value; NotifyPropertyChanged("CourseLink"); }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void NotifyPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}

//using System;
//using System.IO;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using HtmlAgilityPack;

//namespace Application_moodle
//{
//    public partial class DeuxiemeWindow : Window
//    {
//        private string filePath;

//        public DeuxiemeWindow(string filePath)
//        {
//            InitializeComponent();
//            this.filePath = filePath;
//            ScrapCoursesFromFile();
//        }

//        private void ScrapCoursesFromFile()
//        {
//            try
//            {
//                // Charger le contenu du fichier HTML
//                string htmlContent = File.ReadAllText(filePath);

//                // Charger le contenu HTML dans HtmlAgilityPack
//                HtmlDocument doc = new HtmlDocument();
//                doc.LoadHtml(htmlContent);

//                // Sélectionner tous les éléments span avec la classe media-body
//                var nodes = doc.DocumentNode.SelectNodes("//a[@class='list-group-item list-group-item-action  ']");

//                // Vérifier si des éléments ont été trouvés
//                if (nodes != null)
//                {
//                    // Parcourir chaque élément trouvé
//                    foreach (var node in nodes)
//                    {
//                        var spanNode = node.SelectSingleNode(".//span[@class='media-body ']");
//                        if (spanNode != null)
//                        {
//                            // Récupérer le nom du cours
//                            string courseName = spanNode.InnerText.Trim();

//                            // Récupérer le lien du cours
//                            string courseLink = node.GetAttributeValue("href", "");

//                            // Créer un bouton pour chaque cours
//                            Button courseButton = new Button();
//                            courseButton.Content = courseName;
//                            courseButton.Click += async (sender, e) => await OpenCoursePage(courseLink);

//                            // Ajouter le bouton à la fenêtre
//                            CourseStackPanel.Children.Add(courseButton);
//                        }
//                    }
//                }
//                else
//                {
//                    MessageBox.Show("Aucun cours trouvé.");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
//            }
//        }

//        private async Task OpenCoursePage(string courseLink)
//        {
//            try
//            {
//                // Télécharger le contenu de la page du cours
//                HttpClient client = new HttpClient();
//                string pageContent = await client.GetStringAsync(courseLink);

//                // Afficher le contenu dans la fenêtre
//                PageContentTextBlock.Text = pageContent;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Une erreur s'est produite lors du chargement de la page du cours : " + ex.Message);
//            }
//        }
//    }
//}

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

