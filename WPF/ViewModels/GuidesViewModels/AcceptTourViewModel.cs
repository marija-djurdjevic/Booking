using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using BookingApp.WPF.Views.GuideView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class AcceptTourViewModel : BaseViewModel
    {
        private int id;
        private  ObservableCollection<(DateTime, DateTime)> freeDates = new ObservableCollection<(DateTime, DateTime)>();
        private  readonly TourRequestService tourRequestService;
        private  ObservableCollection<(DateTime, DateTime)> bookedDates;
        private (DateTime, DateTime) touristsDates;
        public ObservableCollection<(DateTime, DateTime)> BookedDates { get; set; }

        public (DateTime, DateTime) TouristsDates { get; set; }
        public AcceptTourViewModel(int id)
        {
            this.id = id;
            tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(),Injector.CreateInstance<ITourRepository>());
            LoadBookedDates();
            TouristsDates=tourRequestService.GetDateSlotById(id);
            FreeDates = new ObservableCollection<(DateTime, DateTime)>(tourRequestService.CalculateFreeDates(BookedDates.ToList(), TouristsDates));

        }

        public ObservableCollection<(DateTime, DateTime)> FreeDates
        { 
             get { return freeDates; }
             set
             {
                freeDates = value;
                OnPropertyChanged();
             }
        }


        
        private void LoadBookedDates()
        {
            BookedDates = new ObservableCollection<(DateTime, DateTime)>(tourRequestService.GetUpcomingToursDates());

        }
       

    }
}
