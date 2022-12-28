using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ClosedXML.Excel.XLPredefinedFormat;
using DateTime = System.DateTime;

namespace appCore.components.UI_perso
{
    /// <summary>
    /// Logique d'interaction pour gantt.xaml
    /// </summary>
    public partial class gantt : UserControl
    {
        public gantt()
        {
            InitializeComponent();
        }

        public partial class tasks
        {
            public string name { get; set; }
            public DateTime dayStart { get; set; }
            public DateTime dayEnd { get; set; }
            public double statusPercent { get; set; }

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Now;

            //Texts jours
            for (int i = 0; i < 90; i++)
            {
                DateTime day = today.AddDays(-31 + i);

                //griser les weekends
                if (day.DayOfWeek == DayOfWeek.Sunday || day.DayOfWeek == DayOfWeek.Saturday)
                {
                    Border border = new Border();
                    border.Background = new SolidColorBrush(Colors.LightGray);
                    border.Opacity = 0.2;
                    gridTimeLine.Children.Add(border);
                    Grid.SetRow(border, 1);
                    Grid.SetColumn(border, i);
                    Grid.SetRowSpan(border, 5);

                }

                //border date du jour
                if (day == today)
                {
                    Border borderDay = new Border();
                    borderDay.Background = new SolidColorBrush(Colors.Transparent);
                    borderDay.BorderThickness = new Thickness(0, 0, 1, 0);
                    borderDay.BorderBrush = new SolidColorBrush(Colors.Red);
                    gridTimeLine.Children.Add(borderDay);
                    Grid.SetRow(borderDay, 2);
                    Grid.SetColumn(borderDay, i);
                    Grid.SetRowSpan(borderDay, 4);

                }

                // jour
                TextBlock tb = new TextBlock();
                tb.Text = day.Day.ToString();
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;            
                gridTimeLine.Children.Add(tb);
                Grid.SetRow(tb, 2);
                Grid.SetColumn(tb, i);

                // jour de la semaine
                TextBlock tbDayOfWeek = new TextBlock();
                tbDayOfWeek.Text = day.DayOfWeek.ToString().Substring(0, 1).ToUpper();
                tbDayOfWeek.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tbDayOfWeek.Foreground = new SolidColorBrush(Colors.Gray);
                gridTimeLine.Children.Add(tbDayOfWeek);
                Grid.SetRow(tbDayOfWeek, 3);
                Grid.SetColumn(tbDayOfWeek, i);

                //Afficher les semaines
                if (day.DayOfWeek == DayOfWeek.Monday)
                {
                    TextBlock semaine = new TextBlock();
                    semaine.Text = "WEEK " + CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(day, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    semaine.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    gridTimeLine.Children.Add(semaine);
                    Grid.SetRow(semaine, 1);
                    Grid.SetColumn(semaine, i);
                    Grid.SetColumnSpan(semaine, 5);
                }


                //Afficher les mois
                bool firstMonth = true;
                if (day.Day==1)
                {
                    StackPanel sp = new StackPanel();
                    sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                    TextBlock tbMonth = new TextBlock();
                    tbMonth.Text=day.ToString("MMMM");
                    tbMonth.Text = tbMonth.Text.ToString().ToUpper();  
                    tbMonth.HorizontalAlignment=System.Windows.HorizontalAlignment.Center;
                    tbMonth.VerticalAlignment=System.Windows.VerticalAlignment.Center;

                    tbMonth.Width = double.NaN;
                    if (day.Month % 2 == 0)
                    {
                        sp.Background = new SolidColorBrush(Color.FromArgb(255,74,83,107));
                        if(firstMonth)
                        {
                            borderToBeColored.Background = new SolidColorBrush(Color.FromArgb(255, 53, 60, 77));
                            firstMonth=false;
                        }
                        tbMonth.FontWeight = FontWeights.Bold;
                        tbMonth.Foreground = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        sp.Background = new SolidColorBrush(Color.FromArgb(255, 53, 60, 77));
                        if (firstMonth)
                        {
                            borderToBeColored.Background = new SolidColorBrush(Color.FromArgb(255, 74, 83, 107));
                            firstMonth = false;
                        }
                        tbMonth.FontWeight = FontWeights.Bold;
                        tbMonth.Foreground = new SolidColorBrush(Colors.White);
                    }
                    sp.Children.Add(tbMonth);
                    gridTimeLine.Children.Add(sp);
                    Grid.SetRow(sp, 0);
                    Grid.SetColumn(sp, i);
                    Grid.SetColumnSpan(sp, DateTime.DaysInMonth(day.Year, day.Month));
                }
            }
        }
        public void AddTask(tasks myTask)
        {
            //pannel gauche
            TextBlock tb = new TextBlock();
            tb.Text = myTask.name; ;
            tb.Height = 20;
            listTaches.Children.Add(tb);

            //gantt
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(20);
            gridBorders.RowDefinitions.Add(row);

            Border border = new Border();
            border.Background = new SolidColorBrush(Color.FromArgb(100,105, 70, 198));
            border.Height = 12;
            border.CornerRadius = new CornerRadius(5);
            border.Cursor = Cursors.Hand;
            

            gridBorders.Children.Add(border);
            Grid.SetRow(border, gridBorders.RowDefinitions.Count - 1);          

            if (myTask.dayStart > DateTime.Now.AddDays(-31) && myTask.dayEnd < DateTime.Now.AddDays(60))
            {
                TimeSpan difference = myTask.dayEnd - myTask.dayStart;
                double days = difference.TotalDays;
                border.Width = 20 * days;

                TimeSpan difference_ = myTask.dayStart - DateTime.Now.AddDays(-31);
                double days_ = difference_.TotalDays;
                border.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                border.Margin = new Thickness(Convert.ToInt32(days_)*20, 0, 0, 0);

                Border borderPercet = new Border();
                borderPercet.Background = new SolidColorBrush(Color.FromArgb(255, 105, 70, 198));
                borderPercet.Width = border.Width * myTask.statusPercent;
                borderPercet.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                borderPercet.CornerRadius = new CornerRadius(5);
                border.Child = borderPercet;
            }

            border.ToolTip = myTask.name + "\n" + myTask.dayStart.ToString("dd/MM/yyyy") + " -> " + myTask.dayEnd.ToString("dd/MM/yyyy");
            border.MouseDown += Border_MouseDown;
            border.MouseUp += Border_MouseUp;
            border.MouseMove += Border_MouseMove;
        }


