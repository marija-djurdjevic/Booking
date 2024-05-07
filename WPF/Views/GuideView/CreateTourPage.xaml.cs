using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View.GuideView;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using BookingApp.WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookingApp.View
{
    public partial class CreateTourPage : Page
    {
        public CreateTourPage()
        {
            InitializeComponent();
            DataContext = new CreateTourViewModel();
           
        }
        
        private void NavigateToMainPage(object sender, MouseButtonEventArgs e)
        {
            
        }
        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e){         }

       
    }
}
