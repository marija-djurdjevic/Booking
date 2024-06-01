using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
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
        private readonly ForumCommentsViewModel viewModel;
        public ForumCommentsPage(User loggedInUser, ForumDto selectedForum)
        {
            InitializeComponent();
            SelectedForum = selectedForum;
            LoggedInUser = loggedInUser;
            viewModel = new ForumCommentsViewModel(LoggedInUser, SelectedForum);
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
        /*private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                // Pristupanje podacima komentara pomoću Tag svojstva
                ForumComment comment = button.Tag as ForumComment;

                // Provjera ako gost može biti prijavljen
                if (!comment.GuestVisited)
                {
                    // Poziv metode za prijavu komentara
                    comment.ReportsCount++;
                    forumService.UpdateComment(comment);
                }
                else
                {
                    // Prikaz poruke da gost nije prijavljen jer je već posjetio lokaciju
                    MessageBox.Show("Guest has visited the location and cannot be reported.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }*/
        /*private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            // Pristupanje podacima komentara
            ForumComment comment = (sender as FrameworkElement)?.DataContext as ForumComment;

            // Pozivanje metode u view modelu
            viewModel.ReportComment(comment);
        }*/
        // Metoda koja se poziva prilikom klika na dugme "Report"
        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            // Pristupanje podacima komentara
            ForumComment comment = (sender as FrameworkElement)?.DataContext as ForumComment;

            // Pozivanje metode u view modelu za prijavu komentara
            viewModel.ReportComment(comment);

            // Ažuriranje broja prijava prikazanog na UI-u
            comment.ReportsCount++;

            // Pronalaženje TextBlock elementa koji prikazuje broj prijava
            TextBlock reportsCountTextBlock = FindReportsCountTextBlock(sender as FrameworkElement);

            // Ažuriranje teksta u TextBlock elementu
            if (reportsCountTextBlock != null)
            {
                reportsCountTextBlock.Text = comment.ReportsCount.ToString();
            }
        }

        // Metoda za pronalaženje TextBlock elementa koji prikazuje broj prijava
        private TextBlock FindReportsCountTextBlock(FrameworkElement element)
        {
            // Rekurzivna pretraga roditeljskih elemenata kako bismo pronašli TextBlock element
            while (element != null)
            {
                if (element is TextBlock textBlock && textBlock.Name == "ReportsCountTextBlock")
                {
                    return textBlock;
                }
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
            }
            return null;
        }

    }
}
