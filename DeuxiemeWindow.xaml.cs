
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
using System.Windows;

namespace Application_moodle
{
    public partial class DeuxiemeWindow : Window
    {
        public DeuxiemeWindow()
        {
            InitializeComponent();
        }
        
        private void BtnClickMe_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Le bouton a été cliqué !"); 
        }
    }
}
