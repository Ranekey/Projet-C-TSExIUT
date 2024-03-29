using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projet_IUTxTSE
{
    /// <summary>
    /// Logique d'interaction pour notes.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        public NotesWindow(List<Note2> notes)
        {
            InitializeComponent();
            listView.ItemsSource = notes; // Afficher les données dans ListView
        }
    }
}
