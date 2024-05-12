using BookingApp.View.GuideView;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SideMenuPage.xaml
    /// </summary>
    public partial class SideMenuPage : Page
    {
        public SideMenuPage()
        {
            InitializeComponent();
            DataContext = new SideMenuViewModel();
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
