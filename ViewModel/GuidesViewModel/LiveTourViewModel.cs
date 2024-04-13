using BookingApp.Command;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.GuideView;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BookingApp.ViewModel.GuidesViewModel
{
    public class LiveTourViewModel : BaseViewModel
    {
        private int tourId;
        private readonly LiveTourService liveTourService;
        private readonly TourReservationService tourReservationService;
        private readonly KeyPointService keyPointService;
        private ObservableCollection<TourReservation> tourists;
        private ObservableCollection<KeyPoint> keyPoints;
        private Tour selectedTour;
        private RelayCommand finishTourClickCommand;
        private RelayCommand addTouristClickCommand;
        private RelayCommand uncheckCommand;
        private RelayCommand checkCommand;
        private KeyPointRepository keyPointRepository;
        private LiveTourRepository liveTourRepository;
        private LiveTour liveTour;

        public LiveTourViewModel(int tourId)
        {
            this.tourId = tourId;
           
            keyPointRepository = new KeyPointRepository();
            liveTourRepository= new LiveTourRepository();
            liveTour = liveTourRepository.GetLiveTourById(tourId);
            liveTourService = new LiveTourService();
            keyPointService = new KeyPointService();
            tourReservationService = new TourReservationService();
            Tourists = new ObservableCollection<TourReservation>(liveTourService.GetTouristsByTourId(tourId));
            KeyPoints = new ObservableCollection<KeyPoint>(liveTourService.GetTourKeyPoints(tourId));
            finishTourClickCommand = new RelayCommand(ExecuteFinishTourClick);
            addTouristClickCommand = new RelayCommand(ExecuteAddTouristClick);
            checkCommand = new RelayCommand(Check);
        }


        public ObservableCollection<TourReservation> Tourists
        {
            get { return tourists; }
            set
            {
                tourists = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<KeyPoint> KeyPoints
        {
            get { return keyPoints; }
            set
            {
                keyPoints = value;
                OnPropertyChanged();
            }
        }


        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged();
            }
        }



        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                
                    isChecked = value;
                    OnPropertyChanged();
                
            }
        }





        public RelayCommand FinishTourClickCommand
        {

            get { return finishTourClickCommand; }
            set
            {
                if (finishTourClickCommand != value)
                {
                    finishTourClickCommand = value;
                    OnPropertyChanged();
                }
            }

        }

        public RelayCommand AddTouristClickCommand
        {

            get { return addTouristClickCommand; }
            set
            {
                if (addTouristClickCommand != value)
                {
                    addTouristClickCommand = value;
                    OnPropertyChanged();
                }
            }

        }



        public RelayCommand CheckCommand
        {

            get { return checkCommand; }
            set
            {
                if (checkCommand != value)
                {
                    checkCommand = value;
                    OnPropertyChanged();
                }
            }

        }




        private bool AreAllKeyPointsChecked(List<KeyPoint> keyPoints)
        {
            int checkedKeyPointsCount = keyPoints.Count(kp => kp.IsChecked);
            return checkedKeyPointsCount == keyPoints.Count;
        }


        private void Check(object parameter)
        {
            KeyPoint keyPoint = parameter as KeyPoint;
            keyPoint.IsChecked = true;
            keyPointRepository.Update(keyPoint);
            liveTourRepository.AddOrUpdateLiveTour(liveTour);
            liveTourRepository.Update(liveTour);
            var keypoints = keyPointRepository.GetTourKeyPoints(tourId);
            if (AreAllKeyPointsChecked(keypoints))
            {
                ExecuteFinishTourClick();
            }


        }

        

        private void ExecuteFinishTourClick()
        {

            var touristsOnTour = tourReservationService.GetByTourId(tourId);
            foreach (var tourist in touristsOnTour)
            {
                tourist.IsOnTour = false;
                tourReservationService.UpdateReservation(tourist);
            }

            var keyPoints = keyPointRepository.GetTourKeyPoints(tourId);
            foreach (var keyPoint in keyPoints)
            {
                keyPoint.IsChecked = false;
                keyPointRepository.Update(keyPoint);    
            }
            
            liveTourService.FinishTour(tourId);

        }


        private TourReservation selectedTourist;
        public TourReservation SelectedTourist
        {
            get { return selectedTourist; }
            set
            {
                selectedTourist = value;
                OnPropertyChanged();
            }
        }


        private void ExecuteAddTouristClick()
        {
            if (SelectedTourist != null)
            {
                var keyPoint=keyPointService.GetLastActiveKeyPoint();
                selectedTourist.JoinedKeyPoint = keyPoint;
                selectedTourist.IsOnTour = true;
                tourReservationService.UpdateReservation(selectedTourist);
                MessageBox.Show($"Tourist {selectedTourist.TouristFirstName} added to tour at {keyPoint.Name}.");
            }
            else
            {
                MessageBox.Show("Please select a tourist first.");
            }
        }





    }
}