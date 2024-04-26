using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TouristViewModels;
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

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for TourRequestsPage.xaml
    /// </summary>
    public partial class TourRequestsPage : Page
    {
        public TourRequestsPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new TourRequestsViewModel(loggedInUser);
        }
    }
}
