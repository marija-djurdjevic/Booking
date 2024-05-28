using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.GuestViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.Views.GuestView
{
    /// <summary>
    /// Interaction logic for ForumCommenting.xaml
    /// </summary>
    public partial class ForumCommenting : Page
    {
        private ForumCommentingViewModel viewModel;

        public ForumCommenting(Guest guest, KeyValuePair<Forum, Guest> selectedForum)
        {
            InitializeComponent();
            viewModel = new ForumCommentingViewModel(guest, selectedForum);
            DataContext = viewModel;
        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Send_Comment();
        }
    }
}
