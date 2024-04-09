using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel; // Dodato za INotifyPropertyChanged
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
    public partial class TouristsNumberPage : Page, INotifyPropertyChanged
    {
        private int tourId;
        private int under18Count;
        private int between18And50Count;
        private int over50Count;



        public TouristsNumberPage(int tourId)
        {
            InitializeComponent();
            this.tourId = tourId;

            // Pozovite metodu za brojanje turista po godinama
            CountTouristsByAge();
            DataContext = this;
        }

        public int Under18Count
        {
            get { return under18Count; }
            set
            {
                if (under18Count != value)
                {
                    under18Count = value;
                    OnPropertyChanged(nameof(Under18Count));
                }
            }
        }

        public int Between18And50Count
        {
            get { return between18And50Count; }
            set
            {
                if (between18And50Count != value)
                {
                    between18And50Count = value;
                    OnPropertyChanged(nameof(Between18And50Count));
                }
            }
        }

        public int Over50Count
        {
            get { return over50Count; }
            set
            {
                if (over50Count != value)
                {
                    over50Count = value;
                    OnPropertyChanged(nameof(Over50Count));
                }
            }
        }

        

        private void CountTouristsByAge()
        {
            TouristRepository touristRepository = new TouristRepository();
            var touristIds = new TouristExperienceRepository().GetTouristIdsByTourId(tourId);

            foreach (var touristId in touristIds)
            {
                int age = touristRepository.GetAgeById(touristId);
                if (age < 18)
                {
                    Under18Count++;
                }
                else if (age >= 18 && age <= 50)
                {
                    Between18And50Count++;
                }
                else
                {
                    Over50Count++;
                }
            }
        }

        private void NavigateToSideMenuPage(object sender, MouseButtonEventArgs e)
        {
            // Implementacija navigacije
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
