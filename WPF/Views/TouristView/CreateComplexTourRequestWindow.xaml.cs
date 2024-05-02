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
    /// Interaction logic for CreateComplexTourRequestWindow.xaml
    /// </summary>
    public partial class CreateComplexTourRequestWindow : Window
    {
        public CreateComplexTourRequestWindow(User loggedInUser)
        {

            InitializeComponent();
            DataContext = new CreateComplexTourRequestViewModel(loggedInUser);
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "ScrollComplexRequestsToTop":
                        ComplexRequestsSkrol.ScrollToTop();
                        break;

                    case "ScrollComplexRequestsToBottom":
                        ComplexRequestsSkrol.ScrollToBottom();
                        break;

                    case "ScrollComplexRequestsDown":
                        double newOffset = ComplexRequestsSkrol.VerticalOffset + 40; // Adjust the amount to scroll as needed
                        ComplexRequestsSkrol.ScrollToVerticalOffset(newOffset);
                        break;

                    case "ScrollComplexRequestsUp":
                        double newOffsetUp = ComplexRequestsSkrol.VerticalOffset - 40; // Adjust the amount to scroll as needed
                        ComplexRequestsSkrol.ScrollToVerticalOffset(newOffsetUp);
                        break;
                    case "CloseCreateComplexTourRequestWindowMessage":
                        this.Close();
                        break;
                }
            });
        }
    }
}
