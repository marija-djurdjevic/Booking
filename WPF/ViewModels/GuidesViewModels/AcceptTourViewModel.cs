using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.Models.Enums;
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
using System.Windows;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class AcceptTourViewModel : BaseViewModel
    {
        private int id;
        private  ObservableCollection<(DateTime, DateTime)> freeDates = new ObservableCollection<(DateTime, DateTime)>();
        private  readonly TourRequestService tourRequestService;
        private RequestStatisticService requestStatisticService;
        private TouristGuideNotificationService notificationService;
        private  ObservableCollection<(DateTime, DateTime)> bookedDates;
        private (DateTime, DateTime) touristsDates;
        private RelayCommand sideMenuCommand;
        private TourRequest selectedTour;
        public ObservableCollection<(DateTime, DateTime)> BookedDates { get; set; }
        public (DateTime, DateTime) TouristsDates { get; set; }

        private RelayCommand acceptTourCommand;
        public User LoggedInUser { get; set; }
        public AcceptTourViewModel(int id, User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            this.id = id;
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            requestStatisticService = new RequestStatisticService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            notificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());
            SelectedTour = tourRequestService.GetRequestById(id);
            LoadBookedDates();
            TouristsDates = tourRequestService.GetDateSlotById(id);
            FreeDates = new ObservableCollection<(DateTime, DateTime)>(requestStatisticService.CalculateFreeDates(BookedDates.ToList(), TouristsDates, tourRequestService.GetAllAcceptedDates(loggedInUser.Id)));
            acceptTourCommand = new RelayCommand(ExecuteAcceptTourCommand);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
         
        }


        public TourRequest SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged();
                }
            }
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


        public ObservableCollection<(DateTime, DateTime)> FreeDates
        { 
             get { return freeDates; }
             set
             {
                freeDates = value;
                OnPropertyChanged();
             }
        }



        private void LoadBookedDates()
        {
            BookedDates = new ObservableCollection<(DateTime, DateTime)>(tourRequestService.GetUpcomingToursDates(LoggedInUser.Id));

        }


        public RelayCommand AcceptTourCommand
        {
            get { return acceptTourCommand; }
            set
            {
                if (acceptTourCommand != value)
                {
                    acceptTourCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        private DateTime selectedDateTime;
        public DateTime SelectedDateTime
        {
            get { return selectedDateTime; }
            set
            {
                selectedDateTime = value;
                OnPropertyChanged();
            }
        }


        private bool isAccepted;

        public bool IsAccepted
        {
            get { return isAccepted; }
            set
            {
                isAccepted = value;
                OnPropertyChanged();
            }
        }

        private void ExecuteAcceptTourCommand()
        {
            if (FreeDates.Any(dateRange => SelectedDateTime >= dateRange.Item1 && SelectedDateTime <= dateRange.Item2))
            {

                if (SelectedTour.ComplexId != -1)
                {
                    var acceptedToursByGuide = tourRequestService.GetAllRequests()
                            .Where(tr => tr.ComplexId == SelectedTour.ComplexId && tr.Status == TourRequestStatus.Accepted && tr.GuideId == LoggedInUser.Id)
                            .ToList();

                    if (acceptedToursByGuide.Any())
                    {
                        MessageBox.Show("You have already accepted a part of this complex tour request and cannot accept another part.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }



                tourRequestService.UpdateRequestById(id,SelectedDateTime,LoggedInUser);
                if (!IsAccepted)
                {
                    tourRequestService.UpdateRequestById(id, SelectedDateTime, LoggedInUser);
                    IsAccepted = true;
                    IGuideRepository guideRepository = Injector.CreateInstance<IGuideRepository>();
                    var Guide = guideRepository.GetById(LoggedInUser.Id);
                    TouristGuideNotification touristGuideNotification = new TouristGuideNotification(SelectedTour.TouristId, Guide.Id, SelectedTour.Id, DateTime.Now, Domain.Models.Enums.NotificationType.RequestAccepted, Guide.FirstName+' '+Guide.LastName, SelectedDateTime);
                    notificationService.Save(touristGuideNotification);
                    MessageBox.Show("Tour request successfully accepted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (SelectedTour.ComplexId != -1)
                    {

                        ComplexTourRequests complex = new ComplexTourRequests(LoggedInUser);
                        GuideMainWindow.MainFrame.Navigate(complex);
                    }


                    else
                    {
                        TourRequests requests = new TourRequests(LoggedInUser);
                        GuideMainWindow.MainFrame.Navigate(requests);

                    }
                }
                else
                {
                    MessageBox.Show("Tour request has already been accepted and cannot be accepted again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    

                }
            }
            else
            {
                MessageBox.Show("Selected date is not available for booking.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


      



    }
}
