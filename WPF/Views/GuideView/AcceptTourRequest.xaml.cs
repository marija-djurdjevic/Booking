﻿using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels.GuidesViewModels;
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

namespace BookingApp.WPF.Views.GuideView
{
    /// <summary>
    /// Interaction logic for AcceptTourRequest.xaml
    /// </summary>
    public partial class AcceptTourRequest : Page
    {
        public AcceptTourRequest(int id,User user)
        {
            InitializeComponent();
            DataContext = new AcceptTourViewModel(id,user);
        }
    }
}
