

using System;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows;

namespace moodle2
{
    public partial class MainWindow : Window
    {
        private bool site1Authenticated = false;
        private bool site2Authenticated = false;
        private DeuxiemeWindow deuxiemeWindow;
        private string username1;
        private string password1;
        private string username2;
        private string password2;

        public MainWindow()
        {
            InitializeComponent();
         
        }

        private void BtnLoginSite1_Click(object sender, RoutedEventArgs e)
        {
            username1 = txtUsername1.Text;
            password1 = pwdPassword1.Password;

            // Authentifier sur le premier site
            site1Authenticated = Authentification.AuthentifierSurSite("https://cas.univ-st-etienne.fr/esup-cas/login?service=https%3A%2F%2Fmood.univ-st-etienne.fr%2Flogin%2Findex.php%3FauthCAS%3DCAS", username1, password1);

            if (site1Authenticated)
            {
                MessageBox.Show("Authentification réussie sur le site 1.");
                if (deuxiemeWindow != null)
                {
                    // Abonnez-vous à l'événement AuthenticationCompleted de DeuxiemeWindow
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

            OuvrirDeuxiemeWindowSiAuthentificationReussie(username1, password1, username2, password2);

        }

        private void BtnLoginSite2_Click(object sender, RoutedEventArgs e)
        {
            username2 = txtUsername2.Text;
            password2 = pwdPassword2.Password;

            // Authentifier sur le deuxième site
            site2Authenticated = Authentification.AuthentifierSurSite("https://mootse.telecom-st-etienne.fr/login/index.php", username2, password2);

            if (site2Authenticated)
            {
                MessageBox.Show("Authentification réussie sur le site 2.");
            }
            else
            {
                MessageBox.Show("L'authentification sur le site 2 a échoué. Veuillez vérifier vos informations de connexion.");
            }

            OuvrirDeuxiemeWindowSiAuthentificationReussie(username1, password1,username2, password2);
        }

        public void OuvrirDeuxiemeWindowSiAuthentificationReussie(string username1, string password1,string username2, string password2)
        {
            // Ouvrir la fenêtre DeuxiemeWindow si les deux sites sont authentifiés
            if (site1Authenticated && site2Authenticated)
            {
                DeuxiemeWindow deuxiemeWindow = new DeuxiemeWindow(Authentification.filePath, Authentification.filePath2, username1, password1,username2,password2);
                deuxiemeWindow.Show();

                this.Close();

            }
        }
    }

}


