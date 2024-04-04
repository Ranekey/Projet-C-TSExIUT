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
    /// 
    
    
    public partial class MainWindow : INotifyPropertyChanged
    {
        EDT_Manager edt_manager;
        Date_Manager date_manager;
        

        public MainWindow()
        {
             
            
            DataContext = this;
            InitializeComponent();
            date_manager = new Date_Manager(ListeSemaines);
            
             

            edt_manager = new EDT_Manager(grid2, date_manager.current_week_number);



            Semaine_Correspondance.Text = edt_manager.Set_Semaine(date_manager.current_week_number);


            //SelectWeek.ItemsSource = date_manager.Get_Window_Of_Week(date_manager.week_number); 
        }


        private int _joursSemaine;
        public int joursSemaine { get { return _joursSemaine; } set {  _joursSemaine = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        
        private void ListeSemaines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int week_selected = int.Parse(e.AddedItems[0].ToString());


            date_manager.Update_Date(week_selected, ListeSemaines);
            edt_manager.peupler_edt(week_selected, grid2);
            Semaine_Correspondance.Text = edt_manager.Set_Semaine(week_selected);
            OnPropertyChanged();
        }



            
           
                
        }
        


}
