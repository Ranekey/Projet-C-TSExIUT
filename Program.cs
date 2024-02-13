//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System;

//class Program
//{
//    static void Main()
//    {
//        using (IWebDriver driver = new ChromeDriver())
//        {
//            string casUrl = "https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS";

//            // Demander à l'utilisateur de saisir le nom d'utilisateur
//            Console.Write("Nom d'utilisateur : ");
//            string username = Console.ReadLine();

//            // Demander à l'utilisateur de saisir le mot de passe (masqué)
//            Console.Write("Mot de passe : ");
//            string password = GetHiddenConsoleInput();

//            // Accéder à la page de CAS
//            driver.Navigate().GoToUrl(casUrl);

//            // Remplir le formulaire de connexion
//            IWebElement usernameField = driver.FindElement(By.Name("username"));
//            IWebElement passwordField = driver.FindElement(By.Name("password"));

//            usernameField.SendKeys(username);
//            passwordField.SendKeys(password);

//            // Soumettre le formulaire
//            passwordField.Submit();

//            // Attendre que la page se charge (peut varier en fonction du site)
//            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

//            // À ce stade, vous êtes connecté et pouvez effectuer d'autres opérations
//        }
//    }

//    // Méthode pour masquer l'entrée du mot de passe dans la console
//    private static string GetHiddenConsoleInput()
//    {
//        string password = "";
//        ConsoleKeyInfo key;

//        do
//        {
//            key = Console.ReadKey(true);

//            // Ignorer les touches non imprimables (par exemple, touches de modification)
//            if (!char.IsControl(key.KeyChar))
//            {
//                password += key.KeyChar;
//            }
//            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
//            {
//                // Gérer la touche de suppression arrière
//                password = password.Substring(0, password.Length - 1);
//            }
//        } while (key.Key != ConsoleKey.Enter);

//        Console.WriteLine(); // Nouvelle ligne après la saisie du mot de passe

//        return password;
//    }
//}


//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System;

//class Program
//{
//    static void Main()
//    {
//        using (IWebDriver driver = new ChromeDriver())
//        {
//            string casUrl = "https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS";

//            // Demander à l'utilisateur de saisir le nom d'utilisateur
//            Console.Write("Nom d'utilisateur : ");
//            string username = Console.ReadLine();

//            // Demander à l'utilisateur de saisir le mot de passe (masqué)
//            Console.Write("Mot de passe : ");
//            string password = GetHiddenConsoleInput();

//            // Accéder à la page de CAS
//            driver.Navigate().GoToUrl(casUrl);

//            // Remplir le formulaire de connexion
//            IWebElement usernameField = driver.FindElement(By.Name("username"));
//            IWebElement passwordField = driver.FindElement(By.Name("password"));

//            usernameField.SendKeys(username);
//            passwordField.SendKeys(password);

//            // Soumettre le formulaire
//            passwordField.Submit();

//            // Attendre que la page se charge (peut varier en fonction du site)
//            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

//            // À ce stade, vous êtes connecté et pouvez effectuer d'autres opérations
//            Console.WriteLine("Connecté avec succès.");

//            // Exemple d'action supplémentaire : Cliquer sur un lien après l'authentification
//            IWebElement someLink = driver.FindElement(By.XPath("//a[contains(text(),'Un lien spécifique')]"));
//            someLink.Click();

//            // Effectuer d'autres actions en fonction de vos besoins

//            // Ajouter une pause pour laisser le temps à l'utilisateur de voir la page (facultatif)
//            Console.WriteLine("Appuyez sur une touche pour fermer le navigateur.");
//            Console.ReadKey();
//        }
//    }

//    // Méthode pour masquer l'entrée du mot de passe dans la console
//    private static string GetHiddenConsoleInput()
//    {
//        string password = "";
//        ConsoleKeyInfo key;

//        do
//        {
//            key = Console.ReadKey(true);

//            // Ignorer les touches non imprimables (par exemple, touches de modification)
//            if (!char.IsControl(key.KeyChar))
//            {
//                password += key.KeyChar;
//            }
//            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
//            {
//                // Gérer la touche de suppression arrière
//                password = password.Substring(0, password.Length - 1);
//            }
//        } while (key.Key != ConsoleKey.Enter);

//        Console.WriteLine(); // Nouvelle ligne après la saisie du mot de passe

//        return password;
//    }
//}



using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;



