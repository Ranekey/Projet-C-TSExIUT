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

using static System.Net.Mime.MediaTypeNames;
using System.Windows;
using System.Windows.Media;
using static EDT_TSE_2.Class.EDT_Manager;
using EDT_TSE_2;


namespace EDT_TSE_2.Class
{
    public class EDT_Manager
    {
        public Dictionary<int, Dictionary<DayOfWeek, List<Cours>>> Dict_Semaine_Cours;
        public Dictionary<DayOfWeek, List<Cours>> Dict_Jours_Cours;
        public List<Grid> Lst_Cell_Splitter;

        public DateTime ReferenceDay;

        public void Set_Cell_Splitter(List<Grid> Lst_Cell_Splitter, Grid grid)
        {
            
            for(int i = 0; i<5; i++)
            {
                Lst_Cell_Splitter.Add(new Grid());
            }

            foreach(Grid Lst in Lst_Cell_Splitter)
            {
                 int i = 1;
                RowDefinition row = new RowDefinition();
                Lst.RowDefinitions.Add(row);
                for(int j = 0; j< grid.ColumnDefinitions.Count - 2; j++)
                {
                    ColumnDefinition col = new ColumnDefinition();
                    Lst.ColumnDefinitions.Add(col);
                }

                Grid.SetColumn(Lst,1);
                Grid.SetRow(Lst,i);
                Grid.SetColumnSpan(Lst, grid.ColumnDefinitions.Count - 2);
                grid.Children.Add(Lst);
            }

        }


        public EDT_Manager(Grid grid, int week_number)
        {

            Dict_Semaine_Cours = new Dictionary<int, Dictionary<DayOfWeek, List<Cours>>>();
            Dict_Jours_Cours = new Dictionary<DayOfWeek, List<Cours>>();
            Lst_Cell_Splitter = new List<Grid>();

            string txt_ = Text_to_string(@"C:\Users\messa\Source\Repos\Ranekey\EDT_TSE_2\EDT_TSE_2\Class\ICS_test_TSE.txt");
            string txt_2 = Text_to_string(@"C:\Users\messa\Source\Repos\Ranekey\EDT_TSE_2\EDT_TSE_2\Class\ICS_test_UJM.txt");
            ICS_Dividing_Into_Event(txt_);            
            ICS_Dividing_Into_Event(txt_2);

            Set_Cell_Splitter(Lst_Cell_Splitter, grid);
            peupler_edt(week_number, grid);

        }

