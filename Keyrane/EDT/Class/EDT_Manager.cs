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
using System.Windows.Documents;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Windows;
using System.Windows.Media;
using static EDT_TSE_2.Class.EDT_Manager;


namespace EDT_TSE_2.Class
{
    public class EDT_Manager
    {
        Dictionary<int, Dictionary<DayOfWeek, List<Cours>>> Dict_Semaine_Cours;
        Dictionary<DayOfWeek, List<Cours>> Dict_Jours_Cours;

        public EDT_Manager(Grid grid, int week_number)
        {

            Dict_Semaine_Cours = new Dictionary<int, Dictionary<DayOfWeek, List<Cours>>>();
            Dict_Jours_Cours = new Dictionary<DayOfWeek, List<Cours>>();
            string txt_ = Text_to_string(@"C:\Users\messa\Source\Repos\Ranekey\EDT_TSE_2\EDT_TSE_2\Class\ICS_test_TSE.txt");
            ICS_Dividing_Into_Event(txt_);
            peupler_edt(week_number, grid);

        }

        public void peupler_edt(int week_number, Grid grid)
        {
            grid.Children.Clear();
            List<string> lst_jours_semaine = ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"];

            int nbCol = grid.ColumnDefinitions.Count;
            int nbRow = grid.RowDefinitions.Count;



            for (int i = 2; i < nbCol; i = i + 2)
            {
                TextBlock txt_heure = new TextBlock() { Text = (7 + i / 2).ToString() + " h" };
                Grid.SetColumnSpan(txt_heure, 2);
                Grid.SetRow(txt_heure, 0);
                Grid.SetColumn(txt_heure, i);

                txt_heure.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                txt_heure.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                //On veut que le text créer prenne utilise tout l'espace de son parent
                grid.Children.Add(txt_heure);
            }

            for (int i = 1; i < nbRow; i++)
            {



                TextBlock txt_jour = new TextBlock() { Text = lst_jours_semaine[i] };
                Grid.SetColumnSpan(txt_jour, 2);
                Grid.SetRow(txt_jour, i);
                Grid.SetColumn(txt_jour, 0);
                txt_jour.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                txt_jour.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                //On veut que le text créer prenne utilise tout l'espace de son parent
                grid.Children.Add(txt_jour);
            }

            if (Dict_Semaine_Cours.ContainsKey(week_number))
            {
                foreach (var item in Dict_Semaine_Cours[week_number])
                {
                    foreach (var cours in item.Value)
                    {

                        Console.WriteLine(cours._jour);
                        Console.WriteLine(cours._duree);
                        Console.WriteLine(cours._description);
                        grid.Children.Add(Creation_cours(cours));

                    }
                }

            }

        }
        /*
        public void avoid_stacking(TextBlock cours_txtblock, Grid grid)
        {
            Grid.GetRow(grid);
        }
        */

        public int duree_to_columnspan(TimeSpan duree)
        {
            int span = Math.Abs(duree.Hours) * 2;
            if ((duree.Minutes % 60) != 0)
            {
                span++;
            }

            return span;
        }

        public int jour_to_row(DateTime jour)
        {
            int num_jour = (int)jour.DayOfWeek; // Dimanche = 0, Lundi = 1 etc...
            return num_jour;
        }

        public int jour_to_column(DateTime jour)
        {

            int ind_col = (jour.Hour - 8) * 2 + 3;
            if ((jour.Minute % 60) != 0)
            {
                ind_col = ind_col + 3;
            }

            return ind_col;

        }


        public StackPanel Creation_cours(Cours cours)
        {
            TextBlock txt = new TextBlock() { Text = cours._description };
            StackPanel stackPanel = new StackPanel();

            txt.TextAlignment = TextAlignment.Center;
            txt.TextWrapping = TextWrapping.Wrap;
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            txt.Foreground = Brushes.Navy;

            /*
            Console.WriteLine(cours._jour);
            Grid.SetColumnSpan(txt, duree_to_columnspan(cours._duree));
            Grid.SetRow(txt, jour_to_row(cours._jour));
            Grid.SetColumn(txt, jour_to_column(cours._jour));
            */
            Console.WriteLine(cours._jour);
            Grid.SetColumnSpan(stackPanel, duree_to_columnspan(cours._duree));
            Grid.SetRow(stackPanel, jour_to_row(cours._jour));
            Grid.SetColumn(stackPanel, jour_to_column(cours._jour));


            stackPanel.Background = Brushes.Red;

            stackPanel.Children.Add(txt);


            return stackPanel;


        }

