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
    
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ForumCommentsViewModel;
            if (viewModel != null)
            {
                viewModel.AddComment();
            }
        }
        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            ForumComment comment = (sender as FrameworkElement)?.DataContext as ForumComment;

            viewModel.ReportComment(comment);

            comment.ReportsCount++;

            TextBlock reportsCountTextBlock = FindReportsCountTextBlock(sender as FrameworkElement);

            if (reportsCountTextBlock != null)
            {
                reportsCountTextBlock.Text = comment.ReportsCount.ToString();
            }
        }

        private TextBlock FindReportsCountTextBlock(FrameworkElement element)
        {
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
