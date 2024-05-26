using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.UseCases;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repositories;
using System.Printing;
using BookingApp.Command;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class GuideAccountViewModel : BaseViewModel
    {

        public User LoggedInUser { get; set; }
        public Guide Guide { get; set; }

        private readonly GuideService guideService;
        private readonly TourService tourService;
        private readonly KeyPointService keyPointService;
        private readonly LiveTourService liveTourService;
        private readonly TourReservationService tourReservationService;
        private readonly VoucherService voucherService;
        private readonly TouristService touristService;
        private readonly TourCancellationService tourCancellationService;
        private RelayCommand quitJobCommand;


        public GuideAccountViewModel(User loggedInUser) { 
        
            LoggedInUser = loggedInUser;
            guideService = new GuideService(Injector.CreateInstance<IGuideRepository>());
            quitJobCommand = new RelayCommand(ExecuteQuitJobCommand);
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            keyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            liveTourService = new LiveTourService(Injector.CreateInstance<ILiveTourRepository>(), Injector.CreateInstance<IKeyPointRepository>());
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>(), Injector.CreateInstance<ITouristGuideNotificationRepository>(), Injector.CreateInstance<IVoucherRepository>());
            tourCancellationService = new TourCancellationService(liveTourService, tourReservationService, tourService, keyPointService, voucherService, touristService);

            LoadData();
        
        }



        public RelayCommand QuitJobCommand
        {
            get { return quitJobCommand; }
            set
            {
                if (quitJobCommand != value)
                {
                    quitJobCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public void ExecuteQuitJobCommand()
        {



        }


        public void LoadData()
        {
            Username = LoggedInUser.Username;
            Password = LoggedInUser.Password;


            bool type = guideService.IsSuperGuideById(LoggedInUser.Id);

            if (type == false)
            {
                GuideType = "Guide";
            }

            else
            {
                GuideType = "Super Guide";
            }



        }


        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string guideType;
        public string GuideType
        {
            get { return guideType; }
            set
            {
                guideType = value;
                OnPropertyChanged(nameof(GuideType));
            }
        }

        private double guideRating;
        public double GuideRating
        {
            get { return guideRating; }
            set
            {
                guideRating = value;
                OnPropertyChanged(nameof(GuideRating));
            }
        }
    }
}