        double mouseX = 0;  
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseX = e.GetPosition(sender as IInputElement).X;
        }
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as Border).ReleaseMouseCapture();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            // Récupérer la position de la souris
            Point cursorPosition = e.GetPosition(sender as IInputElement);
            double x = cursorPosition.X;

            Border b = sender as Border;
            if (x < 21 || x > b.ActualWidth-21)
            {
                b.Cursor = Cursors.SizeWE;
            }
            else
            {
                b.Cursor = Cursors.Hand;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Si la souris est enfoncée (clic en cours) et que la bordure est déplacée, déplacer la bordure en fonction de la position de la souris
                if (b.Cursor == Cursors.Hand)
                {
                    double newX = e.GetPosition(b.Parent as UIElement).X - mouseX;
                    b.Margin = new Thickness(newX, 0, 0, 0);
                 }
                // Si la souris est enfoncée (clic en cours) et que la bordure est agrandie ou réduite, ajuster la largeur de la bordure en fonction de la position de la souris
                else if (b.Cursor == Cursors.SizeWE)
                {
                    if(x<21)
                    {
                        double newX = e.GetPosition(b.Parent as UIElement).X - mouseX;
                        if (x < mouseX)
                        {
                            b.Margin = new Thickness(newX, 0, 0, 0);
                            b.Width = b.Width + x - mouseX;
                        }
                        else
                        {
                            b.Margin = new Thickness(newX, 0, 0, 0);
                            b.Width += x;
                        }
                       
                    }
                    else
                    {
                        if(x<mouseX)
                        {
                            b.Width = x;
                        }
                        else
                        {
                            b.Width +=1;
                        }

                    }

                }
            }
        }


    }
}
