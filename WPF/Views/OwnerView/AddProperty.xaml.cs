using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using BookingApp.Domain.Models;
using BookingApp.Aplication.Dto;

namespace BookingApp.View
{
    public partial class AddProperty : Page, INotifyPropertyChanged
    {
        private PropertyDto _propertyDto;
        public User LoggedInUser { get; set; }
        public Owner LoggedInOwner { get; set; }
        public OwnerRepository OwnerRepository { get; set; }

        private PropertyRepository propertyRepository;
        public ObservableCollection<string> ImagesPaths { get; set; }
        private ObservableCollection<BitmapImage> addedImages;

        public ObservableCollection<BitmapImage> AddedImages
        {
            get { return addedImages; }
            set
            {
                addedImages = value;
                OnPropertyChanged(nameof(AddedImages));
            }
        }

        private BitmapImage selectedImage;
        public BitmapImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddProperty(User loggedInUser)
        {
            InitializeComponent();
            OwnerRepository = new OwnerRepository();
            this.LoggedInUser = loggedInUser;
            LoggedInOwner = OwnerRepository.GetByUserId(loggedInUser.Id);
            _propertyDto = new PropertyDto();
            ImagesPaths = new ObservableCollection<string>();
            AddedImages = new ObservableCollection<BitmapImage>();
            DataContext = this; // Set DataContext to this
            propertyRepository = new PropertyRepository();
        }

        private void SaveProperty_Click(object sender, RoutedEventArgs e)
        {
            _propertyDto.OwnerId = LoggedInUser.Id;
            PropertyDto newPropertyDto = new PropertyDto(_propertyDto.OwnerId, _propertyDto.Name, _propertyDto.LocationDto, _propertyDto.Type, _propertyDto.MaxGuests, _propertyDto.MinReservationDays, _propertyDto.CancellationDeadline, _propertyDto.ImagesPaths);
            propertyRepository.AddProperty(newPropertyDto.ToProperty());
            int id = propertyRepository.NextId() - 1;

            MessageBox.Show("Property created successfully!");
            NavigationService.GoBack();
        }

        private void AddImagePathButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                Multiselect = true // Allow multiple image selection
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var selectedFilePath in openFileDialog.FileNames)
                {
                    // Define the relative path and destination path
                    string relativePath = "Resources\\Images\\OwnerImages" + Path.GetFileName(selectedFilePath);
                    string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                    // Copy the file to the destination path
                    File.Copy(selectedFilePath, destinationPath, true);

                    // Add the relative path to the ImagesPaths collection
                    ImagesPaths.Add(relativePath);
                    _propertyDto.ImagesPaths.Add(relativePath);

                    // Add the image to the preview collection
                    BitmapImage bitmap = new BitmapImage(new Uri(destinationPath));
                    AddedImages.Add(bitmap);
                }
            }
        }

        private void RemoveSelectedImage_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedImage != null)
            {
                int index = AddedImages.IndexOf(SelectedImage);
                if (index != -1)
                {
                    AddedImages.RemoveAt(index);
                    if (index < ImagesPaths.Count)
                    {
                        ImagesPaths.RemoveAt(index);
                        _propertyDto.ImagesPaths.RemoveAt(index);
                    }
                }
            }
        }
    }
}
