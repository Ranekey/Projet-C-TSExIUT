using EDT_TSE_2.Class;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


namespace EDT_TSE_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        EDT_Manager edt_manager;
        Date_Manager date_manager;
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            edt_manager = new EDT_Manager(grid2);
            date_manager = new Date_Manager(ListeSemaines);

            //SelectWeek.ItemsSource = date_manager.Get_Window_Of_Week(date_manager.week_number); 
        }


        private int _joursSemaine;
        public int joursSemaine { get { return _joursSemaine; } set {  _joursSemaine = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ListeSemaines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int week_selected =  int.Parse(e.AddedItems[0].ToString());
            date_manager.Update_Date(week_selected, ListeSemaines);
            OnPropertyChanged();
        }
    }
}