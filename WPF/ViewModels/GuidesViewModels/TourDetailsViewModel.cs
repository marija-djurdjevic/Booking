using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Input;
using BookingApp.Command;
using BookingApp.WPF.ViewModels.GuidesViewModel;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class TourDetailsViewModel : BaseViewModel
    {
        private readonly TourService tourService;
        private readonly KeyPointService keyPointService;
        private readonly LiveTourService liveTourService;
        private Tour tour;
        private List<KeyPoint> keyPoints;

        private RelayCommand nextImageCommand;
        private RelayCommand previousImageCommand;
        private int currentImageIndex;
        private BitmapImage currentImage;

        public ObservableCollection<BitmapImage> TourImages { get; set; } = new ObservableCollection<BitmapImage>();

        public Tour Tour
        {
            get { return tour; }
            set
            {
                tour = value;
                OnPropertyChanged(nameof(Tour));
            }
        }

        public List<KeyPoint> KeyPoints
        {
            get { return keyPoints; }
            set
            {
                keyPoints = value;
                OnPropertyChanged(nameof(KeyPoints));
            }
        }

        public RelayCommand NextImageCommand => nextImageCommand;
        public RelayCommand PreviousImageCommand => previousImageCommand;

        public BitmapImage CurrentImage
        {
            get { return currentImage; }
            set
            {
                currentImage = value;
                OnPropertyChanged();
            }
        }

        public TourDetailsViewModel(int tourId)
        {
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            keyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            liveTourService = new LiveTourService(Injector.CreateInstance<ILiveTourRepository>(), Injector.CreateInstance<IKeyPointRepository>());

           
            nextImageCommand = new RelayCommand(NextImage, () => CanExecuteNextImage());
            previousImageCommand = new RelayCommand(PreviousImage, () => CanExecutePreviousImage());

          
            LoadTourDetails(tourId);
            LoadTourImages();

            
            currentImageIndex = 0;  
            UpdateImage();
        }

        private void LoadTourDetails(int tourId)
        {
            Tour = tourService.GetTourById(tourId);
            KeyPoints = keyPointService.GetTourKeyPoints(tourId);
        }

        private void LoadTourImages()
        {
            foreach (var imagePath in Tour.ImagesPaths)
            {
                BitmapImage image = new BitmapImage(new Uri(ImageService.GetAbsolutePath(imagePath)));
                TourImages.Add(image);
            }
        }

        private void NextImage(object parameter)
        {
            if (currentImageIndex < TourImages.Count - 1)
            {
                currentImageIndex++;
                UpdateImage();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void PreviousImage(object parameter)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
                UpdateImage();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool CanExecuteNextImage()
        {
            return currentImageIndex < TourImages.Count - 1;
        }

        private bool CanExecutePreviousImage()
        {
            return currentImageIndex > 0;
        }

        private void UpdateImage()
        {
            if (TourImages.Count > 0)
            {
                CurrentImage = TourImages[currentImageIndex];
            }
        }
    }
}
