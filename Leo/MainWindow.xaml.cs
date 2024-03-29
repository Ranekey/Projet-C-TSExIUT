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
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Projet_IUTxTSE.Models;

/*namespace Projet_IUTxTSE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click_IUT(object sender, RoutedEventArgs e)
        {
            // Appel de la méthode qui effectue la requête HTTP (en mode asynchrone avec le mot-clé "await" pour
            // éviter de bloquer l'application à l'éxecution
            string url = "https://cas.univ-st-etienne.fr/esup-cas/login";
            var username_IUT = TextBox_IUT_username.Text;
            var password_IUT = PasswordBox_IUT.Password;

            await MakeHttpRequest(url, username_IUT, password_IUT);
        }
        private void Button_Click_TSE(object sender, RoutedEventArgs e)
        {

        }

        private async Task MakeHttpRequest(string url, string username, string password)
        {
            try
            {
                using (HttpClient client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true }))
                {
                    //récupérer les informations sur execution
                    var responseGET = await client.GetAsync(url);

                    //convertir les infos en un document html
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(await responseGET.Content.ReadAsStringAsync());

                    //rechercher dans le document execution
                    var element_execution = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='execution']");

                    //verifier si l'element a ete trouve
                    var valeur_execution = element_execution.Attributes["value"].Value;

                    username = "vl03824x";
                    password = "^Jtx6'$CSN%V";

                    var encoded_username = HttpUtility.UrlEncode(username);
                    var encoded_password = HttpUtility.UrlEncode(password);
                    var encoded_execution = HttpUtility.UrlEncode(valeur_execution);

                    //créer un type dictionnaire qui prend une clé de type string et une valeur de type string
                    var informations = new Dictionary<string, string>
                    {
                        { "username", encoded_username },
                        { "password", encoded_password },
                        { "execution", encoded_execution },
                        { "_eventId", "submit" },
                        { "geolocation", "" }
                    };

                    //traduis les informations en un type HttpContent qui peut être lu par une page web
                    var send_content = new FormUrlEncodedContent(informations);
                    
                    // Effectuer la requête POST
                    HttpResponseMessage response = await client.PostAsync(url, send_content);
                    
                    // Vérifier si la requête a réussi (code de statut 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // Traiter la réponse ici (par exemple, extraire le contenu)
                        string content = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Réponse de l'API : " + content);
                    }
                    else
                    {
                        MessageBox.Show($"Erreur de requête : {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        
    }
}*/

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

                List<Note2> notes = ExtractNotesFromHtml(htmlContent); // Extraire les notes du document HTML
                ShowNotesWindow(notes); // Afficher les notes dans une nouvelle fenêtre
            }

        }
        private async void Button_Click_TSE(object sender, RoutedEventArgs e)
        {
            string username = TextBox_TSE_username.Text;
            string password = PasswordBox_TSE.Password;

            string url = "https://www.telecom-st-etienne.fr/intranet/login.php";

            bool connecterTSE = await connecterSeleniumTSE(url, username, password);


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
                    IWebElement failElement = driver.FindElement(By.ClassName("alert-sm"));

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

        private List<Note2> ExtractNotesFromHtml(string html)
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

        private void ShowNotesWindow(List<Note2> notes)
        {
            NotesWindow notesWindow = new NotesWindow(notes);
            notesWindow.Show(); // Afficher la nouvelle fenêtre
        }
        
    }

}