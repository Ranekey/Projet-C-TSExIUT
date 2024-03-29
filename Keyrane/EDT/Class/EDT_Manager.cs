using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Data;

namespace EDT_TSE_2.Class
{
    public class EDT_Manager
    {
        Dictionary<int, Dictionary<DayOfWeek, List<Cours>>> Dict_Semaine_Cours;
        Dictionary<DayOfWeek, List<Cours>> Dict_Jours_Cours;
        public EDT_Manager(Grid grid)
        {
            Dict_Semaine_Cours = new Dictionary<int, Dictionary<DayOfWeek, List<Cours>>>();
            Dict_Jours_Cours = new Dictionary<DayOfWeek, List<Cours>>();

            int nbCol = grid.ColumnDefinitions.Count;
            for (int i = 2; i < nbCol; i = i + 2)
            {
                TextBlock txt = new TextBlock() { Text = (7 + i / 2).ToString() + " h" };
                Grid.SetColumnSpan(txt, 2);
                Grid.SetRow(txt, 0);
                Grid.SetColumn(txt, i);

                txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                //On veut que le text créer prenne utilise tout l'espace de son parent
                grid.Children.Add(txt);

            }



        }

        /*
        public TextBlock Creation_cours(Date_Manager date_Manager)
        {
            int week_number = date_Manager.selected_week_number;
            


        }
        */
        public string Return_Match_Using_Pattern(string text, string pattern)
        {
            string _pattern = @pattern;
            Match match = Regex.Match(text, _pattern);
            return match.Value;
        }
        public void ICS_Dividing_Into_Event(string ICS_Text)
        {

            Date_Manager date_Manager = new Date_Manager();
            int week_number;
            string pattern = @"(?<=BEGIN:VEVENT)(.*?)(?=END:VEVENT)";
            foreach (Match match in Regex.Matches(ICS_Text, pattern))
            {
                string str_start = Return_Match_Using_Pattern(match.Value, "(?<= DTSTART;TZID=Europe/Paris:)(.*?)(?=\n)");
                string str_end = Return_Match_Using_Pattern(match.Value, "(?<= DTEND;TZID=Europe/Paris:)(.*?)(?=\n)");
                string summary = Return_Match_Using_Pattern(match.Value, "(?<= SUMMARY:)(.*?)(?=\n)");

                DateTime start = DateTime.Parse(str_start);
                DateTime end = DateTime.Parse(str_end);
                TimeSpan duree = end.Subtract(start);
                Cours cours = new Cours(start.Date, duree, summary);

                week_number = date_Manager.Get_week_number(start);

                Dict_Jours_Cours[start.DayOfWeek].Add(cours);
                Dict_Semaine_Cours[week_number] = Dict_Jours_Cours;

            }




        }





        public class Cours
        {
            string _description;
            //string Matiere;
            //string Prof;
            //string Location;
            DateTime _jour;
            TimeSpan _duree;

            public Cours(DateTime jour, TimeSpan duree, string description)
            {
                _jour = jour;
                _duree = duree;
                _description = description;



            }
        }

    }
}
