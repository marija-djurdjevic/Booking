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
    /// Interaction logic for GeneralStatistics.xaml
    /// </summary>
    public partial class GeneralStatistics : Page
    {
        public GeneralStatistics(string language,string location,User user)
        {
            InitializeComponent();
            DataContext=new GeneralStatisticsViewModel(language,location,user);
        }
    }
}
