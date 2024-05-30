using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RecommendationsViewModel : INotifyPropertyChanged
    {
        private readonly PropertyReservationService _propertyReservationService;
        private int _topN;
        private int _bottomN;

        public RecommendationsViewModel()
        {
            _propertyReservationService = new PropertyReservationService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IReservedDateRepository>());
            _topN = 3;
            _bottomN = 3;
            LoadRecommendations();
        }

        private ObservableCollection<string> _popularLocations;
        public ObservableCollection<string> PopularLocations
        {
            get { return _popularLocations; }
            set
            {
                _popularLocations = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _unpopularLocations;
        public ObservableCollection<string> UnpopularLocations
        {
            get { return _unpopularLocations; }
            set
            {
                _unpopularLocations = value;
                OnPropertyChanged();
            }
        }

        private void LoadRecommendations()
        {
            PopularLocations = new ObservableCollection<string>(_propertyReservationService.GetMostPopularLocations(_topN));
            UnpopularLocations = new ObservableCollection<string>(_propertyReservationService.GetLeastPopularLocations(_bottomN));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

