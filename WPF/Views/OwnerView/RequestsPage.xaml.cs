using BookingApp.Aplication.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BookingApp.Repositories;
using BookingApp.Serializer;
using BookingApp.WPF.ViewModel;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModel.OwnerViewModel;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {

        public RequestsPage()
        {
            InitializeComponent();
            DataContext = new RequestsViewModel();
           
        }
        private void DenyButton_Click(object sender, RoutedEventArgs e) 
        {

            if (sender is FrameworkElement element && element.DataContext is ReservationChangeRequest request)
            {
                
                //int requestId = request.Id;
                DenyRequestPage denyRequestPage = new DenyRequestPage(request);
                NavigationService.Navigate(denyRequestPage);
            }
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is RequestsViewModel viewModel && sender is FrameworkElement element && element.DataContext is ReservationChangeRequest request)
            {
                viewModel.Accept(request);
            }
        }


    }


}
