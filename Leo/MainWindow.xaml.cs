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

namespace Projet_IUTxTSE
{
    public partial class MainWindow : Window
    {
        private IWebDriver driver;
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

        private async void Button_Click_IUT(object sender, RoutedEventArgs e)
        {
            string username = TextBox_IUT_username.Text;
            string password = PasswordBox_IUT.Password;

            string url = "https://cas.univ-st-etienne.fr/esup-cas/login";

            bool connectionIUT = await connecterSeleniumIUT(url, username, password);

            if (connectionIUT)
            {
                driver.Navigate().GoToUrl("https://intraiut.univ-st-etienne.fr/scodoc/report/");

                string htmlContent = driver.PageSource;

                List<Note2> notes = ExtractNotesIUTFromHtml(htmlContent); // Extraire les notes du document HTML
                ShowNotesWindow(notes); // Afficher les notes dans une nouvelle fenêtre
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

                List<Note2> notes = ExtractNotesTSEFromHtml(htmlContent); // Extraire les notes du document HTML
                ShowNotesWindow(notes); // Afficher les notes dans une nouvelle fenêtre
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
            var rows = doc.DocumentNode.SelectNodes("//tr[@class='evaluation']");

            // Parcourir chaque ligne
            foreach (var row in rows)
            {
                // Sélectionner toutes les cellules dans la ligne
                var cells = row.SelectNodes("td");

                // Extraire les données pertinentes
                string module = cells[1].InnerText.Trim();
                string min = cells[2].InnerText.Trim();
                string max = cells[3].InnerText.Trim();
                string moy = cells[4].InnerText.Trim();
                string note = cells[5].InnerText.Trim();
                string coef = cells[6].InnerText.Trim();

                // Ajouter la note à la liste
                notes.Add(new Note2 { Module = module, Min = min, Max = max, Moy = moy, Note = note, Coefficient = coef });
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
            var tableRows = doc.DocumentNode.SelectNodes("//table[@id='overview-grade']//tr[position()>0]");

            if (tableRows != null)
            {
                // Parcourir chaque ligne du tableau
                foreach (var row in tableRows)
                {
                    if (row.Attributes["class"] != null && row.Attributes["class"].Value == "emptyrow")
                    {
                        // La ligne est vide, passer à la suivante
                        continue;
                    }

                    var cells = row.SelectNodes("td");
                    if (cells != null && cells.Count >= 3)
                    {
                        // Récupérer le nom de la matière (1ère colonne) et lien associé
                        string module = cells[0].InnerText.Trim();
                        string link = cells[0].SelectSingleNode(".//a").GetAttributeValue("href", "").Replace("&amp;", "&");

                        // Récupérer la note (2ème colonne)
                        string noteTexte = cells[1].InnerText.Trim();

                        if (float.TryParse(noteTexte,out float note))
                        {
                            //acceder au lien associé
                            driver.Navigate().GoToUrl(link);

                            string htmlNoteDetails = driver.PageSource;

                            //extraire les détails des notes
                            notes.AddRange(ExtractNoteDetailsFromHtml(htmlNoteDetails));
                        }
                    }
                }
            }

            return notes;
        }

        private List<Note2> ExtractNoteDetailsFromHtml(string html)
        {
            List<Note2> noteDetails = new List<Note2>();

            // Charger le document HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Sélectionner les éléments de tableau contenant les données des notes en détail
            var tableRows = doc.DocumentNode.SelectNodes("//tr[contains(@class, 'cat_')]");

            if (tableRows != null)
            {
                // Parcourir chaque ligne du tableau
                foreach (var row in tableRows)
                {
                    var cells = row.SelectNodes("td | th");
                    if (cells != null && cells.Count >= 7)
                    {
                        string devoir = cells[0].InnerText.Trim();
                        string coefficientText = cells[1].InnerText.Trim();
                        string noteText = cells[2].InnerText.Trim();

                        if (float.TryParse(noteText,out float note))
                        {
                            noteDetails.Add(new Note2 { Name = devoir, Coefficient = coefficientText, Note = noteText });
                        }
                    }
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

                Thread.Sleep(5000);

                // Sélectionner le deuxième bouton en utilisant un XPath spécifique
                IWebElement secondButton = driver.FindElement(By.XPath("//button[@aria-describedby='x-auto-1']"));
                
                // Cliquer sur le deuxième bouton
                secondButton.Click();

                
                // Sélectionner le bouton "Générer URL" par son texte
                IWebElement generateUrlButton = driver.FindElement(By.XPath("//button[text()='Générer URL']"));

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

        
    }

}