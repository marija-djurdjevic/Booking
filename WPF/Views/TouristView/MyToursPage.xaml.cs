using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using BookingApp.WPF.ViewModels.TouristViewModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for MyToursPaige.xaml
    /// </summary>
    public partial class MyToursPage : Page
    {
        public MyToursPage(User loggedInUser)
        {
            InitializeComponent();
            DataContext = new MyToursViewModel(loggedInUser);
            TabControla.Focus();
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "ChangeTabLeftMyTours":
                        if (Active.IsSelected)
                            TabControla.SelectedItem = All;
                        else if (Finished.IsSelected)
                            TabControla.SelectedItem = Active;
                        break;

                    case "ChangeTabRightMyTours":
                        if (All.IsSelected)
                            TabControla.SelectedItem = Active;
                        else if (Active.IsSelected)
                            TabControla.SelectedItem = Finished;
                        break;

                    case "ScrollMyToursToTop":
                        if (All.IsSelected)
                            AllScrol.ScrollToTop();
                        else if (Active.IsSelected)
                            ActiveScrol.ScrollToTop();
                        else if (Finished.IsSelected)
                            FinishedScrol.ScrollToTop();
                        break;

                    case "ScrollMyToursToBottom":
                        if (All.IsSelected)
                            AllScrol.ScrollToBottom();
                        else if (Active.IsSelected)
                            ActiveScrol.ScrollToBottom();
                        else if (Finished.IsSelected)
                            FinishedScrol.ScrollToBottom();
                        break;

                    case "ScrollMyToursDown":
                        double newOffset = AllScrol.VerticalOffset + 40; // Adjust the amount to scroll as needed
                        if (All.IsSelected)
                            AllScrol.ScrollToVerticalOffset(newOffset);
                        else if (Active.IsSelected)
                            ActiveScrol.ScrollToVerticalOffset(newOffset);
                        else if (Finished.IsSelected)
                            FinishedScrol.ScrollToVerticalOffset(newOffset);
                        break;

                    case "ScrollMyToursUp":
                        double newOffsetUp = AllScrol.VerticalOffset - 40; // Adjust the amount to scroll as needed
                        if (All.IsSelected)
                            AllScrol.ScrollToVerticalOffset(newOffsetUp);
                        else if (Active.IsSelected)
                            ActiveScrol.ScrollToVerticalOffset(newOffsetUp);
                        else if (Finished.IsSelected)
                            FinishedScrol.ScrollToVerticalOffset(newOffsetUp);
                        break;
                }
            });
        }
    }
}
