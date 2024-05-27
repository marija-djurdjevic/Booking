using BookingApp.Domain.Models;
using System.Collections.Generic;
using System;
using BookingApp.Domain.Models;
using System.Windows;

namespace BookingApp.Aplication.UseCases
{
    public class TourCancellationService
    {
        private readonly LiveTourService liveTourService;
        private readonly TourReservationService tourReservationService;
        private readonly TourService tourService;
        private readonly KeyPointService keyPointService;
        private readonly VoucherService voucherService;
        private readonly TouristService touristService;

        public TourCancellationService(LiveTourService liveTourService, TourReservationService tourReservationService, TourService tourService, KeyPointService keyPointService, VoucherService voucherService, TouristService touristService)
        {
            this.liveTourService = liveTourService;
            this.tourReservationService = tourReservationService;
            this.tourService = tourService;
            this.keyPointService = keyPointService;
            this.voucherService = voucherService;
            this.touristService = touristService;
        }

        public void CancelTour(Tour tour, List<KeyPoint> keyPoints, List<TourReservation> tourReservations,int guideId)
        {
            if ((tour.StartDateTime - DateTime.Now).TotalHours <= 48)
            {
                MessageBox.Show("Tour cannot be canceled less than 48 hours before the start.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            liveTourService.RemoveLiveTour(tour.Id);
            tourReservationService.DeleteByTourId(tour.Id);
            DeleteTourAndKeyPoints(tour.Id);
            GenerateVouchersForCanceledTourists(tour.Id, tourReservations,guideId);
            MessageBox.Show("Tour canceled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        public void QuitJob(Tour tour, List<KeyPoint> keyPoints, List<TourReservation> tourReservations, int guideId)
        {
           
            liveTourService.RemoveLiveTour(tour.Id);
            tourReservationService.DeleteByTourId(tour.Id);
            DeleteTourAndKeyPoints(tour.Id);
            GenerateVouchersForQuitJob(tour.Id, tourReservations, guideId);
            
        }






        private void DeleteTourAndKeyPoints(int tourId)
        {
            tourService.Delete(tourId);
            keyPointService.DeleteKeyPoints(tourId);
        }

        private void GenerateVouchersForCanceledTourists(int tourId, List<TourReservation> tourReservations,int guideId)
        {
            var usersToReceiveVoucher = new List<int>();

            foreach (var reservation in tourReservations)
            {
                int userId = reservation.UserId;
                var tourist = touristService.GetByUserId(userId);

                if (tourist != null && IsTouristReservationMatch(tourist, reservation))
                {
                    usersToReceiveVoucher.Add(userId);
                }
            }

            foreach (var userId in usersToReceiveVoucher)
            {
                GenerateVoucher(userId,guideId);
            }
        }







        private void GenerateVouchersForQuitJob(int tourId, List<TourReservation> tourReservations, int guideId)
        {
            var usersToReceiveVoucher = new List<int>();

            foreach (var reservation in tourReservations)
            {
                int userId = reservation.UserId;
                var tourist = touristService.GetByUserId(userId);

                if (tourist != null && IsTouristReservationMatch(tourist, reservation))
                {
                    usersToReceiveVoucher.Add(userId);
                }
            }

            foreach (var userId in usersToReceiveVoucher)
            {
                GenerateVoucherQuitJob(userId, guideId);
            }
        }











        private bool IsTouristReservationMatch(Tourist tourist, TourReservation reservation)
        {
            string fullName = $"{tourist.FirstName} {tourist.LastName}";
            string reservationFullName = $"{reservation.TouristFirstName} {reservation.TouristLastName}";
            return fullName == reservationFullName;
        }

        private void GenerateVoucher(int userId,int guideId)
        {



            var newVoucher = new Voucher()
            {
                TouristId = userId,
                Reason = "Tour guide cancellation",
                ExpirationDate = DateTime.Now.AddYears(1),
                IsUsed = false,
                GuideId = guideId
            };

            voucherService.Save(newVoucher);
        }




        private void GenerateVoucherQuitJob(int userId, int guideId)
        {
            var existingVouchers = voucherService.GetVouchersForGuide(guideId);

            foreach (var existingVoucher in existingVouchers)
            {
                existingVoucher.GuideId = -1;
                voucherService.Update(existingVoucher);
            }

            var newVoucher = new Voucher()
            {
                TouristId = userId,
                Reason = "Guide quit job",
                ExpirationDate = DateTime.Now.AddYears(2),
                IsUsed = false,
                GuideId = guideId
            };

            voucherService.Save(newVoucher);
        }



    }
}
