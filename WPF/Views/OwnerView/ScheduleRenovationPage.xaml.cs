using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for ScheduleRenovationPage.xaml
    /// </summary>
    public partial class ScheduleRenovationPage : Page
    {
        private User LoggedInUser;
        private readonly PropertyRepository _propertyRepository;
        private readonly PropertyReservationRepository _propertyReservationRepository;
        private readonly RenovationRepository _renovationRepository;
        private readonly ScheduleRenovationViewModel viewModel;
        private DateRange SelectedDateRange;
        public List<string> AllPropertyNames { get; set; }
        private List<DateRange> AvailableDateRanges;


        public ScheduleRenovationPage(User loggedInUser)
        {
            InitializeComponent();
           
            LoggedInUser = loggedInUser;

            viewModel = new ScheduleRenovationViewModel(LoggedInUser);
            DataContext = viewModel;
            viewModel.GoBackRequested += ViewModel_GoBackRequested;
        }
        private void ViewModel_GoBackRequested(object sender, EventArgs e)
        {
            NavigationService?.GoBack();
        }
       
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedPropertyName = (string)PropertyComboBox.SelectedItem;
            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.MaxValue;
            int duration;
            int.TryParse(DurationTextBox.Text, out duration);

            ObservableCollection<DateRange> availableDateRanges = new ObservableCollection<DateRange>();
            availableDateRanges = viewModel.SearchAvailableDateRanges(selectedPropertyName, startDate, endDate, duration);
            DateDataGrid.ItemsSource = availableDateRanges;


        }

        private void DateDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDateRange = (DateRange)DateDataGrid.SelectedItem;
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int duration;
            int.TryParse(DurationTextBox.Text, out duration);
            if (SelectedDateRange == null)
            {
                MessageBox.Show("Please select a renovation date.");
                return;
            }

            viewModel.SubmitRenovation(LoggedInUser.Id,SelectedDateRange.PropertyId,SelectedDateRange.Start,SelectedDateRange.End,RenovationDescriptionTextBox.Text,duration);
            
        }

        private void DateDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedDateRange = (DateRange)DateDataGrid.SelectedItem;
        }
    }
}
