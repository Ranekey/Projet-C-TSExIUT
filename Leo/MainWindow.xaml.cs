using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Projet_IUTxTSE.Models;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using OpenQA.Selenium.Support.UI;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace Projet_IUTxTSE
{
    public partial class MainWindow : Window
    {
        //déclaration du driver
        private IWebDriver driver;

        // Déclaration de la variable globale de liste de notes
        private List<Note2> notes = new List<Note2>();
        public MainWindow()
        {
            InitializeComponent();

            // Créer une instance de ChromeDriver
            driver = new ChromeDriver();
            
            // Ajouter un gestionnaire d'événements pour l'événement de fermeture de la fenêtre
            Closed += MainWindow_Closed;

        }

        // Gestionnaire d'événements pour la fermeture de la fenêtre
        void MainWindow_Closed(object sender, EventArgs e)
        {
            QuitDriver(); // Ferme le navigateur et libère les ressources
        }

        void QuitDriver()
        {
            if (driver != null)
            {
                driver.Quit(); // Ferme le navigateur et libère les ressources
                driver = null; // Assurez-vous de libérer la référence au pilote
            }
        }

        public async void Button_Click_IUT(object sender, RoutedEventArgs e)
        {
            string username = TextBox_IUT_username.Text;
            string password = PasswordBox_IUT.Password;

            string url = "https://cas.univ-st-etienne.fr/esup-cas/login";

            bool connectionIUT = await connecterSeleniumIUT(url, username, password);

            if (connectionIUT)
            {
                driver.Navigate().GoToUrl("https://intraiut.univ-st-etienne.fr/scodoc/report/");

                string htmlContent = driver.PageSource;

                notes.AddRange(ExtractNotesIUTFromHtml(htmlContent)); // Extraire les notes du document HTML
            }

        }
        private async void Button_Click_TSE(object sender, RoutedEventArgs e)
        {
            string username = TextBox_TSE_username.Text;
            string password = PasswordBox_TSE.Password;

            string url = "https://mootse.telecom-st-etienne.fr/login/index.php";

            bool connecterTSE = await connecterSeleniumTSE(url, username, password);

            if (connecterTSE)
            {
                driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/grade/report/overview/index.php");

                string htmlContent = driver.PageSource;

                notes.AddRange(ExtractNotesTSEFromHtml(htmlContent)); // Extraire les notes du document HTML
            }
        }

        private async Task<bool> connecterSeleniumIUT(string url, string username, string password)
        {
            try
            {
                // Ouvrir une page web
                driver.Navigate().GoToUrl(url);

                // Rechercher un élément sur la page
                IWebElement usernameField = driver.FindElement(By.Name("username"));
                IWebElement passwordField = driver.FindElement(By.Name("password"));

                // envoyer les valeurs dans les éléments
                usernameField.SendKeys(username);
                passwordField.SendKeys(password);

                // Soumettre le formulaire
                passwordField.Submit();

                try
                {
                    //vérifier si l'authentification a réussi ou non
                    IWebElement failElement = driver.FindElement(By.Id("loginErrorsPanel"));

                    // Authentification non réussie
                    MessageBox.Show("Non réussi");
                    return false;
                }

                catch (NoSuchElementException)
                {
                    //Authentification réussi
                    MessageBox.Show("réussi !!");
                    return true;
                }
                
            }
            catch (NoSuchElementException ex)
            {
                // Gérer l'erreur si un élément n'est pas trouvé
                MessageBox.Show($"Erreur: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Gérer d'autres exceptions
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
                return false;
            }

        }

        private async Task<bool> connecterSeleniumTSE(string url, string username, string password)
        {
            try
            {
                // Ouvrir une page web
                driver.Navigate().GoToUrl(url);

                // Rechercher un élément sur la page
                IWebElement usernameField = driver.FindElement(By.Name("username"));
                IWebElement passwordField = driver.FindElement(By.Name("password"));

                // envoyer les valeurs dans les éléments
                usernameField.SendKeys(username);
                passwordField.SendKeys(password);

                // Soumettre le formulaire
                passwordField.Submit();

                try
                {
                    //vérifier si l'authentification a réussi ou non
                    IWebElement failElement = driver.FindElement(By.Id("loginerrormessage"));

                    // Authentification non réussie
                    MessageBox.Show("Non réussi");
                    return false;
                }

                catch (NoSuchElementException)
                {
                    //Authentification réussi
                    MessageBox.Show("réussi !!");
                    return true;
                }

            }
            catch (NoSuchElementException ex)
            {
                // Gérer l'erreur si un élément n'est pas trouvé
                MessageBox.Show($"Erreur: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Gérer d'autres exceptions
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
                return false;
            }

        }

        private List<Note2> ExtractNotesIUTFromHtml(string html)
        {
            List<Note2> notes = new List<Note2>();

            // Charger le document HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Sélectionner tous les éléments de tableau (balise <tr>)
            var rows = doc.DocumentNode.SelectNodes("//tr");

            //initialiser module et ue compteur
            string currentModule = null;
            int ueCount = 0;

            // Parcourir chaque ligne
            foreach (var row in rows)
            {
                // Check if the row has class 'ue'
                if (row.GetClasses().Contains("ue"))
                {
                    // If it's a module row, extract module data
                    ueCount++;
                }
                // Check if the row has class 'module'
                else if (row.GetClasses().Contains("module"))
                {
                    // If it's a module row, extract module data
                    currentModule = row.InnerText.Trim();
                }

                // Check if the row has class 'evaluation'
                else if (ueCount == 1 && row.GetClasses().Contains("evaluation"))
                {
                    // Initialize variables to store evaluation data
                    string name = null;
                    string min = null;
                    string max = null;
                    string moy = null;
                    string note = null;
                    string coef = null;

                    // Sélectionner toutes les cellules dans la ligne
                    var cells = row.SelectNodes("td");
                    if (cells != null)
                    {
                        // Extraire les données pertinentes
                        name = cells[1].InnerText.Trim();
                        min = cells[2].InnerText.Trim();
                        max = cells[3].InnerText.Trim();
                        moy = cells[4].InnerText.Trim();
                        note = cells[5].InnerText.Trim();
                        coef = cells[6].InnerText.Trim();
                    }
                    // Ajouter la note à la liste
                    notes.Add(new Note2 { Module = currentModule, Name = name, Min = min, Max = max, Moy = moy, Note = note, Coefficient = coef });

                }
            }
            return notes;
        }

        private List<Note2> ExtractNotesTSEFromHtml(string html)
        {
            List<Note2> notes = new List<Note2>();

            // Charger le document HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            //Selectionner les éléments de tableau contenant les données des matières
            var tableRows = doc.DocumentNode.SelectNodes("//tr");


            // Parcourir chaque ligne du tableau
            foreach (var row in tableRows)
            {
                var cells = row.SelectNodes("td");
                // Vérifier si la ligne contient une note (ignorer les lignes vides)
                if (cells != null && cells.Count == 3 && cells[1].InnerText.Trim() != "-" && row.Attributes["class"].Value != "emptyrow")
                {
                    // Récupérer le lien et la note
                    var moduleLink = cells[0].SelectSingleNode("a").Attributes["href"].Value.Replace("amp;","");
                    var moduleName = cells[0].InnerText.Trim();
                    
                    //acceder au lien associé
                    driver.Navigate().GoToUrl(moduleLink);

                    string htmlNoteDetails = driver.PageSource;

                    //extraire les détails des notes
                    notes.AddRange(ExtractNoteDetailsFromHtml(htmlNoteDetails,moduleName));
                }
            }

            return notes;
        }

        private List<Note2> ExtractNoteDetailsFromHtml(string html, string module)
        {
            List<Note2> noteDetails = new List<Note2>();

            // Charger le document HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Sélectionner les éléments de tableau contenant les données des notes en détail
            var tableRows = doc.DocumentNode.SelectNodes("//tbody//tr");

            // Parcourir chaque ligne du tableau
            foreach (var row in tableRows)
            {
                var cells = row.SelectNodes("td | th");
                if (cells != null && cells.Count == 7 && cells[2].InnerText.Trim() != "-")
                {
                    string nomDevoir = cells[0].InnerText.Trim();
                    string coefficientText = cells[1].InnerText.Trim();
                    string noteText = cells[2].InnerText.Trim();
                    
                    //ajouter la note
                    noteDetails.Add(new Note2 { Module = module, Name = nomDevoir, Coefficient = coefficientText, Note = noteText });
                }
            }
            return noteDetails;
        }

        private void ShowNotesWindow(List<Note2> notes)
        {
            NotesWindow notesWindow = new NotesWindow(notes);
            notesWindow.Show(); // Afficher la nouvelle fenêtre
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            string username = TextBox_TSE_username.Text;
            string password = PasswordBox_TSE.Password;
            string ICS = await ExtractICS(username, password);
            MessageBox.Show(ICS);
        }

        private async Task<bool> connecterIntranetTSE(string url, string username, string password)
        {
            try
            {
                // Ouvrir une page web
                driver.Navigate().GoToUrl(url);

                // Rechercher un élément sur la page
                IWebElement usernameField = driver.FindElement(By.Name("username"));
                IWebElement passwordField = driver.FindElement(By.Name("password"));

                // envoyer les valeurs dans les éléments
                usernameField.SendKeys(username);
                passwordField.SendKeys(password);

                // Soumettre le formulaire
                passwordField.Submit();

                try
                {
                    //vérifier si l'authentification a réussi ou non
                    IWebElement failElement = driver.FindElement(By.ClassName("alert-danger"));

                    // Authentification non réussie
                    MessageBox.Show("Non réussi");
                    return false;
                }

                catch (NoSuchElementException)
                {
                    //Authentification réussi
                    MessageBox.Show("réussi !!");
                    return true;
                }

            }
            catch (NoSuchElementException ex)
            {
                // Gérer l'erreur si un élément n'est pas trouvé
                MessageBox.Show($"Erreur: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Gérer d'autres exceptions
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
                return false;
            }

        }

        private async Task<string> ExtractICS(string username, string password)
        {
            string urlIntranet = "https://www.telecom-st-etienne.fr/intranet/login.php";
            bool connecterTSE = await connecterIntranetTSE(urlIntranet, username, password);

            if (connecterTSE)
            {
                driver.Navigate().GoToUrl("https://www.telecom-st-etienne.fr/intranet/");

                // Charger le document HTML
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(driver.PageSource);

                HtmlNode bElement = doc.DocumentNode.SelectSingleNode("//footer//b");
                string iud = bElement.InnerText;
                string link = "https://www.telecom-st-etienne.fr/intranet/annuaire/annuaire_fiche.php?id=" + iud;
                MessageBox.Show(link);

                driver.Navigate().GoToUrl(link);

                // Charger le document HTML
                doc.LoadHtml(driver.PageSource);

                // Sélectionner l'élément <a> avec la classe "btn btn-primary" et un texte spécifique
                var elementsA = doc.DocumentNode.SelectNodes("//a[@class='btn btn-primary']");

                HtmlNode troisiemeElementA = elementsA[2];
                string texteElementA = troisiemeElementA.InnerText.Trim();

                string firstHalf = texteElementA.Substring(0, 6); // Extraire la première moitié du mot
                string secondHalf = texteElementA.Substring(6); // Extraire la deuxième moitié du mot

                string result = firstHalf + " " + secondHalf;

                string resultFinal = result.Replace('_', ' ');

                MessageBox.Show(resultFinal);

                string urlPlanning = "https://planning.univ-st-etienne.fr/direct/index.jsp?projectId=2&login=Etu&password=etudiant";

                driver.Navigate().GoToUrl(urlPlanning);

                // Définir le temps d'attente implicite à 10 secondes
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                // Sélectionner l'élément de recherche par son ID
                IWebElement searchInputElement = driver.FindElement(By.Id("x-auto-111-input"));

                searchInputElement.SendKeys(resultFinal);

                // Sélectionner le bouton de recherche par sa classe
                IWebElement searchButton = driver.FindElement(By.XPath("//button[@aria-describedby='x-auto-6']"));

                // Cliquer sur le bouton de recherche
                searchButton.Click();

                Thread.Sleep(10000);

                // Sélectionner le deuxième bouton en utilisant un XPath spécifique
                IWebElement secondButton = driver.FindElement(By.Id("x-auto-107"));
                
                // Cliquer sur le deuxième bouton
                secondButton.Click();

                
                // Sélectionner le bouton "Générer URL" par son texte
                //IWebElement generateUrlButton = driver.FindElement(By.XPath("//button[text()='Générer URL']"));


                // Sélectionner le bouton "Générer URL" par son texte
                IWebElement generateUrlButton = driver.FindElement(By.Id("x-auto-346"));
                

                generateUrlButton.Click();

                // Sélectionner l'élément contenant le lien
                IWebElement linkElement = driver.FindElement(By.Id("logdetail"));

                // Obtenir le lien contenu dans l'élément
                string linkUrlPlanning = linkElement.FindElement(By.TagName("a")).GetAttribute("href");

                // Pattern pour capturer les dates
                string pattern2 = @"(firstDate=)(\d{4}-\d{2}-\d{2})(&lastDate=)(\d{4}-\d{2}-\d{2})";
                
                // Remplacement des dates
                string modifiedUrl = Regex.Replace(linkUrlPlanning, pattern2, match =>
                {
                    DateTime startDate = DateTime.Parse(match.Groups[2].Value);
                    DateTime endDate = DateTime.Parse(match.Groups[4].Value);

                    // Modification des dates
                    DateTime newStartDate = new DateTime(startDate.Year, 9, 1); // 1er septembre
                    DateTime newEndDate = new DateTime(endDate.Year, 6, 30);    // 30 juin de l'année suivante

                    return $"{match.Groups[1].Value}{newStartDate:yyyy-MM-dd}{match.Groups[3].Value}{newEndDate:yyyy-MM-dd}";
                });

                return modifiedUrl;
            }

            return "erreur";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowNotesWindow(notes);
        }
    }

}