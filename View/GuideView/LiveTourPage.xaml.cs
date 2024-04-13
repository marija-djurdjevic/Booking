using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.GuideView;
using BookingApp.ViewModel.GuidesViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View
{
   
    public partial class LiveTourPage : Page
    {
        private int tourId;

        private readonly TourRepository tourRepository;
        private Tour selectedTour;
        private readonly KeyPointRepository keyPointRepository;
        private readonly TouristGuideNotificationRepository touristGuideNotificationRepository;
        private readonly LiveTourRepository liveTourRepository;
        private readonly TourReservationRepository tourReservationRepository;
        public LiveTourPage(int tourId)
        {

            InitializeComponent();
            keyPointRepository = new KeyPointRepository();
            DataContext = new LiveTourViewModel(tourId);
            /* this.selectedTour = selectedTour;
             DataContext = selectedTour;
             tourRepository = new TourRepository(); 
             keyPointRepository = new KeyPointRepository();
             tourReservationRepository = new TourReservationRepository();
             liveTourRepository = new LiveTourRepository();
             touristGuideNotificationRepository = new TouristGuideNotificationRepository();

             if (IsActiveTour())
             {
                 MessageBox.Show("Please finish the active tour before starting a new one.");
                 DisplayActiveTourDetails();
                 return;
             }

             StartSelectedTour();
            */
        }




        private bool IsActiveTour()
        {
            return liveTourRepository.GetAllLiveTours().Any(t => t.IsLive);
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
            var tourists = tourReservationRepository.GetUncheckedByTourId(activeTour.TourId);
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
            liveTourRepository.AddOrUpdateLiveTour(liveTour);
        }

        private void SetFirstKeyPointChecked(List<KeyPoint> keyPoints)
        {
            keyPoints[0].IsChecked = true;
            liveTourRepository.SaveChanges();
        }

        private void LoadTourists()
        {
            if (selectedTour != null)
            {
                var tourists = tourReservationRepository.GetByTourId(selectedTour.Id);
                touristsListBox.ItemsSource = new ObservableCollection<TourReservation>(tourists);
            }
        }


       

        private void AddKeyPointToDisplay(KeyPoint keyPoint)
        {
            TextBlock textBlock = CreateTextBlock(keyPoint.Name);
            keyPointsListBox.Items.Add(textBlock);
        }

        private void ClearKeyPointsListBox()
        {
            keyPointsListBox.Items.Clear();
        }

        private void DisplayKeyPoints(List<KeyPoint> keyPoints)
        {
            ClearKeyPointsListBox();

            foreach (var keyPoint in keyPoints)
            {
                AddKeyPointToDisplay(keyPoint, keyPoints);
            }

            foreach (var item in keyPointsListBox.Items)
            {
                if (item is StackPanel stackPanel)
                {
                    foreach (var child in stackPanel.Children)
                    {
                        if (child is TextBlock textBlock)
                        {
                            textBlock.FontSize = 32;
                        }
                    }
                }
            }
        }

        private void AddKeyPointToDisplay(KeyPoint keyPoint, List<KeyPoint> keyPoints)
        {
            StackPanel stackPanel = CreateKeyPointStackPanel(keyPoint, keyPoints);
            keyPointsListBox.Items.Add(stackPanel);
        }



        


        private void HandleCheckBoxChecked(object sender, KeyPoint keyPoint, List<KeyPoint> keyPoints)
        {
            CheckBox checkBox = (CheckBox)sender;
            keyPoint.IsChecked = true;
            liveTourRepository.SaveChanges();

            if (!ArePreviousKeyPointsChecked(keyPoint, keyPoints))
            {
                MessageBox.Show("You need to check the previous key points before checking this one.");
                checkBox.IsChecked = false;
                keyPoint.IsChecked = false;
                return;
            }

            if (AreAllKeyPointsChecked(keyPoints))
            {
                FinishTourAutomatically();
            }
        }

        private bool ArePreviousKeyPointsChecked(KeyPoint keyPoint, List<KeyPoint> keyPoints)
        {
            int ordinalKeyNumber = keyPoint.OrdinalNumber;
            for (int i = 1; i < ordinalKeyNumber; i++)
            {
                var previousKeyPoint = keyPoints[i - 1];
                if (!previousKeyPoint.IsChecked)
                {
                    return false;
                }
            }
            return true;
        }

        private bool AreAllKeyPointsChecked(List<KeyPoint> keyPoints)
        {
            int checkedKeyPointsCount = keyPoints.Count(kp => kp.IsChecked);
            return checkedKeyPointsCount == keyPoints.Count;
        }




        private StackPanel CreateKeyPointStackPanel(KeyPoint keyPoint, List<KeyPoint> keyPoints)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            TextBlock textBlock = CreateTextBlock(keyPoint.Name);
            CheckBox checkBox = CreateCheckBox(keyPoint);

            checkBox.Checked += (sender, e) => HandleCheckBoxChecked(sender, keyPoint, keyPoints);


            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            checkBox.Margin = new Thickness(0, 0, 10, 0);
            stackPanel.Children.Add(checkBox);
            stackPanel.Children.Add(textBlock);


            return stackPanel;
        }



        private TextBlock CreateTextBlock(string keyName)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.Text = keyName;
            return textBlock;
        }

        private CheckBox CreateCheckBox(KeyPoint keyPoint)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.IsChecked = keyPoint.IsChecked;
            checkBox.HorizontalAlignment=HorizontalAlignment.Left;
            checkBox.Width = 20; 
            checkBox.Height = 20; 
            return checkBox;
        }


        private void FinishTourAutomatically()
        {
            var activeTour = GetActiveTour();
            if (activeTour != null)
            {
                var touristsOnTour = tourReservationRepository.GetByTourId(activeTour.TourId);
                foreach (var tourist in touristsOnTour)
                {
                    tourist.IsOnTour = false;
                }
                tourReservationRepository.SaveChanges();
                FinishActiveTour();

                MessageBox.Show("The tour has been successfully completed automatically!");
            }
        }

        private LiveTour GetActiveTour()
        {
            return liveTourRepository.GetAllLiveTours().FirstOrDefault(t => t.IsLive);
        }



        private List<KeyPoint> LoadTourKeyPoints(int tourId)
        {
            return keyPointRepository.GetTourKeyPoints(tourId);
        }


        private void SetTourAsLive(int tourId)
        {
            liveTourRepository.ActivateTour(tourId);
        }

        private void FinishTourButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsActiveTour())
            {
                var touristsOnTour = tourReservationRepository.GetByTourId(GetActiveTour().TourId);
                foreach (var tourist in touristsOnTour)
                {
                    tourist.IsOnTour = false;
                }
                tourReservationRepository.SaveChanges();

                

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
                for (int i = 0; i < activeTour.KeyPoints.Count; i++)
                {
                    keyPoint[i].IsChecked = false;
                    liveTourRepository.SaveChanges();
                }
                liveTourRepository.SaveChanges();
            }
        }




        private void AddTouristButtonClick(object sender, RoutedEventArgs e)
        {
            if (selectedTour == null || touristsListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a tour and a tourist first.");
                return;
            }

            var selectedTourist = (TourReservation)touristsListBox.SelectedItem;
            if (selectedTourist.IsOnTour)
            {
                MessageBox.Show("Tourist is already on tour.");
                return;
            }

            var keyPoint = GetLastActiveKeyPoint();
            selectedTourist.JoinedKeyPoint = keyPoint;
            selectedTourist.IsOnTour = true;
            tourReservationRepository.UpdateReservation(selectedTourist);
            MessageBox.Show($"Tourist {selectedTourist.TouristFirstName} added to tour at {keyPoint.Name}.");

            if (tourReservationRepository.IsUserOnTour(selectedTourist.UserId, selectedTourist.TourId))
            {
                List<string> addedPersons = new List<string> { $"{selectedTourist.TouristFirstName} {selectedTourist.TouristLastName}" };
                touristGuideNotificationRepository.Save(new TouristGuideNotification(selectedTourist.UserId, 2, selectedTourist.TourId, addedPersons, DateTime.Now, NotificationType.TouristJoined, keyPoint.Name, "Ognjen", selectedTour.Name));
            }

            var tourists = (ObservableCollection<TourReservation>)touristsListBox.ItemsSource;
            tourists.Remove(selectedTourist);
        }






        private KeyPoint GetLastActiveKeyPoint()
        {
            var liveTour = liveTourRepository.GetAllLiveTours().FirstOrDefault(t => t.IsLive);
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


        private void NavigateToMainPage(object sender, MouseButtonEventArgs e)
        {
            GuideMainPage1 guideMainPage = new GuideMainPage1();
            this.NavigationService.Navigate(guideMainPage);


        }


        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {
            SideMenuPage sideMenuPage = new SideMenuPage();
            this.NavigationService.Navigate(sideMenuPage);
        }






        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            KeyPoint keyPoint = (KeyPoint)checkBox.DataContext;
            keyPoint.IsChecked = true;
            keyPointRepository.SaveChanges();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            KeyPoint keyPoint = (KeyPoint)checkBox.DataContext;
            keyPoint.IsChecked = false;
            keyPointRepository.SaveChanges();
        }
    }
}
