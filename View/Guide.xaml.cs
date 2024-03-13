using System.Collections.ObjectModel;
using System.Windows;
using BookingApp.Repository;
using BookingApp.Model;
using System.Linq;
using System.Windows.Controls;
using BookingApp.Model.Enums;
using System.Collections.Generic;

namespace BookingApp.View
{
    public partial class Guide : Window
    {
        private readonly TourRepository _tourRepository;
        private Tour _selectedTour;
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
            _selectedTour=new Tour();
            LoadTours();
        }

        private void LoadTours()
        {
            var tours = _tourRepository.GetAllTours();
            tourListBox.ItemsSource = new ObservableCollection<Tour>(tours);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreatingTour creatingTourWindow = new CreatingTour();
            creatingTourWindow.Show();
        }



        private void tourListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tourListBox.SelectedItem != null)
            {
                _selectedTour = (Tour)tourListBox.SelectedItem;
            }
            else
            {
                _selectedTour = null;
            }
        }

        private bool IsActiveTour()
        {
            return _liveTourRepository.GetAllLiveTours().Any(t => t.IsLive);
        }



        private void StartTourButton_Click(object sender, RoutedEventArgs e)
        {

            if (IsActiveTour())
            {
                MessageBox.Show("There is already an active tour. Please finish the current tour before starting a new one.");
                return;
            }

            if (_selectedTour != null)
            {

                
                var keyPoints = LoadKeyPointsForTour(_selectedTour.Id);
                LiveTour liveTour = new LiveTour(_selectedTour.Id, keyPoints, true);
                keyPoints[0].IsChecked = true;
                _liveTourRepository.SaveChanges();

                AddKeyPointsToLiveTour(keyPoints);

                // Postavi tura kao live u livetour.csv
                SetTourAsLive(_selectedTour.Id);

                MessageBox.Show("Tour started successfully!");
                DisplayKeyPoints(keyPoints);
                LoadTouristsForSelectedTour();
            }
            else
            {
                MessageBox.Show("Please select a tour first.");
            }
        }
        private void DisplayKeyPoints(List<KeyPoints> keyPoints)
        {
            int keyCounter = 0;
            keyPointsListBox.Items.Clear();
            foreach (var keyPoint in keyPoints)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;

                TextBlock textBlock = new TextBlock();
                textBlock.Text = keyPoint.KeyName;

                CheckBox checkBox = new CheckBox();
                checkBox.IsChecked = keyPoint.IsChecked;
                checkBox.Checked += (sender, e) =>
                {
                    keyPoint.IsChecked = true;
                    _liveTourRepository.SaveChanges();
                    int ordinalKeyNumber = keyPoint.OrdinalNumber;


                    for (int i = 1; i < ordinalKeyNumber; i++)
                    {

                        var keyPoint = keyPoints[i];
                        bool allPreviousChecked = true;
                        for (int j = 0; j < i; j++)
                        {
                            if (!keyPoints[j].IsChecked)
                            {
                                allPreviousChecked = false;
                                break;
                            }
                        }

                        if (!allPreviousChecked)
                        {
                           
                            MessageBox.Show("Morate cekirati prethodne kljucne tacke pre nego sto cekirate ovu.");
                            checkBox.IsChecked = false;
                        }
                    }
                        foreach (var keyPoint in keyPoints)
                        {
                            if (keyPoint.IsChecked)
                                 keyCounter++;
                        }
                    if(keyCounter == keyPoints.Count)
                    {
                        FinishTourAutomatically();
                    }
                    keyCounter = 0;


                };
                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(checkBox);

                keyPointsListBox.Items.Add(stackPanel);
            }
        }


        private void FinishTourAutomatically()
        {
            
            FinishActiveTour();
            MessageBox.Show("Tura je uspešno završena automatski!");
        }


        private List<KeyPoints> LoadKeyPointsForTour(int tourId)
        {
            return _keyPointsRepository.GetKeyPointsForTour(tourId);
        }

        private void AddKeyPointsToLiveTour(List<KeyPoints> keyPoints)
        {
            LiveTour liveTour = new LiveTour(_selectedTour.Id, keyPoints, true);
            _liveTourRepository.AddOrUpdateLiveTour(liveTour);
        }

        private void SetTourAsLive(int tourId)
        {
            _liveTourRepository.SetTourAsLive(tourId);
        }

        private void FinishTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsActiveTour())
            {
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

     
         
        private LiveTour GetActiveTour()
        {
            return _liveTourRepository.GetAllLiveTours().FirstOrDefault(t => t.IsLive);
        }



        private void AddTouristButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTour != null && touristsListBox.SelectedItem != null)
            {
                var selectedTourist = (TourReservation)touristsListBox.SelectedItem;
                var keyPoint = GetActiveKeyPoint(); // Metoda za dobijanje trenutne ključne tačke
                selectedTourist.JoinedKeyPoint = keyPoint;

                _reservationDataRepository.SaveChanges();

                MessageBox.Show($"Tourist {selectedTourist.TouristFirstName} added to tour at {keyPoint.KeyName}.");

                // Remove selected tourist from the list
                var tourists = ((ObservableCollection<TourReservation>)touristsListBox.ItemsSource);
                tourists.Remove(selectedTourist);
            }
            else
            {
                MessageBox.Show("Please select a tour and a tourist first.");
            }
        }




        private void LoadTouristsForSelectedTour()
        {
            if (_selectedTour != null)
            {
                var tourists = _reservationDataRepository.GetByTourId(_selectedTour.Id);
                touristsListBox.ItemsSource = new ObservableCollection<TourReservation>(tourists);
            }
        }
        private KeyPoints GetActiveKeyPoint()
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