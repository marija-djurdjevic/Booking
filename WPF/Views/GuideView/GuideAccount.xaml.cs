using BookingApp.Domain.Models;
using BookingApp.View;
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

namespace BookingApp.WPF.Views.GuideView
{
    /// <summary>
    /// Interaction logic for GuideAccount.xaml
    /// </summary>
    public partial class GuideAccount : Page
    {
        public GuideAccount(User user)
        {
            InitializeComponent();
            var viewModel = new GuideAccountViewModel(user);
            viewModel.LogOutAction = LogOutClick; // Postavljanje delegata
            DataContext = viewModel;

        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {

            SignInForm signInWindow = new SignInForm();
            signInWindow.Show();

            Window mainWindow = Window.GetWindow(this);
            mainWindow.Close();



        }

    }
}
