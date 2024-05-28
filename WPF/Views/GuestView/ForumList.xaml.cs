using BookingApp.Domain.Models;
using BookingApp.GuestView;
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
    /// Interaction logic for ForumList.xaml
    /// </summary>
    public partial class ForumList : Page
    {
        public Guest LoggedInGuest { get; set; }
        public ForumList(Guest guest)
        {
            InitializeComponent();
            LoggedInGuest = guest;
        }

        private void OpenNewForum_Click(object sender, RoutedEventArgs e)
        {
            OpenNewForum newForum = new OpenNewForum(LoggedInGuest);
            NavigationService.Navigate(newForum);
        }
    }
}
