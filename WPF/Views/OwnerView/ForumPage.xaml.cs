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
    /// Interaction logic for ForumPage.xaml
    /// </summary>
    public partial class ForumPage : Page
    {
        private User LoggedInUser;
        public ForumPage(User loggedInUser)
        {
            InitializeComponent();
            LoggedInUser = loggedInUser;
            DataContext = new ForumViewModel(LoggedInUser);
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                if (listBox.SelectedItem is ForumDto selectedForum)
                {
                    this.NavigationService.Navigate(new ForumCommentsPage(LoggedInUser, selectedForum));
                }
            }
        }
        /*private void ForumItem_Click(object sender, MouseButtonEventArgs e)
        {
            // Dobijamo odabrani forum
            var selectedForum = (sender as ListBox).SelectedItem as Forum;

            // Proveravamo da li je forum odabran
            if (selectedForum != null)
            {
                // Kreiramo novu instancu ForumDetailsPage i prosleđujemo odabrani forum kao parametar
                ForumCommentsPage forumDetailsPage = new ForumCommentsPage(LoggedInUser, selectedForum);

                this.NavigationService.Navigate(new ForumCommentsPage(LoggedInUser, selectedForum));
            }
        }*/
    }

}
