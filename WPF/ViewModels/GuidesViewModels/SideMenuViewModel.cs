using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.View;
using BookingApp.View.GuideView;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using BookingApp.WPF.Views.GuideView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class SideMenuViewModel:BaseViewModel
    {
        private RelayCommand sideMenuCommand;
        private RelayCommand tourStatisticCommand;
        private RelayCommand tourRequestCommand;
        private RelayCommand tourSuggestedCommand;
        private RelayCommand tourRequestStatisticCommand;
        private RelayCommand homeCommand;

        public SideMenuViewModel()
        {
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            tourStatisticCommand = new RelayCommand(TourStatisticCommand);
            tourRequestCommand = new RelayCommand(TourRequestCommand);
            tourSuggestedCommand = new RelayCommand(ExecuteSuggestedTourCommand);
            tourRequestStatisticCommand = new RelayCommand(ExecuteTourRequestStatisticCpmmand);
            homeCommand = new RelayCommand(ExecuteHomeCommand);
        }

        

        public RelayCommand SideManuCommand
        {
            get { return sideMenuCommand; }
            set
            {
                if (sideMenuCommand != value)
                {
                    sideMenuCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand HomeCommand
        {
            get { return homeCommand; }
            set
            {
                if (homeCommand != value)
                {
                    homeCommand = value;
                    OnPropertyChanged();
                }
            }
        }




        public RelayCommand NavigateToSuggestedTourCommand
        {
            get { return tourSuggestedCommand; }
            set
            {
                if (tourSuggestedCommand != value)
                {
                    tourSuggestedCommand = value;
                    OnPropertyChanged();
                }
            }
        }





        public RelayCommand NavigateToTourRequestsStatisticCommand
        {
            get { return tourRequestStatisticCommand; }
            set
            {
                if (tourRequestStatisticCommand != value)
                {
                    tourRequestStatisticCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ExecuteSuggestedTourCommand()
        {
            var suggestedTour= new CreateSuggestedTour();
            GuideMainWindow.MainFrame.Navigate(suggestedTour);

        }

        public RelayCommand NavigateToTourStatisticCommand
        {
            get { return tourStatisticCommand; }
            set
            {
                if (tourStatisticCommand != value)
                {
                    tourStatisticCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand NavigateToTourRequestCommand
        {
            get { return tourRequestCommand; }
            set
            {
                if (tourRequestCommand != value)
                {
                    tourRequestCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        private void ExecuteSideMenuClick()
        {

            if (GuideMainWindow.MainFrame.CanGoBack)
            {
                GuideMainWindow.MainFrame.GoBack();
            }

        }


        private void TourStatisticCommand()
        {

            var tourStatistic = new TourStatistic();
            GuideMainWindow.MainFrame.Navigate(tourStatistic);
        }



        private void TourRequestCommand()
        {
            var tourRequests = new TourRequests();
            GuideMainWindow.MainFrame.Navigate(tourRequests);
        }

        private void ExecuteTourRequestStatisticCpmmand()
        {
            var tourRequestStatistics = new TourRequestsStatistic();
            GuideMainWindow.MainFrame.Navigate(tourRequestStatistics);

        }


        private void ExecuteHomeCommand()
        {
            var homePage=new GuideMainPage();
            GuideMainWindow.MainFrame.Navigate(homePage);
        }



    }
}
