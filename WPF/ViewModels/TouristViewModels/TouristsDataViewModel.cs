using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BookingApp.Aplication.UseCases;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.WPF.ViewModel.TouristViewModel
{
    public class TouristsDataViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Tuple<TourReservation, string, bool>> Tourists { get; set; }
        public TourDto SelectedTour { get; set; }
        public Tourist LoggedInTourist { get; set; }
        public TourService TourService { get; set; }
        private readonly TouristService touristService;
        private VoucherService voucherService;
        private TourReservationService reservationDataService;

        public string TitleTxt { get; set; }

        public TouristsDataViewModel(int touristNumber, TourDto selectedTour, int userId)
        {
            Tourists = new ObservableCollection<Tuple<TourReservation, string, bool>>();
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            reservationDataService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());

            SelectedTour = selectedTour;
            LoggedInTourist = touristService.GetByUserId(userId);

            TitleTxt = "Enter the data of " + touristNumber + " people";

            Tourists.Add(new Tuple<TourReservation, string, bool>(new TourReservation(SelectedTour.Id, LoggedInTourist, true), "Tourist 1", true));
            for (int i = 0; i < touristNumber - 1; i++)
            {
                int a = i + 2;
                Tourists.Add(new Tuple<TourReservation, string, bool>(new TourReservation(SelectedTour.Id, userId, false), "Tourist " + a, false));
            }

        }

        public bool UseVouchers()
        {

            if (voucherService.GetByToueristId(LoggedInTourist.Id).Count() > 0)
            {
                MessageBoxResult useVouchers = MessageBox.Show("Would you like to use vouchers for booking this tour?", "Vouchers", MessageBoxButton.YesNo);
                if (useVouchers == MessageBoxResult.Yes)
                {
                    VouchersForReservationWindow vouchersForReservationWindow = new VouchersForReservationWindow(LoggedInTourist);
                    vouchersForReservationWindow.ShowDialog();
                    if (!vouchersForReservationWindow.VouchersForReservationViewModel.WindowReturnValue)
                    {
                        return false;
                    }
                }

            }

            foreach (TourReservation data in Tourists.Select(t => t.Item1).ToList())
            {
                reservationDataService.Save(data);
            }

            SelectedTour.MaxTouristNumber -= Tourists.Count();
            TourService.Update(SelectedTour.ToTour());

            MessageBoxResult successfullyBooked = MessageBox.Show("Reservation successfully created?", "Reservation", MessageBoxButton.OK);
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
