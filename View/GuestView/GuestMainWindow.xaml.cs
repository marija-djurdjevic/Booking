using BookingApp.Model;
using BookingApp.Repository;
using System.Windows;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for Guest.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public User LoggedInUser { get; set; }
        public GuestMainWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            LoggedInUser = user;
            frame.Navigate(new PropertyView(LoggedInUser));
        }

        private void ViewProperties_Click(object sender, RoutedEventArgs e)
        {
            //PropertyView propertyview = new PropertyView(LoggedInUser);
            //propertyview.Owner = this;
            //propertyview.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //propertyview.ShowDialog();
            Close();
        }

        private void MenuBurger_Click(object sender, RoutedEventArgs e)
        {
            ActionFrame.Navigate(new ActionList(LoggedInUser));
        }
    }
}
