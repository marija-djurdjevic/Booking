using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.OwnerViewModels;
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
    /// Interaction logic for ForumCommentsPage.xaml
    /// </summary>
    public partial class ForumCommentsPage : Page
    {
        private User LoggedInUser;
        public ForumDto SelectedForum { get; set; }
        public ForumCommentsPage(User loggedInUser, ForumDto selectedForum)
        {
            InitializeComponent();
            SelectedForum = selectedForum;
            LoggedInUser = loggedInUser;
            DataContext = new ForumCommentsViewModel(LoggedInUser, SelectedForum);
        }
        /*private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Dobij tekst iz TextBox-a
            string newComment = CommentInput.Text;

            if (!string.IsNullOrWhiteSpace(newComment))
            {
                // Dodaj novi komentar u kolekciju komentara (pod pretpostavkom da koristiš ObservableCollection)
                var viewModel = this.DataContext as ForumCommentsViewModel;
                if (viewModel != null)
                {
                    viewModel.AddComment(newComment);
                    CommentInput.Clear(); // Očisti TextBox nakon dodavanja komentara
                }
            }
            else
            {
                MessageBox.Show("Molimo unesite validan komentar.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }*/
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ForumCommentsViewModel;
            if (viewModel != null)
            {
                viewModel.AddComment();
            }
        }
    }
}
