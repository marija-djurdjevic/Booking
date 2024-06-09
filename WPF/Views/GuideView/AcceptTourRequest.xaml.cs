using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.GuidesViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace BookingApp.WPF.Views.GuideView
{
    /// <summary>
    /// Interaction logic for AcceptTourRequest.xaml
    /// </summary>
    public partial class AcceptTourRequest : Page
    {
       

        public AcceptTourRequest(int id,User user)
        {
            InitializeComponent();
            
            DataContext = new AcceptTourViewModel(id,user);
        }


        public void StartDateTimePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var calendar = ((DateTimePicker)sender).Template.FindName("PART_Calendar", (DateTimePicker)sender) as Calendar;
            if (calendar != null)
            {
                calendar.BlackoutDates.Clear();


                var freeDates = ((AcceptTourViewModel)DataContext).FreeDates;
                if (freeDates.Count > 0)
                {
                    var firstFreeDate = freeDates.OrderBy(fd => fd.Item1).First().Item1;
                    calendar.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, firstFreeDate.AddDays(-1)));
                }

        
                if (freeDates.Count > 0)
                {
                    var lastFreeDate = freeDates.OrderBy(fd => fd.Item2).Last().Item2;
                    calendar.BlackoutDates.Add(new CalendarDateRange(lastFreeDate.AddDays(1), DateTime.MaxValue));
                }

        
                for (int i = 1; i < freeDates.Count; i++)
                {
                    var previousFreeDate = freeDates[i-1].Item2;
                    var nextFreeDate = freeDates[i].Item1;
                    calendar.BlackoutDates.Add(new CalendarDateRange(previousFreeDate.AddDays(1), nextFreeDate.AddDays(-1)));
                }
            }
        }




    }
}
