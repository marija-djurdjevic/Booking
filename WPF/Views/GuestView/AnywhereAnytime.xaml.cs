using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.GuestViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.Views.GuestView
{
    /// <summary>
    /// Interaction logic for AnywhereAnytime.xaml
    /// </summary>
    public partial class AnywhereAnytime : Page
    {
        public AnywhereAnytimeViewModel viewModel { get; set; }
        
        public AnywhereAnytime(Guest guest)
        {
            InitializeComponent();
            viewModel = new AnywhereAnytimeViewModel(guest);
            DataContext = viewModel;
        } 
        private void daterangesData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.SelectedItem != null && dataGrid.SelectedItem is DateRange)
            {
                viewModel.SetSelectedItems((DateRange)dataGrid.SelectedItem);
            }
        }

        private void DatePicker_SelectedDate1Changed(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                viewModel.SetStartDate(sender);
            }
        }

        private void DatePicker_SelectedDate2Changed(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                viewModel.SetEndDate(sender);
            }
        }


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSearchInput())
                return;
            viewModel.SearchProperties();
        }

       
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Confirm_Click())
            {
                MessageBox.Show("Successfully reserved!");

            }
            else
            {
                MessageBox.Show("Select one date range");
            }
        }

        private bool ValidateSearchInput()
        {
            if (viewModel.ValidateSearchInput() == -1)
            {
                MessageBox.Show("Enter the number of guests!");
                return false;
            }
            else if(viewModel.ValidateSearchInput() == 0)
            {
                MessageBox.Show("Enter the number of days");
                return false;
            }
            else
            {
                return true;
            }
        }

        
    }
}
