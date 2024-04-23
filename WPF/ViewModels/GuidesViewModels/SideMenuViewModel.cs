using BookingApp.Command;
using BookingApp.View;
using BookingApp.View.GuideView;
using BookingApp.WPF.ViewModels.GuidesViewModel;
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

        public SideMenuViewModel()
        {
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            tourStatisticCommand = new RelayCommand(TourStatisticCommand);
            
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

    }
}