class Program
{
    static void Main()
    {
        using (IWebDriver driver = new ChromeDriver())
        {
            string casUrl = "https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS";

            // Demander à l'utilisateur de saisir le nom d'utilisateur
            Console.Write("Nom d'utilisateur : ");
            string username = Console.ReadLine();

            // Demander à l'utilisateur de saisir le mot de passe (masqué)
            Console.Write("Mot de passe : ");
            string password = GetHiddenConsoleInput();

            // Accéder à la page de CAS
            driver.Navigate().GoToUrl(casUrl);

            // Remplir le formulaire de connexion
            IWebElement usernameField = driver.FindElement(By.Name("username"));
            IWebElement passwordField = driver.FindElement(By.Name("password"));

            usernameField.SendKeys(username);
            passwordField.SendKeys(password);

            // Soumettre le formulaire
            passwordField.Submit();

            // Attendre que la page se charge (peut varier en fonction du site)
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

            //  connecté

            //  extraire les titres des cours
            string coursesUrl = "https://mood.univ-st-etienne.fr/my/";
            string pageContent = DownloadPage(coursesUrl);
            List<string> courseTitles = ExtractCourseTitles(pageContent);

            // Afficher les titres des cours
            Console.WriteLine("Titres des cours :");
            foreach (string ligne in courseTitles)
            {
                Console.WriteLine(ligne);
            }

            // Exemple de contenu HTML
            string htmlContent = "<a class=\"list-group-item list-group-item-action  \" href=\"https://mood.univ-st-etienne.fr/course/view.php?id=3272\" data-key=\"3272\" data-isexpandable=\"1\" data-indent=\"1\" data-showdivider=\"0\" data-type=\"20\" data-nodetype=\"1\" data-collapse=\"0\" data-forceopen=\"0\" data-isactive=\"0\" data-hidden=\"0\" data-preceedwithhr=\"0\" data-parent-key=\"mycourses\"><div class=\"ml-1\"><div class=\"media\"><span class=\"media-left\"><i class=\"icon fa fa-graduation-cap fa-fw \" aria-hidden=\"true\" ></i></span><span class=\"media-body \">IUTSEGEII_BUT2_AII-ESE-44_BDD</span></div></div></a>";

            // Charger le contenu HTML dans HtmlDocument
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // Utiliser XPath pour sélectionner tous les liens (<a>) avec la classe spécifique
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//a[contains(@class, 'list-group-item-action')]");

            // Vérifier si des liens ont été trouvés
            if (linkNodes != null)
            {
                // Utiliser foreach pour parcourir chaque lien
                foreach (HtmlNode linkNode in linkNodes)
                {
                    // Récupérer le texte à l'intérieur du lien
                    string linkText = linkNode.SelectSingleNode(".//span[@class='media-body']")?.InnerText.Trim();

                    // Afficher le texte du lien
                    Console.WriteLine(linkText);

                    // Récupérer l'URL du lien
                    string linkUrl = linkNode.GetAttributeValue("href", "");
                    Console.WriteLine(linkUrl);
                }
            }

            // Ajout d'une pause 
            Console.WriteLine("Appuyez sur une touche pour fermer le navigateur.");
            Console.ReadKey();
        }
        



    }

    // Méthode pour masquer l'entrée du mot de passe dans la console
    private static string GetHiddenConsoleInput()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Ignorer les touches non imprimables (par exemple, touches de modification)
            if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                // Gérer la touche de suppression arrière
                password = password.Substring(0, password.Length - 1);
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Nouvelle ligne après la saisie du mot de passe

        return password;
    }

    // Méthode pour télécharger le contenu de la page web
    private static string DownloadPage(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return client.GetStringAsync(url).Result;
        }
    }

    // Méthode pour extraire les titres des cours à partir du contenu HTML
    private static List<string> ExtractCourseTitles(string htmlContent)
    {
        List<string> courseTitles = new List<string>();

        // Charger le contenu HTML dans HtmlDocument
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // Utiliser XPath pour extraire les titres des cours
        HtmlNodeCollection titleNodes = doc.DocumentNode.SelectNodes("//h2[@class='course-title']");

        if (titleNodes != null)
        {
            foreach (HtmlNode titleNode in titleNodes)
            {
                // Ajouter le titre du cours à la liste
                courseTitles.Add(titleNode.InnerText.Trim());
            }
        }

        return courseTitles;




    }
}