        public void peupler_edt(int week_number, Grid grid)
        {
            grid.Children.Clear();
            List<string> lst_jours_semaine = ["Dimanche","Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"];

            int nbCol = grid.ColumnDefinitions.Count;
            int nbRow = grid.RowDefinitions.Count;
            

            

            for (int i = 0; i < nbCol; i = i + 2)
            {
                TextBlock txt_heure = new TextBlock() { Text = (7 + i / 2).ToString() + " h" };
                Border border = new Border();
                Grid.SetColumnSpan(border, 2);
                Grid.SetRow(border, 0);
                Grid.SetColumn(border, i);
                txt_heure.HorizontalAlignment = HorizontalAlignment.Center;
                txt_heure.VerticalAlignment = VerticalAlignment.Center;

                

                border.HorizontalAlignment = HorizontalAlignment.Stretch;
                border.VerticalAlignment = VerticalAlignment.Stretch;
                //txt_heure.HorizontalAlignment = HorizontalAlignment.Center;
                border.Background = Brushes.Gray;

                //On veut que le text créer prenne utilise tout l'espace de son parent
                border.Child = txt_heure;
                grid.Children.Add(border);
            }

            for(int i = 1; i< nbRow; i++)
            {



                TextBlock txt_jour = new TextBlock() { Text = lst_jours_semaine[i] };
                Grid.SetColumnSpan(txt_jour, 2);
                Grid.SetRow(txt_jour, i);
                Grid.SetColumn(txt_jour, 0);

                txt_jour.HorizontalAlignment = HorizontalAlignment.Stretch;
                txt_jour.VerticalAlignment = VerticalAlignment.Stretch;
                txt_jour.Background = Brushes.Gray;

                //On veut que le text créer prenne utilise tout l'espace de son parent
                grid.Children.Add(txt_jour);
            }

            if (Dict_Semaine_Cours.ContainsKey(week_number))
            {
                Clear_Child();
                foreach (var item in Dict_Semaine_Cours[week_number])
                {
                    /*
                    List<int> nbr_cours_stacker_par_jour = new List<int>(); // Param int = 0 --> Lundi etc
                    Dictionary<int, List<Cours>>  dict_coursstacker_par_jour= new Dictionary<int, List<Cours>>(); // Param int : DayOfWeek, 
                    
                    for(int i = 0; i<5; i++)
                    {
                        dict_coursstacker_par_jour.Add(i, new List<Cours>());
                    }
                    */

                    foreach (var cours in item.Value)
                    {

                        grid.Children.Add(Creation_cours(cours));
                        ReferenceDay = cours._jour;

                                              
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

        public bool Cours_overlap(Cours cours_courant, Cours cours_statique)
        {
            DateTime start1 = cours_courant._jour;
            DateTime end1 = cours_courant._jour.Add(cours_courant._duree);

            DateTime start2 = cours_statique._jour;
            DateTime end2 = cours_statique._jour.Add(cours_statique._duree);

            return start1 < end2 && start2 < end1; // retourne vrai si overlap

        }
        /*
        public void Check_Stacking(Dictionary<int, List<Cours>> dict, int dayofweek, Cours cours, List<int> nbr_cours_stacker_par_jour)
        {
            foreach(var cours_courant in dict[dayofweek])
            {
                if(Cours_overlap(cours_courant, cours))
                {
                    dict[dayofweek].
                }

            }
        }
        */
       
        public int duree_to_columnspan(TimeSpan duree)
        {
            int span = Math.Abs(duree.Hours) * 2;
            if((duree.Minutes % 60) != 0)
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

        public int jour_to_column(Cours cours)
        {

            int ind_col = (cours._jour.Hour - 7 )*2 +2;
            if ((cours._jour.Minute % 60) != 0)
            {
                if ((cours._jour.Add(cours._duree).Minute % 60) != 0) // Le cours commence avec 30min de décalage
                {
                    ind_col = ind_col + 1;
                }
                else
                {
                    ind_col = ind_col - 1;
                }
                    
                
            }
            if (cours._estTSE)
            {
                ind_col = ind_col - 2;
            }
            else
            {
              //  ind_col = 
            }

            return ind_col;

        }
        public SolidColorBrush Color_Type(Cours cours)
        {
            

            if (cours._type.Equals("TD"))
            {
                return Brushes.LavenderBlush ;
            }
            else if (cours._type.Equals("TP"))
            {
                return Brushes.LightYellow;
            }
            else if (cours._type.Equals("CM"))
            {
                return Brushes.LightGreen;
            }
            else if (cours._type.Equals("Eval"))
            {
                return Brushes.IndianRed;
            }
            else
            {
                return Brushes.BlanchedAlmond;
            }
               
        }
        
    public Border Creation_cours(Cours cours)
        {
            TextBlock txt = new TextBlock() { Text = cours._description };
            Border border = new Border();

            txt.TextAlignment = TextAlignment.Center;
            txt.TextWrapping = TextWrapping.WrapWithOverflow;
            txt.HorizontalAlignment = HorizontalAlignment.Stretch;
            txt.VerticalAlignment = VerticalAlignment.Stretch;
            
            txt.ClipToBounds = true;

            txt.Foreground = Brushes.Navy;
            //txt.Background = Color_Type(cours);

            border.Background = Color_Type(cours);
            border.Child = txt;
            border.BorderThickness = new Thickness(3);
            border.CornerRadius = new CornerRadius(10);
            


            Grid.SetColumnSpan(border, duree_to_columnspan(cours._duree));
            Grid.SetRow(border, jour_to_row(cours._jour));
            Grid.SetColumn(border, jour_to_column(cours));
          

            return border;

        }

        public void Clear_Child()
        {
            foreach(Grid cell in Lst_Cell_Splitter)
            {
                cell.Children.Clear();
            }
            
        }

        public string Set_Semaine(int week_selected)
        {
            return "Date :" +ReferenceDay.Date.ToString(" ddd/MM/yy");
        }
       


        /*
        public StackPanel Creation_cours(Cours cours)
        {
            TextBlock txt = new TextBlock() { Text = cours._description };
            StackPanel stackPanel = new StackPanel();

            txt.TextAlignment = TextAlignment.Center;
            txt.TextWrapping = TextWrapping.Wrap;
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            txt.Foreground = Brushes.Navy;

            
            Console.WriteLine(cours._jour);
            Grid.SetColumnSpan(stackPanel, duree_to_columnspan(cours._duree));
            Grid.SetRow(stackPanel, jour_to_row(cours._jour));
            Grid.SetColumn(stackPanel, jour_to_column(cours._jour));
            Grid.SetIsSharedSizeScope(stackPanel, true);
            
         
            stackPanel.Background = Brushes.Red;

            stackPanel.Children.Add(txt);


            return stackPanel;

        }
        */
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
            return (_Hour >= 8)&&(_Hour <= 20);
        }

        public void ICS_Dividing_Into_Event(string ICS_Text)
        {

            Date_Manager date_Manager = new Date_Manager();
            int week_number;
            string pattern = @"(?<=BEGIN:VEVENT)(.*?)(?=END:VEVENT)"; //(?<=BEGIN:VEVENT)(.*?)(?=END:VEVENT)
            string pattern_estTSE = @"telecom-st-etienne\.fr";
            Regex regex = new Regex(pattern_estTSE);
            bool estTSE = regex.IsMatch(ICS_Text);

            Console.WriteLine(ICS_Text);
            foreach (Match match in Regex.Matches(ICS_Text, pattern))
            {
                string str_start = Return_Match_Using_Pattern(match.Value, @"(?<=DTSTART;TZID=Europe/Paris:|DTSTART:)(.*?)(\S+)");// (?<=DTSTART;TZID=Europe/Paris:)(.*?)(?=\n)
                string str_end = Return_Match_Using_Pattern(match.Value, @"(?<=DTEND;TZID=Europe/Paris:|DTEND:)(.*?)(\S+)");// 
                string summary = Return_Match_Using_Pattern(match.Value, @"(?<=SUMMARY:)(.*?)(?=LOCATION)");
                //string infoComplet = Return_Match_Using_Pattern(match.Value, @"(?<=SUMMARY:)(.*?)(?=LOCATION)");
                //string summary = Return_Match_Using_Pattern(match.Value, @"(?<=SUMMARY:)(.*?)(?=LOCATION)");


                char lastChar = str_start[str_start.Length - 1];
                TimeSpan ajoutHeure = new TimeSpan(0,0,0); // Lorsqu'il ya un Z a la fin d'une date cela signifie qu'il faut ajouter 1h
                Console.WriteLine(lastChar);

                if (lastChar.Equals('Z'))
                {
                    str_start = str_start.Remove(str_start.Length -1 );
                    str_end  = str_end.Remove(str_end.Length -1);
                    ajoutHeure = ajoutHeure.Add(new TimeSpan(1, 0, 0));
                }

                Console.WriteLine(match.Value);
                Console.WriteLine(str_start);

                DateTime start = DateTime.ParseExact(str_start, "yyyyMMddTHHmmss", null);
                DateTime end = DateTime.ParseExact(str_end, "yyyyMMddTHHmmss", null);
                TimeSpan duree = end.Subtract(start).Duration();
                Cours cours = new Cours(start.AddHours(ajoutHeure.Hours), duree, summary, estTSE);
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

        /*
        public class EvenlyDistributedGrid : Grid
        {
            public static void AddChildren(this UIElementCollection children, UIElement child, int row, int column)
            {
                Grid parentGrid = GetParentGrid(children);

                if (parentGrid == null)
                {
                    // If the parent of the children collection is not a Grid, exit the method
                    return;
                }

                if (IsCellAvailable(parentGrid, row, column))
                {
                    // If the cell is available, add the child to the specified row and column
                    Grid.SetRow(child, row);
                    Grid.SetColumn(child, column);
                    parentGrid.Children.Add(child);
                }
                else
                {
                    // If the cell is not available, create a new row and add the child to it
                    RowDefinition newRowDefinition = new RowDefinition();
                    parentGrid.RowDefinitions.Add(newRowDefinition);
                    Grid.SetRow(child, parentGrid.RowDefinitions.Count - 1);
                    Grid.SetColumn(child, column);
                    parentGrid.Children.Add(child);
                }
            }

            private static Grid GetParentGrid(UIElementCollection children)
            {
                DependencyObject parent = children.Owner;
                while (parent != null && !(parent is Grid))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                return parent as Grid;
            }

            private static bool IsCellAvailable(Grid grid, int row, int column)
            {
                foreach (UIElement child in grid.Children)
                {
                    if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column)
                    {
                        // Cell is not available, it's already occupied
                        return false;
                    }
                }
                // Cell is available
                return true;
            }
        }
        */


        public class Cours
        {
            public string _description;
            public string _InfoComplet;
            public bool _estTSE;

            //string Matiere;
            //string Prof;
            //string Location;
            public DateTime _jour;
            public TimeSpan _duree;
            public string _type; // TD, TP, CM, Eval, Autre
            //public string _location; // Iut/ TSE
            //public string _salle;

            public Cours(DateTime jour, TimeSpan duree, string description, bool estTSE)
            {
                _jour = jour;
                _duree = duree;
                _description = description;
                _type = Type(description);
                _estTSE = estTSE;
            }            
            public string Type(string description)
            {
                string type_cours = "Autre";
                List<string> different_type = new List<string> {"TD", "CM", "TP", "Eval"};

                foreach(string type in different_type)
                {
                    string pattern = @"\b" + Regex.Escape(type) + @"\b";
                    
                    Regex regex = new Regex(pattern);

                    if (regex.IsMatch(description))
                    {
                        type_cours = type;
                    }
                }

                return type_cours;
                
            }


        }
    }
    
        /*
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
                return finalSize;

            double widthPerChild = finalSize.Height / Children.Count;

            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];
                child.Arrange(new Rect(new Point(i * widthPerChild, 0), new Size(widthPerChild, finalSize.Height)));
            }

            return finalSize;
        }
        */
    }


