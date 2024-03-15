using System.Collections.ObjectModel;
using System.Windows;
using BookingApp.Repository;
using BookingApp.Model;
using System.Linq;
using System.Windows.Controls;
using BookingApp.Model.Enums;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BookingApp.View
{
    public partial class Guide : Window
    {
        private readonly TourRepository _tourRepository;
        private Tour selectedTour;
        private readonly KeyPointsRepository _keyPointsRepository;
        private readonly LiveTourRepository _liveTourRepository;
        private readonly TourReservationRepository _reservationDataRepository;
        public Guide()
        {
            InitializeComponent();
            _tourRepository = new TourRepository();
            _keyPointsRepository = new KeyPointsRepository();
            _reservationDataRepository = new TourReservationRepository();
            _liveTourRepository = new LiveTourRepository();
            selectedTour=new Tour();
            LoadTours();
        }

        private void LoadTours()
        {
            var tours = _tourRepository.GetTodayTours();
            tourListBox.ItemsSource = new ObservableCollection<Tour>(tours);
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateTour createTourWindow = new CreateTour();
            createTourWindow.Show();
        }



        private void UpdateSelectedTour(object sender, SelectionChangedEventArgs e)
        {
            if (tourListBox.SelectedItem != null)
            {
                selectedTour = (Tour)tourListBox.SelectedItem;
            }
            else
            {
                selectedTour = null;
            }
        }

        private bool IsActiveTour()
        {
            return _liveTourRepository.GetAllLiveTours().Any(t => t.IsLive);
        }



        private void StartTourButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsActiveTour())
            {
                MessageBox.Show("Please finish the active tour before starting a new one.");
                DisplayActiveTourDetails();
                return;
            }

            StartSelectedTour();
        }

        private void DisplayActiveTourDetails()
        {
            var activeTour = GetActiveTour();
            var activeTourKeyPoints = activeTour.KeyPoints;
            var tourists = _reservationDataRepository.GetUncheckedByTourId(activeTour.TourId);
            DisplayKeyPoints(activeTourKeyPoints);
            touristsListBox.ItemsSource = new ObservableCollection<TourReservation>(tourists);
        }

        private void StartSelectedTour()
        {
            if (selectedTour != null)
            {
                var keyPoints = LoadTourKeyPoints(selectedTour.Id);
                InitializeLiveTour(keyPoints);
                SetFirstKeyPointChecked(keyPoints);
                SetTourAsLive(selectedTour.Id);
                MessageBox.Show("Tour started successfully!");
                DisplayKeyPoints(keyPoints);
                LoadTourists();
            }
            else
            {
                MessageBox.Show("Please select a tour first.");
            }
        }

        private void InitializeLiveTour(List<KeyPoint> keyPoints)
        {
            LiveTour liveTour = new LiveTour(selectedTour.Id, keyPoints, true);
            _liveTourRepository.AddOrUpdateLiveTour(liveTour);
        }

        private void SetFirstKeyPointChecked(List<KeyPoint> keyPoints)
        {
            keyPoints[0].IsChecked = true;
            _liveTourRepository.SaveChanges();
        }

        private void LoadTourists()
        {
            if (selectedTour != null)
            {
                var tourists = _reservationDataRepository.GetByTourId(selectedTour.Id);
                touristsListBox.ItemsSource = new ObservableCollection<TourReservation>(tourists);
            }
        }


        private void DisplayKeyPoints(List<KeyPoint> keyPoints)
        {
            int keyPointsCounter = 0;
            keyPointsListBox.Items.Clear();
            foreach (var keyPoint in keyPoints)
            {
                StackPanel stackPanel = CreateStackPanel(keyPoint);
                TextBlock textBlock = CreateTextBlock(keyPoint.Name);
                CheckBox checkBox = CreateCheckBox(keyPoint);

                checkBox.Checked += (sender, e) =>
                {
                    keyPoint.IsChecked = true;
                    _liveTourRepository.SaveChanges();

                    int ordinalKeyNumber = keyPoint.OrdinalNumber;
                    for (int i = 1; i < ordinalKeyNumber; i++)
                    {
                        var previousKeyPoint = keyPoints[i - 1];
                        if (!previousKeyPoint.IsChecked)
                        {
                            MessageBox.Show("You need to check the previous key points before checking this one.");
                            checkBox.IsChecked = false;
                            keyPoint.IsChecked = false;
                            return;
                        }
                    }

                    keyPointsCounter = keyPoints.Count(kp => kp.IsChecked);
                    if (keyPointsCounter == keyPoints.Count)
                    {
                        FinishTourAutomatically();
                    }
                };

                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(checkBox);

                keyPointsListBox.Items.Add(stackPanel);
            }
        }

        private StackPanel CreateStackPanel(KeyPoint keyPoint)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            return stackPanel;
        }

        private TextBlock CreateTextBlock(string keyName)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = keyName;
            return textBlock;
        }

        private CheckBox CreateCheckBox(KeyPoint keyPoint)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.IsChecked = keyPoint.IsChecked;
            return checkBox;
        }

        private void FinishTourAutomatically()
        {
            var activeTour = GetActiveTour();
            if (activeTour != null)
            {
                var touristsOnTour = _reservationDataRepository.GetByTourId(activeTour.TourId);
                foreach (var tourist in touristsOnTour)
                {
                    tourist.IsOnTour = false;
                }
                _reservationDataRepository.SaveChanges();
                FinishActiveTour();

                MessageBox.Show("The tour has been successfully completed automatically!");
            }
        }

        private LiveTour GetActiveTour()
        {
            return _liveTourRepository.GetAllLiveTours().FirstOrDefault(t => t.IsLive);
        }



        private List<KeyPoint> LoadTourKeyPoints(int tourId)
        {
            return _keyPointsRepository.GetTourKeyPoints(tourId);
        }

      
        private void SetTourAsLive(int tourId)
        {
            _liveTourRepository.ActivateTour(tourId);
        }

        private void FinishTourButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsActiveTour())
            {
                var touristsOnTour = _reservationDataRepository.GetByTourId(GetActiveTour().TourId);
                foreach (var tourist in touristsOnTour)
                {
                    tourist.IsOnTour = false;
                }
                _reservationDataRepository.SaveChanges();
                FinishActiveTour();
                MessageBox.Show("Tura je uspešno završena!");
            }
            else
            {
                MessageBox.Show("Nema aktivne ture koju možete završiti.");
            }
        }

        private void FinishActiveTour()
        {
            var activeTour = GetActiveTour();
            if (activeTour != null)
            {
                activeTour.IsLive = false;
                var keyPoint = activeTour.KeyPoints;
                for (int i=0; i<activeTour.KeyPoints.Count;i++)
                {
                    keyPoint[i].IsChecked=false;
                    _liveTourRepository.SaveChanges();
                }
                _liveTourRepository.SaveChanges();
            }
        }

     
 

        private void AddTouristButtonClick(object sender, RoutedEventArgs e)
        {
            if (selectedTour != null && touristsListBox.SelectedItem != null)
            {
                var selectedTourist = (TourReservation)touristsListBox.SelectedItem;
                if (!selectedTourist.IsOnTour)
                {
                    var keyPoint = GetLastActiveKeyPoint();
                    selectedTourist.JoinedKeyPoint = keyPoint;
                    selectedTourist.IsOnTour = true;
                    _reservationDataRepository.UpdateReservation(selectedTourist);
                    MessageBox.Show($"Tourist {selectedTourist.TouristFirstName} added to tour at {keyPoint.Name}.");

                    var tourists = ((ObservableCollection<TourReservation>)touristsListBox.ItemsSource);
                    tourists.Remove(selectedTourist);
                }
            }
            else
            {
                MessageBox.Show("Please select a tour and a tourist first.");
            }
        }




        
        private KeyPoint GetLastActiveKeyPoint()
        {
            var liveTour = _liveTourRepository.GetAllLiveTours().FirstOrDefault(t => t.IsLive);
            if (liveTour != null)
            {
                var checkedKeyPoints = liveTour.KeyPoints.Where(k => k.IsChecked).ToList();
                if (checkedKeyPoints.Any())
                {
                    return checkedKeyPoints.Last();
                }
            }
            return null;
        }


    }
}