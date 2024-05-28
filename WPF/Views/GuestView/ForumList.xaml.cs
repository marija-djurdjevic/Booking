using BookingApp.Domain.Models;
using BookingApp.GuestView;
using BookingApp.Repositories;
using BookingApp.WPF.ViewModels.GuestViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.Views.GuestView
{
    /// <summary>
    /// Interaction logic for ForumList.xaml
    /// </summary>
    public partial class ForumList : Page
    {
        private ForumListViewModel viewModel;
        public ForumList(Guest guest)
        {
            InitializeComponent();
            viewModel = new ForumListViewModel(guest);
            DataContext = viewModel;
        }
        private void OpenNewForum_Click(object sender, RoutedEventArgs e)
        {
            OpenNewForum newForum = viewModel.OpenForum();
            NavigationService.Navigate(newForum);
        }

        
        private void forumData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (forumsData.SelectedItem != null && forumsData.SelectedItem is KeyValuePair<Forum, Guest>)
            {
                viewModel.SelectedForum = (KeyValuePair<Forum, Guest>)forumsData.SelectedItem;
                ForumCommenting forumCommenting = viewModel.ForumCommentingView();
                NavigationService.Navigate(forumCommenting);
            }
        }
        
    }
}
