using BookingApp.Domain.Models;
using BookingApp.Domain.Models.Enums;
using BookingApp.Repositories;
using BookingApp.View.GuideView;
using BookingApp.WPF.ViewModels.GuidesViewModel;
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

namespace BookingApp.View
{

    public partial class LiveTourPage : Page
    {
        
        private User loggedInUser;
        public LiveTourPage(int tourId, User loggedInUser)
        {

            InitializeComponent();
            DataContext = new LiveTourViewModel(tourId,loggedInUser);
            this.loggedInUser = loggedInUser;

        }


        


       
    }
}
