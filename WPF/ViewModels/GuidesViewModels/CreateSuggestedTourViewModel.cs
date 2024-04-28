using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Command;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    
     internal class CreateSuggestedTourViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private int duration;
        private string city;
        private string country;
        private int maxTouristNumber;
        private TourDto tourDto;
        private BitmapImage currentImage;
        private readonly TourService tourService;
        private readonly KeyPointService keyPointService;
        private readonly TourRepository tourRepository;
        private readonly KeyPointRepository keyPointRepository;
        private readonly ImageService imageService;
        private TourRequestService tourRequestService;
        private RequestStatisticService requestStatisticService;
        private GlobalLanguagesService globalLanguagesService;
        private GlobalLocationsService globalLocationsService;
        private int currentImageIndex = 0;
        private ObservableCollection<DateTime> tourDates = new ObservableCollection<DateTime>();
        private string selectedLocation;
        private string selectedLanguage;
        private RelayCommand uploadImageCommand;
        private RelayCommand removeKeyPointCommand;
        private RelayCommand addKeyPointCommand;
        private RelayCommand createTourCommand;
        private RelayCommand nextImageCommand;
        private RelayCommand previousImageCommand;
        private RelayCommand deleteImageCommand;
        private RelayCommand addDateCommand;
        private RelayCommand removeDateCommand;
        private RelayCommand sideMenuCommand;
        public ObservableCollection<string> KeyPointNames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Langugages { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CitiesCountries { get; set; } = new ObservableCollection<string>();


        public CreateSuggestedTourViewModel()
        {
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            keyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            requestStatisticService = new RequestStatisticService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());
            MostRequestedLanguage = requestStatisticService.GetMostRequestedLanguage();
            MostRequestedLocation= requestStatisticService.GetMostRequestedLocation();
            imageService = new ImageService();
            globalLanguagesService = new GlobalLanguagesService(Injector.CreateInstance<IGlobalLanguagesRepository>());
            globalLocationsService = new GlobalLocationsService(Injector.CreateInstance<IGlobalLocationsRepository>());
            uploadImageCommand = new RelayCommand(UploadImage);
            removeKeyPointCommand = new RelayCommand(RemoveKeyPoint);
            addKeyPointCommand = new RelayCommand(AddKeyPoint);
            createTourCommand = new RelayCommand(CreateTour);
            nextImageCommand = new RelayCommand(NextImage);
            previousImageCommand = new RelayCommand(PreviousImage);
            deleteImageCommand = new RelayCommand(DeleteImage);
            addDateCommand = new RelayCommand(AddDate);
            removeDateCommand = new RelayCommand(RemoveDate);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            TourDto = new TourDto();
            SelectedLanguage = MostRequestedLanguage;
            SelectedLocation = MostRequestedLocation;
            LoadLanguages();
            LoadCitiesCountries();
        }




        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        public string City
        {
            get { return city; }
            set { city = value; OnPropertyChanged(); }
        }

        public string Country
        {
            get { return country; }
            set { country = value; OnPropertyChanged(); }
        }

        public int Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(); }
        }

        public int MaxTouristNumber
        {
            get { return maxTouristNumber; }
            set { maxTouristNumber = value; OnPropertyChanged(); }
        }




        public TourDto TourDto
        {
            get { return tourDto; }
            set { tourDto = value; OnPropertyChanged(); }
        }

        public ObservableCollection<DateTime> TourDates
        {
            get { return tourDates; }
            set { tourDates = value; OnPropertyChanged(); }
        }

        public string SelectedLocation
        {
            get { return selectedLocation; }
            set { selectedLocation = value; OnPropertyChanged(); }

        }

        public string SelectedLanguage
        {
            get { return selectedLanguage; }
            set { selectedLanguage = value; OnPropertyChanged(); }
        }

        private void LoadLanguages()
        {
            List<string> languages = globalLanguagesService.GetAll();

            if (!languages.Contains(MostRequestedLanguage))
            {
                languages.Add(MostRequestedLanguage);
            }

            foreach (var language in languages)
            {
                Langugages.Add(language);
            }
        }

        private void LoadCitiesCountries()
        {
            List<string> citiesAndCountries = globalLocationsService.GetRandomCitiesAndCountries();

            if (!citiesAndCountries.Contains(MostRequestedLocation))
            {
                citiesAndCountries.Add(MostRequestedLocation);
            }

            foreach (var cityAndCountry in citiesAndCountries)
            {
                CitiesCountries.Add(cityAndCountry);
            }
        }



        public BitmapImage CurrentImage
        {
            get { return currentImage; }
            set { currentImage = value; OnPropertyChanged(); }
        }


        public RelayCommand CreateTourCommand
        {
            get { return createTourCommand; }
            set
            {
                if (createTourCommand != value)
                {
                    createTourCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand RemoveKeyPointCommand
        {
            get { return removeKeyPointCommand; }
            set
            {
                if (removeKeyPointCommand != value)
                {
                    removeKeyPointCommand = value;
                    OnPropertyChanged();
                }
            }
        }



        public RelayCommand AddKeyPointCommand
        {
            get { return addKeyPointCommand; }
            set
            {
                if (addKeyPointCommand != value)
                {
                    addKeyPointCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand UploadImageCommand
        {
            get { return uploadImageCommand; }
            set
            {
                if (uploadImageCommand != value)
                {
                    uploadImageCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand NextImageCommand
        {
            get { return nextImageCommand; }
            set
            {
                if (nextImageCommand != value)
                {
                    nextImageCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand PreviousImageCommand
        {
            get { return previousImageCommand; }
            set
            {
                if (previousImageCommand != value)
                {
                    previousImageCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand DeleteImageCommand
        {
            get { return deleteImageCommand; }
            set
            {
                if (deleteImageCommand != value)
                {
                    deleteImageCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand RemoveDateCommand
        {
            get { return removeDateCommand; }
            set
            {
                if (removeDateCommand != value)
                {
                    removeDateCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand AddDateCommand
        {
            get { return addDateCommand; }
            set
            {
                if (addDateCommand != value)
                {
                    addDateCommand = value;
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

            var sideMenuPage = new SideMenuPage();
            GuideMainWindow.MainFrame.Navigate(sideMenuPage);

        }

        private void RemoveKeyPoint(object parameter)
        {
            string keyPointName = parameter.ToString();
            KeyPointNames.Remove(keyPointName);
        }



        private void UploadImage(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                string[] images = { selectedImagePath };
                string relativePath = "Resources\\Images\\TourImages\\";
                ImageService imageService = new ImageService();
                List<string> relativeImagePaths = imageService.SaveImages(images, relativePath);
                foreach (var relativeImagePath in relativeImagePaths)
                {
                    tourDto.ImagesPaths.Add(relativeImagePath);
                }
                currentImageIndex = tourDto.ImagesPaths.Count - 1;
                UpdateImage();
            }
        }



        private void UpdateImage()
        {
            if (tourDto.ImagesPaths.Count > 0)
            {
                string selectedImagePath = ImageService.GetAbsolutePath(tourDto.ImagesPaths[currentImageIndex]);
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.UriSource = new Uri(selectedImagePath);
                imageSource.EndInit();
                CurrentImage = imageSource;
            }
            else
            {
                CurrentImage = null;
            }
        }

        private void AddKeyPoint(object parameter)
        {
            string keyPointName = parameter.ToString();
            KeyPointNames.Add(keyPointName);


        }


        private void NextImage(object parameter)
        {
            if (currentImageIndex < tourDto.ImagesPaths.Count - 1)
            {
                currentImageIndex++;
                UpdateImage();
            }
        }

        private void PreviousImage(object parameter)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
                UpdateImage();
            }
        }

        private void DeleteImage(object parameter)
        {
            tourDto.ImagesPaths.RemoveAt(currentImageIndex);
            if (currentImageIndex >= tourDto.ImagesPaths.Count)
            {
                currentImageIndex--;
            }
            UpdateImage();
        }
        private void AddDate(object parameter)
        {
            if (parameter is DateTime selectedDateTime)
            {
                TourDates.Add(selectedDateTime);
            }
        }

        private void RemoveDate(object parameter)
        {
            if (parameter is DateTime dateToRemove)
            {
                TourDates.Remove(dateToRemove);

            }
        }

        private void CreateTour()
        {
            foreach (var startDate in TourDates)
            {
                LocationDto newLocationDto = GetLocationDto();
                TourDto newTourDto = CreateNewTourDto(newLocationDto, startDate, SelectedLanguage);
                CreateTourService createTourService = new CreateTourService(Injector.CreateInstance<ITourRepository>());
                bool success = createTourService.CreateTour(newTourDto, KeyPointNames, startDate);
                if (!success)
                {
                    return;
                }
            }
        }

        private LocationDto GetLocationDto()
        {
            string[] locationParts = selectedLocation.Split(',');
            string city = locationParts[0].Trim();
            string country = locationParts[1].Trim();
            return new LocationDto { City = city, Country = country };
        }

        private TourDto CreateNewTourDto(LocationDto locationDto, DateTime startDate, string selectedLanguage)
        {
            return new TourDto(Name, Description, selectedLanguage, MaxTouristNumber, startDate, Duration, locationDto, tourDto.ImagesPaths);
        }


        private string mostRequestedLocation;
        public string MostRequestedLocation
        {
            get { return mostRequestedLocation; }
            set { mostRequestedLocation = value; OnPropertyChanged(); }
        }

        private string mostRequestedLanguage;
        public string MostRequestedLanguage
        {
            get { return mostRequestedLanguage; }
            set { mostRequestedLanguage = value; OnPropertyChanged(); }
        }



    }
}
