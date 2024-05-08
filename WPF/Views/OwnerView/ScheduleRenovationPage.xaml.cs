using BookingApp.Domain.Models;
using BookingApp.Repositories;
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

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for ScheduleRenovationPage.xaml
    /// </summary>
    public partial class ScheduleRenovationPage : Page
    {
        User LoggedInUser;
        private readonly PropertyRepository _propertyRepository;
        public List<string> AllPropertyNames { get; set; }

        public ScheduleRenovationPage(User loggedInUser)
        {
            InitializeComponent();
            LoggedInUser = loggedInUser;
            _propertyRepository = new PropertyRepository();
            Loaded += ScheduleRenovationPage_Loaded;
        }
        private void ScheduleRenovationPage_Loaded(object sender, RoutedEventArgs e)
        {
            AllPropertyNames = _propertyRepository.GetAllPropertyNames();
            DataContext = this;
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Logika za pretragu dostupnih slotova
            // Ovdje bi se trebala popuniti DataGrid sa dostupnim slotovima

            // Nakon popunjavanja DataGrid-a i pripreme podataka za prikaz, postavite vidljivost na Visible
            /*InstructionLabel.Visibility = Visibility.Visible;
            DateGrid.Visibility = Visibility.Visible;
            AdditionalInfoTextBox.Visibility = Visibility.Visible;
            SubmitButton.Visibility = Visibility.Visible;*/
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e) { }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