        public string Return_Match_Using_Pattern(string text, string pattern)
        {
            string _pattern = @pattern;
            Match match = Regex.Match(text, _pattern);
            return match.Value;
        }
        public string Text_to_string(string path)
        {
            string _path = @path;
            string txt = File.ReadAllText(path);
            //string txt_sansretourligne = Regex.Replace(txt, @"(^\p{Zs}*\r\n){2,}", "", RegexOptions.Multiline);//@"(^\p{Zs}*\r\n){2,}", "\r\n"
            string txt_sansretourligne = txt.ReplaceLineEndings(" ");
            Console.WriteLine(txt_sansretourligne);

            return txt_sansretourligne;

        }

        public bool Cours_valide(Cours cours)
        {
            int _Hour = cours._jour.Hour;
            return (_Hour >= 8) && (_Hour <= 20);
        }

        public void ICS_Dividing_Into_Event(string ICS_Text)
        {

            Date_Manager date_Manager = new Date_Manager();
            int week_number;
            string pattern = @"(?<=BEGIN:VEVENT)(.*?)(?=END:VEVENT)"; //(?<=BEGIN:VEVENT)(.*?)(?=END:VEVENT)
            Console.WriteLine(ICS_Text);
            foreach (Match match in Regex.Matches(ICS_Text, pattern))
            {
                string str_start = Return_Match_Using_Pattern(match.Value, @"(?<=DTSTART;TZID=Europe/Paris:|DTSTART:)(.*?)(\S+)");// (?<=DTSTART;TZID=Europe/Paris:)(.*?)(?=\n)
                string str_end = Return_Match_Using_Pattern(match.Value, @"(?<=DTEND;TZID=Europe/Paris:|DTEND:)(.*?)(\S+)");// 
                string summary = Return_Match_Using_Pattern(match.Value, @"(?<=SUMMARY:)(.*?)(?=LOCATION)");


                char lastChar = str_start[str_start.Length - 1];
                TimeSpan ajoutHeure = new TimeSpan(0, 0, 0); // Lorsqu'il ya un Z a la fin d'une date cela signifie qu'il faut ajouter 1h
                Console.WriteLine(lastChar);

                if (lastChar.Equals('Z'))
                {
                    str_start = str_start.Remove(str_start.Length - 1);
                    str_end = str_end.Remove(str_end.Length - 1);
                    ajoutHeure = ajoutHeure.Add(new TimeSpan(1, 0, 0));
                }

                Console.WriteLine(match.Value);
                Console.WriteLine(str_start);

                DateTime start = DateTime.ParseExact(str_start, "yyyyMMddTHHmmss", null);
                DateTime end = DateTime.ParseExact(str_end, "yyyyMMddTHHmmss", null);
                TimeSpan duree = end.Subtract(start).Duration();
                Cours cours = new Cours(start.AddHours(ajoutHeure.Hours), duree, summary);
                Console.WriteLine(cours._description);

                week_number = date_Manager.Get_week_number(start);
                if (Cours_valide(cours))
                {
                    if (Dict_Semaine_Cours.ContainsKey(week_number))
                    {
                        if (Dict_Semaine_Cours[week_number].ContainsKey(start.DayOfWeek))
                        {
                            Dict_Semaine_Cours[week_number][start.DayOfWeek].Add(cours);
                        }
                        else
                        {
                            Dict_Semaine_Cours[week_number].Add(start.DayOfWeek, [cours]);
                        }

                    }
                    else
                    {
                        Dict_Semaine_Cours.Add(week_number, new Dictionary<DayOfWeek, List<Cours>>());
                        Dict_Semaine_Cours[week_number].Add(start.DayOfWeek, [cours]);

                    }

                }











            }
        }





        public class Cours
        {
            public string _description;
            //string Matiere;
            //string Prof;
            //string Location;
            public DateTime _jour;
            public TimeSpan _duree;

            public Cours(DateTime jour, TimeSpan duree, string description)
            {
                _jour = jour;
                _duree = duree;
                _description = description;



            }


        }

    }
}