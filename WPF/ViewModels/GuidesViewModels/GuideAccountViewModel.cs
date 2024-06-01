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
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using BookingApp.View;
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
        private LiveTourRepository liveTourRepository;
        private readonly TourCancellationService tourCancellationService;
        private RelayCommand quitJobCommand;
        private ObservableCollection<Tour> upcomingTours;
        private ObservableCollection<Tour> finishedTours;
        private readonly TouristExperienceService touristExperienceService;
        private string language;



        private RelayCommand sideMenuCommand;


        public string Language
        {
            get { return language; }
            set { language = value; OnPropertyChanged(); }
        }

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }

        private string[] _labels;
        public string[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }


        public ObservableCollection<double> AverageGrade { get; set; }
        public ObservableCollection<string> Year { get; set; }
        public GuideAccountViewModel(User loggedInUser) {

            
            LoggedInUser = loggedInUser;
            guideService = new GuideService(Injector.CreateInstance<IGuideRepository>());
            quitJobCommand = new RelayCommand(ExecuteQuitJobCommand);
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            keyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            liveTourService = new LiveTourService(Injector.CreateInstance<ILiveTourRepository>(), Injector.CreateInstance<IKeyPointRepository>());
            liveTourRepository = new LiveTourRepository();
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>(), Injector.CreateInstance<ITouristGuideNotificationRepository>(), Injector.CreateInstance<IVoucherRepository>());
            tourCancellationService = new TourCancellationService(liveTourService, tourReservationService, tourService, keyPointService, voucherService, touristService);
            touristExperienceService = new TouristExperienceService(Injector.CreateInstance<ITouristExperienceRepository>());
            AverageGrade = new ObservableCollection<double>();
            Year = new ObservableCollection<string>();
            SeriesCollection = new SeriesCollection();
            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            LoadData();
            GuideRating = Math.Round(CountAverage(), 2);
            LoadChartData();
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);

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

        private void LoadChartData()
        {
            var random = new Random();
            var series = new LineSeries
            {
                Title = "Average Grade",
                Values = new ChartValues<double>()
            };

            for (var i = 0; i < 12; i++)
            {
                var totalRating = 0.0;
                

                var oneYearAgo = DateTime.Today.AddYears(-1);


                var filteredTours = FinishedTours
                    .Where(tour => tour.StartDateTime >= oneYearAgo)
                    .OrderBy(tour => tour.StartDateTime)
                    .ToList();


                int total =filteredTours.Count();
                var totalTours = 0;

                    for (var j = 0; j < total; j++)
                {
                    var touristExperiences = touristExperienceService.GetTouristExperiencesForTour(filteredTours[j].Id);
                    foreach (var review in touristExperiences)
                    {
                        double srvr = review.GuideLanguageRating + review.GuideKnowledgeRating + review.TourInterestingesRating;
                        totalRating += srvr / 3;
                        totalTours++;
                    }

                    //var fakeRating = random.Next(1, 6);
                    //totalRating += fakeRating;
                }

                var averageRating = totalRating / totalTours;
                series.Values.Add(Math.Round(averageRating, 2));
            }

            SeriesCollection.Add(series);
        }





        public String Langugage { get; set; }

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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to quit your job?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                UpcomingTours = new ObservableCollection<Tour>(tourService.GetUpcomingTours().Where(tour => tour.GuideId == LoggedInUser.Id));
                foreach (var tour in UpcomingTours)
                {
                    var tourKeyPoints = keyPointService.GetTourKeyPoints(tour.Id);
                    var tourReservation = tourReservationService.GetByTourId(tour.Id);
                    tourCancellationService.QuitJob(tour, tourKeyPoints, tourReservation, LoggedInUser.Id);

                }
                guideService.RemoveGuide(LoggedInUser.Id);
            }

                

        }


        public ObservableCollection<Tour> UpcomingTours
        {
            get { return upcomingTours; }
            set { upcomingTours = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tour> FinishedTours
        {
            get { return finishedTours; }
            set { finishedTours = value; OnPropertyChanged(); }
        }

        public void LoadData()
        {
            bool superGuide = false;

            var finishedLiveTours = liveTourRepository.GetFinishedTours();
            FinishedTours = new ObservableCollection<Tour>(finishedLiveTours.Select(tour => tourService.GetTourById(tour.TourId)).Where(tour => tour.GuideId == LoggedInUser.Id));
            var oneYearAgo = DateTime.Today.AddYears(-1);
            var oneYearAfter = DateTime.Today.AddDays(1);

            var filterTours = FinishedTours
                .Where(tour => tour.StartDateTime >= oneYearAgo)
                .GroupBy(tour => tour.Language)  // Grupišete ture po jeziku
                .Where(group => group.Count() >= 20)  // Birate samo grupe koje imaju 20 ili više tura
                .SelectMany(group => group)  // Spajate sve ture iz svake grupe nazad u jednu listu
                .ToList();



            var languages = FinishedTours
                .Where(tour => tour.StartDateTime >= oneYearAgo)
                .GroupBy(tour => tour.Language)  // Grupišete ture po jeziku
                .Where(group => group.Count() >= 20)  // Birate samo grupe koje imaju 20 ili više tura
                .Select(group => group.Key)  // Izdvajate ključeve grupa, koji predstavljaju jezike
                .ToList();



            var toursWithHighAverageRating = new List<Tour>();
           
            foreach (var language in languages)
            {
                var toursForLanguage = filterTours
                    .Where(tour =>  tour.Language == language)
                    .ToList();


                double totalRating = 0.0;
                var totalTours = 0;
                foreach (var tour in toursForLanguage)
                {
                  
                    var touristExperiences = touristExperienceService.GetTouristExperiencesForTour(tour.Id);
                    foreach (var review in touristExperiences)
                    {
                        double srvr = review.GuideLanguageRating + review.GuideKnowledgeRating + review.TourInterestingesRating;
                        totalRating += srvr/3;
                        totalTours++;
                    }

                   
                }
                var averageRating = totalRating / totalTours;
                if (averageRating >= 4)
                {
                    toursWithHighAverageRating = toursForLanguage;
                    Language = language;
                    superGuide = true;
                    guideService.setStatus(LoggedInUser.Id,superGuide);
                    guideService.IsSuperGuideById(LoggedInUser.Id);

                    break;
                }


            }


            Username = LoggedInUser.Username;
            Password = LoggedInUser.Password;


           

            if (superGuide == false)
            {
                GuideType = "Ordinary Guide";
                guideService.setStatus(LoggedInUser.Id, superGuide);

            }

            else
            {
                GuideType = "Super Guide";
                guideService.setStatus(LoggedInUser.Id, superGuide);
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






        public double CountAverage()
        {
            double totalRating = 0;
            var totalTours = 0;

            foreach (var tour in FinishedTours)
            {

                var touristExperiences = touristExperienceService.GetTouristExperiencesForTour(tour.Id);
                foreach (var review in touristExperiences)
                {
                    double srvr = review.GuideLanguageRating + review.GuideKnowledgeRating + review.TourInterestingesRating;
                    totalRating += srvr / 3;
                    totalTours++;
                }


            }
            var averageRating = totalRating / totalTours;
            return averageRating;
        }





    }
}
