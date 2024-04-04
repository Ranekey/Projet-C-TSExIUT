using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Media;

namespace EDT_TSE_2.Class
{
    public class Date_Manager : INotifyPropertyChanged
    {
        CultureInfo cInfo;

        //public DateTime Week_Of_Year;
        CalendarWeekRule myCWR;

        public DateTime Current_Day;
        public DateTime Selected_Day; // retourne le lundi de la semaine selectionner
        public DateTime Selected_Week;

        public int current_week_number;
        public int selected_week_number;

        public Date_Manager(ComboBox ListeSemaine)
        {
            cInfo = new CultureInfo("fr-FR");
            myCWR = cInfo.DateTimeFormat.CalendarWeekRule; // Indique comment avoir la 1er semaine de l'année
            Current_Day = DateTime.Now;
            current_week_number = Get_week_number(Current_Day);
            ListeSemaine.Text = current_week_number.ToString();
            ListeSemaine.ItemsSource = Get_Window_Of_Week(current_week_number);
            
           

        }
        public Date_Manager()
          {
            cInfo = new CultureInfo("fr-FR");
            myCWR = cInfo.DateTimeFormat.CalendarWeekRule; // Indique comment avoir la 1er semaine de l'année
            Current_Day = DateTime.Now;
            current_week_number = Get_week_number(Current_Day);
         }


        // Source : https://stackoverflow.com/questions/11154673/get-the-correct-week-number-of-a-given-date
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public int Get_week_number(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
           
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public List<int> Get_Window_Of_Week(int week_number)
        {

            List<int> window_Of_Week = new List<int>();
            window_Of_Week.Add(week_number);
            for (int i = 0; i < 10; i++)
            {
                window_Of_Week.Insert(0, Math.Abs(week_number - (i + 1)) % 52);
                window_Of_Week.Add((week_number + i + 1) % 52);
            }
            return window_Of_Week;
        }


        public DateTime week_to_date(int week_number)
        {
            int number_hours = week_number * 168; // Dans une semaine il y a 168 heures
 
            DateTime start_year = new DateTime(Selected_Day.Year, 1, 1);

            return start_year.AddHours(number_hours);

        }
        public DateTime Get_Date()
        {
            return Selected_Day.Date;

        }

        

        public void Update_Date(int week_number, ComboBox ListeSemaine)
        {
            Selected_Day = week_to_date(week_number);
            selected_week_number = week_number;
            ListeSemaine.Text = selected_week_number.ToString();
            ListeSemaine.ItemsSource = Get_Window_Of_Week(selected_week_number);
            OnPropertyChanged();

        }




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
