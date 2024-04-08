using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Build.Evaluation;
using BookingApp.Repository;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace BookingApp.TouristView.MyTours
{
    /// <summary>
    /// Interaction logic for RateTourWindow.xaml
    /// </summary>
    public partial class RateTourWindow : Window, INotifyPropertyChanged
    {
        private string showingImage { get; set; }
        public int ImageIndex { get; set; }
        public TourDto SelectedTour { get; set; }
        public User LoggedInTourist { get; set; }
        public TouristExperience TouristExperience { get; set; }
        private TouristExperienceRepository touristExperienceRepository { get; set; }

        public RateTourWindow(TourDto selectedTour, User loggedInTourist)
        {
            InitializeComponent();
            DataContext = this;

            TouristExperience = new TouristExperience();
            touristExperienceRepository = new TouristExperienceRepository();
            SelectedTour= selectedTour;
            LoggedInTourist = loggedInTourist;

            ImageIndex = -1;
        }

        public string ShowingImage
        {
            get { return showingImage; }
            set
            {
                showingImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowingImage)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            TouristExperience.TouristId = LoggedInTourist.Id;
            TouristExperience.TourId = SelectedTour.Id;
            TouristExperience.TourInterestingesRating = int.Parse(GetSelectedRadioButtonValue(Interesting,"interesting"));
            TouristExperience.GuideKnowledgeRating = int.Parse(GetSelectedRadioButtonValue(Knowledge, "knowledge"));
            TouristExperience.GuideLanguageRating = int.Parse(GetSelectedRadioButtonValue(Language, "language"));
            
            touristExperienceRepository.Save(TouristExperience);

            Close();
        }

        private string GetSelectedRadioButtonValue(UniformGrid RadioName,string groupName)
        {
            var radioButtons = RadioName.Children.OfType<RadioButton>().Where(r => r.GroupName == groupName);

            var selectedRadioButton = radioButtons.FirstOrDefault(r => r.IsChecked == true);

            if (selectedRadioButton != null)
            {
                return selectedRadioButton.Content.ToString();
            }

            return null;
        }

        private void AddImageButtonClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            openFileDialog.Title = "Select an Image File";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                string relativnaPutanja = "TouristView\\Images";


                string uniqueFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(selectedImagePath);

                string destinationFilePath = System.IO.Path.Combine(GetRelativePath(relativnaPutanja), uniqueFileName);

                System.IO.File.Copy(selectedImagePath, destinationFilePath);
                ShowingImage = "/TouristView/Images/"+uniqueFileName;
                TouristExperience.ImagesPaths.Add(ShowingImage);                 
            }

        }

        private string GetRelativePath(string relativePath)
        {
            string projectPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            
            string solutionPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(projectPath)));

            string destinationFilePath = System.IO.Path.Combine(solutionPath, relativePath);

            return destinationFilePath;
        }   

        private void RemoveImageButtonClick(object sender, RoutedEventArgs e)
        {
            TouristExperience.ImagesPaths.Remove(ShowingImage);
        }

        public void GetNextImage()
        {
            if (ImageIndex < TouristExperience.ImagesPaths.Count - 1)
            {
                string imagePath = SelectedTour.ImagesPaths[++ImageIndex];
                ShowingImage = imagePath;
            }
        }

        public void GetPreviousImage()
        {
            if (ImageIndex > 0)
            {
                string imagePath = TouristExperience.ImagesPaths[--ImageIndex];
                ShowingImage = imagePath;

            }
        }

        private void NextImageButtonClick(object sender, RoutedEventArgs e)
        {
            GetNextImage();
        }

        private void PreviousImageButtonClick(object sender, RoutedEventArgs e)
        {
            GetPreviousImage();
        }
    }
}
