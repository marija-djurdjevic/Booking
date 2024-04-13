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

        public LiveTourViewModel(int tourId)
        {
            this.tourId = tourId;
            liveTourService = new LiveTourService();
            keyPointService = new KeyPointService();
            tourReservationService = new TourReservationService();
            Tourists = new ObservableCollection<TourReservation>(liveTourService.GetTouristsByTourId(tourId));
            KeyPoints = new ObservableCollection<KeyPoint>(liveTourService.GetTourKeyPoints(tourId));
            finishTourClickCommand = new RelayCommand(ExecuteFinishTourClick);
            addTouristClickCommand = new RelayCommand(ExecuteAddTouristClick);
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



        private bool isAllKeyPointsChecked;
        public bool IsAllKeyPointsChecked
        {
            get { return isAllKeyPointsChecked; }
            set
            {
                if (isAllKeyPointsChecked != value)
                {
                    isAllKeyPointsChecked = value;
                    OnPropertyChanged();
                }
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



        private void ExecuteFinishTourClick()
        {

            var touristsOnTour = tourReservationService.GetByTourId(tourId);
            foreach (var tourist in touristsOnTour)
            {
                tourist.IsOnTour = false;
            }
            tourReservationService.SaveChanges();
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