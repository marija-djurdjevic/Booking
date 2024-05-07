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
    }
}
