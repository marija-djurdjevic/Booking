using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.GuestViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.Views.GuestView
{
    /// <summary>
    /// Interaction logic for OpenNewForum.xaml
    /// </summary>
    public partial class OpenNewForum : Page
    {

        private ForumViewModel viewModel;
        public OpenNewForum(Guest guest)
        {
            InitializeComponent();
            viewModel = new ForumViewModel(guest);
            DataContext = viewModel;
        }

        public void Post_Click(object sender, RoutedEventArgs e)
        {
            ForumList forumList = viewModel.PostForum();
            NavigationService.Navigate(forumList);
        }
    }
}
