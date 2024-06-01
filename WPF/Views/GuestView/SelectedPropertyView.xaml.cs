using BookingApp.Domain.Models;
using BookingApp.Aplication.Dto;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using BookingApp.WPF.Views.GuestView;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for SelectedPropertyView.xaml
    /// </summary>
    public partial class SelectedPropertyView : Page, INotifyPropertyChanged
    {
        private int _currentImageIndex;
        private string _currentImage;

        public string CurrentImage
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
        }
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public static PropertyReservationRepository PropertyReservationRepository = new PropertyReservationRepository();
        public Guest LoggedInGuest { get; set; }
        public Property SelectedProperty { get; set; }
        public SelectedPropertyView(Property selectedProperty, Guest guest, PropertyRepository propertyRepository, PropertyReservationRepository propertyReservationRepository)
        {
            InitializeComponent();
            DataContext = this;
            _currentImageIndex = 0;
            NextImageCommand = new HelpCommand(NextImage, CanNextImage);
            PreviousImageCommand = new HelpCommand(PreviousImage, CanPreviousImage);
            SelectedProperty = selectedProperty;
            CurrentImage = SelectedProperty.ImagesPaths[_currentImageIndex];
            LoggedInGuest = guest;
            PropertyRepository = propertyRepository;
            PropertyReservationRepository = propertyReservationRepository;
        }

        public ICommand NextImageCommand { get; }
        public ICommand PreviousImageCommand { get; }

        private void NextImage(object parameter)
        {
            if (_currentImageIndex < SelectedProperty.ImagesPaths.Count - 1)
            {
                _currentImageIndex++;
                CurrentImage = SelectedProperty.ImagesPaths[_currentImageIndex];
            }
        }

        private bool CanNextImage(object parameter)
        {
            return _currentImageIndex < SelectedProperty.ImagesPaths.Count - 1;
        }

        private void PreviousImage(object parameter)
        {
            if (_currentImageIndex > 0)
            {
                _currentImageIndex--;
                CurrentImage = SelectedProperty.ImagesPaths[_currentImageIndex];
            }
        }

        private bool CanPreviousImage(object parameter)
        {
            return _currentImageIndex > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MakeReservation_Click(object sender, RoutedEventArgs e)
        {
            PropertyBooking propertybooking = new PropertyBooking(SelectedProperty, LoggedInGuest, PropertyRepository, PropertyReservationRepository);
            NavigationService.Navigate(propertybooking);
        }
    }
}
