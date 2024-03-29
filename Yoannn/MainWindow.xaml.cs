

using System;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows;

namespace Application_moodle
{
    public partial class MainWindow : Window
    {
        private bool site1Authenticated = false;
        private bool site2Authenticated = false;
        private DeuxiemeWindow deuxiemeWindow;

        public MainWindow()
        {
            InitializeComponent();
            // Abonnez-vous à l'événement AuthenticationCompleted
            Authentification.AuthenticationCompleted += Authentification_AuthenticationCompleted;
        }

        // Méthode de gestion d'événements pour l'événement AuthenticationCompleted
        public void Authentification_AuthenticationCompleted(object sender, DeuxiemeWindow deuxiemeWindow)
        {
            // Assurez-vous que deuxiemeWindow n'est pas null
            if (deuxiemeWindow != null)
            {
                // Stockez l'instance de DeuxiemeWindow pour une utilisation ultérieure
                this.deuxiemeWindow = deuxiemeWindow;
            }
            else
            {
                // Gérez le cas où deuxiemeWindow est null
                MessageBox.Show("L'instance de DeuxiemeWindow est null.");
            }
        }
        public void Authentification_AuthenticationCompleted(object sender, EventArgs e)
        {
            // Assurez-vous que deuxiemeWindow n'est pas null
            if (deuxiemeWindow != null)
            {
                // Stockez l'instance de DeuxiemeWindow pour une utilisation ultérieure
                this.deuxiemeWindow = deuxiemeWindow;
            }
            else
            {
                // Gérez le cas où deuxiemeWindow est null
                MessageBox.Show("L'instance de DeuxiemeWindow est null.");
            }
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
                if (deuxiemeWindow != null)
                {
                    // Abonnez-vous à l'événement AuthenticationCompleted de DeuxiemeWindow
                    deuxiemeWindow.AuthenticationCompleted += Authentification_AuthenticationCompleted;
                    deuxiemeWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("L'instance de DeuxiemeWindow n'est pas initialisée.");
                }
            }
            else
            {
                MessageBox.Show("L'authentification sur le site 1 a échoué. Veuillez vérifier vos informations de connexion.");
            }

            OuvrirDeuxiemeWindowSiAuthentificationReussie(username1, password1);
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

            OuvrirDeuxiemeWindowSiAuthentificationReussie(username2, password2);
        }

        public void OuvrirDeuxiemeWindowSiAuthentificationReussie(string username,string password)
        {
            // Ouvrir la fenêtre DeuxiemeWindow si les deux sites sont authentifiés
            if (site1Authenticated && site2Authenticated)
            {
                DeuxiemeWindow deuxiemeWindow = new DeuxiemeWindow(Authentification.filePath, Authentification.filePath2, username, password);
                deuxiemeWindow.Show();
 
                this.Close();

            }
        }
    }

}



