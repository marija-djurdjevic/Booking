using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.Models.Enums;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using BookingApp.WPF.Views.GuideView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Command;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
   public class ComplexTourRequestViewModel:BaseViewModel
    {


        private ObservableCollection<TourRequest> tourRequests;
        private TourRequestService tourRequestService;
        private RequestStatisticService requestStatisticService;
        private string language;
        private string location;
        private RelayCommand acceptCommand;
        private RelayCommand sideMenuCommand;
        public User LoggedInUser { get; set; }

        public ComplexTourRequestViewModel(User loggedInUser)
        {
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            requestStatisticService = new RequestStatisticService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            ComplexRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllComplexRequests().Where(request => request.Status == TourRequestStatus.Pending));
            acceptCommand = new RelayCommand(ExecuteAcceptCommand);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            LoggedInUser = loggedInUser;
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


        private void ExecuteSideMenuClick()
        {

            var sideMenuPage = new SideMenuPage(LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(sideMenuPage);

        }



        public ObservableCollection<TourRequest> ComplexRequests
        {
            get { return tourRequests; }
            set { tourRequests = value; OnPropertyChanged(); }
        }

      

        public string Language
        {
            get { return language; }
            set { language = value; OnPropertyChanged(); }
        }


        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged(); }
        }


      
        private DateTime endDateTime;
        public DateTime EndDateTime
        {
            get { return endDateTime; }
            set { endDateTime = value; OnPropertyChanged(); }
        }


        private DateTime startDateTime;
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set { startDateTime = value; OnPropertyChanged(); }
        }


       

        public RelayCommand AcceptTourClickCommand
        {
            get { return acceptCommand; }
            set
            {
                if (acceptCommand != value)
                {
                    acceptCommand = value;
                    OnPropertyChanged();
                }
            }
        }




        private void ExecuteAcceptCommand(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            var acceptTour = new AcceptTourRequest(id,LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(acceptTour);

        }


        



}
}
