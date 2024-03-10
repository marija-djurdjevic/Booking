using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using BookingApp.DTO;
using BookingApp.Repository;

namespace BookingApp.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window, INotifyPropertyChanged
    {
        public static ObservableCollection<TourDto> Tours {  get; set; }

        public TourDto SelectedTour { get; set; }

        private readonly TourRepository repository;

        private bool _isCancelSearchButtonVisible;

        public TouristMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            repository = new TourRepository();
            Tours = new ObservableCollection<TourDto>();
            IsCancelSearchButtonVisible = false;
            GetAllTours();
        }

        public bool IsCancelSearchButtonVisible
        {
            get => _isCancelSearchButtonVisible;
            set
            {
                if (value != _isCancelSearchButtonVisible)
                {
                    _isCancelSearchButtonVisible = value;
                    OnPropertyChanged("IsCancelSearchButtonVisible");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetAllTours()
        {
            Tours.Clear();
            foreach (var tour in repository.GetAllTours())
            {
                Tours.Add(new TourDto(tour));
            }
        }

        private void SelectedTourCard(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Kliknuli");
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow(Tours);
            searchWindow.ShowDialog();

            IsCancelSearchButtonVisible = searchWindow.IsCancelSearchButtonVisible;
        }

        private void CancelSearchButtonClick(object sender, RoutedEventArgs e)
        {
            IsCancelSearchButtonVisible = false;
            GetAllTours();
        }
    }
}
