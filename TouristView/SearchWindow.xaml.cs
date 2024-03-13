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

namespace BookingApp.TouristView
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public TourDto SearchParams { get; set; }
        public static ObservableCollection<TourDto> Tours { get; set; }

        private readonly TourRepository TourRepository;
        public bool IsCancelSearchButtonVisible { get; set; }


        public SearchWindow(ObservableCollection<TourDto> tours)
        {
            InitializeComponent();
            DataContext = this;
            Tours = tours;
            SearchParams = new TourDto();
            TourRepository = new TourRepository();
            IsCancelSearchButtonVisible = false;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            List<Tour> matchingTours = TourRepository.getMatchingTours(SearchParams);

            if (matchingTours.Count > 0)
            {
                UpdateCollection(matchingTours);
                IsCancelSearchButtonVisible = true;
            }
            else
            {
                MessageBox.Show("There are no tours with that parameters");
                UpdateCollection(TourRepository.GetAllTours());
            }

            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void UpdateCollection(List<Tour> tours)
        {
            Tours.Clear();
            foreach (var tour in tours)
            {
                Tours.Add(new TourDto(tour));
            }
        }
    }
}
