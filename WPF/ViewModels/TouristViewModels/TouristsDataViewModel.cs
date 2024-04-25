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

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TouristsDataViewModel : BindableBase
    {
        public ObservableCollection<Tuple<TourReservation, string, bool>> Tourists { get; set; }
        public TourDto SelectedTour { get; set; }
        public TourRequest TourRequest { get; set; }
        public bool IsRequest { get; set; }
        public Tourist LoggedInTourist { get; set; }
        public TourService TourService { get; set; }
        private readonly TouristService touristService;
        private VoucherService voucherService;
        private TourReservationService reservationDataService;
        private TourRequestService requestService;

        public string TitleTxt { get; set; }

        public TouristsDataViewModel(int touristNumber, TourDto selectedTour, int userId, bool isRequest, TourRequest tourRequest)
        {
            Tourists = new ObservableCollection<Tuple<TourReservation, string, bool>>();
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            reservationDataService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            TourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            requestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>(), Injector.CreateInstance<ITourRepository>());

            SelectedTour = selectedTour;
            TourRequest = tourRequest;
            LoggedInTourist = touristService.GetByUserId(userId);

            TitleTxt = "Enter the data of " + touristNumber + " people";

            Tourists.Add(new Tuple<TourReservation, string, bool>(new TourReservation(SelectedTour.Id, LoggedInTourist, true), "Tourist 1", true));
            for (int i = 0; i < touristNumber - 1; i++)
            {
                int a = i + 2;
                Tourists.Add(new Tuple<TourReservation, string, bool>(new TourReservation(SelectedTour.Id, userId, false), "Tourist " + a, false));
            }
            IsRequest = isRequest;
        }

        public bool Confirm()
        {
            if(IsRequest)
            {
                foreach (Tuple<TourReservation, string, bool> data in Tourists)
                {
                    Tuple<string, string, int> person = new Tuple<string, string, int>(data.Item1.TouristFirstName,data.Item1.TouristLastName,data.Item1.TouristAge);
                    TourRequest.Persons.Add(person);
                }
                requestService.CreateRequest(TourRequest);
                MessageBoxResult successfullyCreated = MessageBox.Show("Tour request successfully created?", "Request", MessageBoxButton.OK);
                return true;
            }
            else
            {
                return UseVouchers();
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
    }
}
