using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for OwnerReview.xaml
    /// </summary>
    public partial class OwnerRating : Page
    {
        public PropertyReservation SelectedReservation { get; set; }
        public Property SelectedProperty { get; set; }
        public Guest LoggedInGuest { get; set; }
        public OwnerReviewDto OwnerReview { get; set; }
        public OwnerReviewRepository OwnerReviewRepository { get; set; }
        private ObservableCollection<BitmapImage> images = new ObservableCollection<BitmapImage>();
        public OwnerRating(PropertyReservation selectedReservation, Property selectedProperty, Guest guest)
        {
            InitializeComponent();
            DataContext = this;
            OwnerReview = new OwnerReviewDto();
            SelectedReservation = selectedReservation;
            OwnerReview.ReservationId = SelectedReservation.Id;
            SelectedProperty = selectedProperty;
            OwnerReviewRepository = new OwnerReviewRepository();
            LoggedInGuest = guest;
            imageListBox.ItemsSource = images;
        }

        private void AddPhotos_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif)|*.png;*.jpeg;*.jpg;*.gif|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Iteriranje kroz odabrane datoteke i dodavanje slika u ObservableCollection
                foreach (string fileName in openFileDialog.FileNames)
                {
                    OwnerReview.ImagesPaths.Add(fileName);
                    BitmapImage bitmap = new BitmapImage(new Uri(fileName));
                    images.Add(bitmap);
                }
            }
        }

        private void Send_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OwnerReviewRepository.AddOwnerReview(OwnerReview.ToOwnerReview());
            MessageBox.Show("Review sent!");
        }

        private void CleanlinessRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                string cleanliness = radioButton.Content.ToString();
                switch (cleanliness)
                {
                    case "1":
                        OwnerReview.Cleanliness = 1;
                        break;
                    case "2":
                        OwnerReview.Cleanliness = 2;
                        break;
                    case "3":
                        OwnerReview.Cleanliness = 3;
                        break;
                    case "4":
                        OwnerReview.Cleanliness = 4;
                        break;
                    case "5":
                        OwnerReview.Cleanliness = 5;
                        break;
                    default:
                        OwnerReview.Cleanliness = 0; 
                        break;
                }
            }
        }

        private void CorrectnessRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                string correctness = radioButton.Content.ToString();
                switch (correctness)
                {
                    case "1":
                        OwnerReview.Correctness = 1;
                        break;
                    case "2":
                        OwnerReview.Correctness = 2;
                        break;
                    case "3":
                        OwnerReview.Correctness = 3;
                        break;
                    case "4":
                        OwnerReview.Correctness = 4;
                        break;
                    case "5":
                        OwnerReview.Correctness = 5;
                        break;
                    default:
                        OwnerReview.Correctness = 0;
                        break;
                }
            }
        }
    }
}
