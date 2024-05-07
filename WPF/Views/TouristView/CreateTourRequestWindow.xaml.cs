using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Shapes;

namespace BookingApp.WPF.Views.TouristView
{
    /// <summary>
    /// Interaction logic for CreateTourRequestWindow.xaml
    /// </summary>
    public partial class CreateTourRequestWindow : Window
    {
        public CreateTourRequestWindow(User loggedInUser,bool IsComplex,ComplexTourRequest complexTourRequest)
        {
            InitializeComponent();
            DataContext = new CreateTourRequestViewModel(loggedInUser,IsComplex,complexTourRequest);
            Messenger.Default.Register<NotificationMessage>(this, CloseWindow);
        }

        private void CloseWindow(NotificationMessage message)
        {
            if (message.Notification == "CloseCreateTourRequestWindowMessage")
            {
                this.Close();
            }
        }
    }
}
