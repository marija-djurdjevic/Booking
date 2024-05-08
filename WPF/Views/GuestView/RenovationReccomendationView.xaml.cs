using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels.GuestViewModel;
using BookingApp.WPF.ViewModels.GuestViewModels;
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

namespace BookingApp.WPF.Views.GuestView
{
    /// <summary>
    /// Interaction logic for RenovationReccomendation.xaml
    /// </summary>
    public partial class RenovationReccomendationView : Page
    {
        private RenovationReccomendationViewModel viewModel;

        public RenovationReccomendationView()
        {
            InitializeComponent();
            viewModel = new RenovationReccomendationViewModel();
            DataContext = viewModel;
        }

        private void SaveReccomendation_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveReccomendation();
            NavigationService?.GoBack();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.SetUrgencyLevel(sender);
        }
    }
}
