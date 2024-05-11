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
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.OwnerViewModels;

namespace BookingApp.WPF.Views.OwnerView
{
    /// <summary>
    /// Interaction logic for AnalyticsPage.xaml
    /// </summary>
    public partial class AnalyticsPage : Page
    {
        private User LoggedInUser;
        public AnalyticsPage(User loggedInUser)
        {
            InitializeComponent();
            LoggedInUser = loggedInUser;
            DataContext = new AnalyticsViewModel(LoggedInUser);
    }
        
        
    }
}
