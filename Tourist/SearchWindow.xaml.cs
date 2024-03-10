using BookingApp.DTO;
using BookingApp.Repository;
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
using BookingApp.Model;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.Tourist
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window, INotifyPropertyChanged
    {
        public TourDto SearchParams { get; set; }
        public static ObservableCollection<TourDto> Tours { get; set; }

        private readonly TourRepository repository;

        public bool IsCancelSearchButtonVisible { get; set; }

        public SearchWindow(ObservableCollection<TourDto> tours)
        {
            InitializeComponent();
            DataContext = this;
            Tours = tours;
            SearchParams = new TourDto();
            repository = new TourRepository();
            IsCancelSearchButtonVisible = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            List<Tour> matchingTours = repository.getMatchingTours(SearchParams);

            if (matchingTours.Count > 0) {
                Tours.Clear();
                foreach (var tour in matchingTours)
                {
                    Tours.Add(new TourDto(tour));
                }

                IsCancelSearchButtonVisible = true;
            }
            else
            {
                MessageBox.Show("There are no tours with that parameters");
            }

            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
