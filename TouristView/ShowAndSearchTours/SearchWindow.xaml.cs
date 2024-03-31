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
using System.IO;
using System.Collections;

namespace BookingApp.TouristView
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public TourDto SearchParams { get; set; }
        public static ObservableCollection<TourDto> Tours { get; set; }
        public static ObservableCollection<string> Cities { get; set; }
        public static ObservableCollection<string> Countires { get; set; }
        public static ObservableCollection<string> Languages { get; set; }

        private readonly TourRepository TourRepository;

        private Dictionary<string, List<string>> countriesAndCities = new Dictionary<string, List<string>>();
        public bool IsCancelSearchButtonVisible { get; set; }


        public SearchWindow(ObservableCollection<TourDto> tours)
        {
            InitializeComponent();
            DataContext = this;
            Tours = tours;
            SearchParams = new TourDto();
            TourRepository = new TourRepository();
            Countires = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            Languages = new ObservableCollection<string>();
            IsCancelSearchButtonVisible = false;

            UcitajPodatkeIzCSV("../../../Resources/Data/AllCitiesAndCountries.csv");
            UcitajPodatkeIzCSVLanguage("../../../Resources/Data/AllLanguages.csv");

            // Popunjavanje ComboBox-a sa državama
            foreach (string country in countriesAndCities.Keys)
            {
                Countires.Add(country);
            }

            // Povezivanje događaja za promenu izabrane države
            comboBoxDrzave.SelectionChanged += (sender, e) =>
            {
                PopuniGradove();
                //ComboCity.IsDropDownOpen = true;
            };

        }

        private void UcitajPodatkeIzCSV(string putanjaDoDatoteke)
        {
            using (StreamReader sr = new StreamReader(putanjaDoDatoteke))
            {
                while (!sr.EndOfStream)
                {
                    string[] tokens = sr.ReadLine().Split(',');
                    string drzava = tokens[1];
                    string grad = tokens[0];

                    Cities.Add(grad);

                    if (!countriesAndCities.ContainsKey(drzava))
                    {
                        countriesAndCities.Add(drzava, new List<string>());
                    }

                    countriesAndCities[drzava].Add(grad);
                }
            }
        }

        private void UcitajPodatkeIzCSVLanguage(string putanjaDoDatoteke)
        {
            using (StreamReader sr = new StreamReader(putanjaDoDatoteke))
            {
                while (!sr.EndOfStream)
                {
                    string language = sr.ReadLine();

                    Languages.Add(language);

                }
            }
        }

        private void PopuniGradove()
        {
            Cities.Clear();
            if (comboBoxDrzave.SelectedItem != null)
            {
                string izabranaDrzava = comboBoxDrzave.SelectedItem.ToString();
                foreach (string grad in countriesAndCities[izabranaDrzava])
                {
                    Cities.Add(grad);
                }
            }

        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            List<Tour> matchingTours = TourRepository.GetMatchingTours(SearchParams);

            if (matchingTours.Count > 0)
            {
                UpdateCollection(matchingTours);
                IsCancelSearchButtonVisible = true;
            }
            else
            {
                MessageBox.Show("There are no tours with that parameters");
                UpdateCollection(TourRepository.GetAll());
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

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
