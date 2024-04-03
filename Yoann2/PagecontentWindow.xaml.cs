using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using CefSharp;
using CefSharp.Wpf;
using System.Threading.Tasks;
using System.Xml.Linq;
using CefSharp.DevTools.Network;
using System.Threading;







namespace moodle2
{



public partial class PagecontentWindow : Window
{
    private IWebDriver driver;
        private string courselink;
        private string username;
        private string password;

    public PagecontentWindow(string courselink1, string username, string password)
        {
            this.courselink = courselink1;
            InitializeComponent();
            InitializeChromiumBrowser(); // Initialisation du contrôle ChromiumWebBrowser
            this.username = username;
            this.password = password;
        }

        private void InitializeChromiumBrowser()
    {
        // Créer une instance du contrôle ChromiumWebBrowser
        chromeBrowser = new ChromiumWebBrowser();
        // Ajouter le contrôle ChromiumWebBrowser à la grille (ou tout autre conteneur approprié)
        grid.Children.Add(chromeBrowser); // Suppose que vous avez une grille nommée 'grid' dans votre XAML

        //// Charger la page de connexion
        //chromeBrowser.Address = "https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS";
            // se diriger vers la page du cours
            chromeBrowser.Address = courselink;
            //se diriger vers la page de connexion la page de connexion https://mood.univ-st-etienne.fr/login/index.php?authCAS=CAS
  
            // Attacher l'événement FrameLoadEnd
            chromeBrowser.FrameLoadEnd += ChromeBrowser_FrameLoadEnd;
    }

    public async void ChromeBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
    {
        // Vérifier si la frame chargée est la frame principale de la page
        if (e.Frame.IsMain)
        {
                try
                {

                    // Effectuer l'authentification
                    await PerformAuthentication();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


            }
        }

    private Task PerformAuthentication()
    {

            //cliquer sur le bouton de connexion
            //chromeBrowser.EvaluateScriptAsync("document.querySelector('a[href=\"\"https://mood.univ-st-etienne.fr/login/index.php?authCAS=CAS\"\"]').click();");
            chromeBrowser.ExecuteScriptAsync("document.querySelector('a[href=\"https://mood.univ-st-etienne.fr/login/index.php?authCAS=CAS\"]').click();");
            chromeBrowser.ExecuteScriptAsync("document.getElementById('single_button660aab00e022a2').click();");//telecom marche pas


            //chromeBrowser.ExecuteScriptAsync("document.getElementById('username').value = '{username}';");
            chromeBrowser.ExecuteScriptAsync($"document.getElementById('username').value = '{username}';");

            chromeBrowser.ExecuteScriptAsync($"document.getElementById('password').value = '{password}';");


            //attendre un court instant pour permettre au chargement de se terminer
            System.Threading.Thread.Sleep(3000);

            //cliquer sur le bouton de connexion
            // Cliquer sur le bouton de connexion
            chromeBrowser.EvaluateScriptAsync("document.querySelector('button[name=\"submit\"]').click();");
            chromeBrowser.ExecuteScriptAsync("document.getElementById('loginbtn').click();");//telecom


            return Task.CompletedTask;
    }

        public void LoadPageContent(string url)
        {
            Dispatcher.Invoke(() =>
            {
                chromeBrowser.Address = url;
            });
        }


        // Méthode appelée lorsque la fenêtre est fermée avec bouton_click
        private void Button_Click(object sender, RoutedEventArgs e)
    {
        // Fermer la fenêtre
        Close();
    }

    // Méthode appelée lorsque la fenêtre est fermée
    private void PagecontentWindow_Closed(object sender, EventArgs e)
    {
        // Fermer le pilote ChromeDriver lorsque la fenêtre est fermée
        driver?.Quit();
    }
}

    }