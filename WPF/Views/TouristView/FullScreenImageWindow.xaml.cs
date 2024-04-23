using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for FullScreenImageWindow.xaml
    /// </summary>
    public partial class FullScreenImageWindow : Window
    {
        public FullScreenImageWindow(List<string> imagePaths, int showingIndex)
        {
            InitializeComponent();
            DataContext= new FullScreenImageViewModel(imagePaths, showingIndex);
            Messenger.Default.Register<NotificationMessage>(this, CloseWindow);
            Slika.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight-100;
        }

        private void CloseWindow(NotificationMessage message)
        {
            if (message.Notification == "FullScreenImageWindowMessage")
            {
                this.Close();
            }
        }
    }
}
