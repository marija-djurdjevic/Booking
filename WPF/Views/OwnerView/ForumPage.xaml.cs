﻿using BookingApp.Domain.Models;
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
    }
}
