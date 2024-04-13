﻿using BookingApp.Model;
using System.Collections.Generic;
using System;

namespace BookingApp.Service
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

        public void CancelTour(Tour tour, List<KeyPoint> keyPoints, List<TourReservation> tourReservations)
        {
            if ((tour.StartDateTime - DateTime.Now).TotalHours <= 48)
            {
                return;
            }
            liveTourService.RemoveLiveTour(tour.Id);
            tourReservationService.DeleteByTourId(tour.Id);
            DeleteTourAndKeyPoints(tour.Id);
            GenerateVouchersForCanceledTourists(tour.Id, tourReservations);
        }

        private void DeleteTourAndKeyPoints(int tourId)
        {
            tourService.Delete(tourId);
            keyPointService.DeleteKeyPoints(tourId);
        }

        private void GenerateVouchersForCanceledTourists(int tourId, List<TourReservation> tourReservations)
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
                GenerateVoucher(userId);
            }
        }

        private bool IsTouristReservationMatch(Tourist tourist, TourReservation reservation)
        {
            string fullName = $"{tourist.FirstName} {tourist.LastName}";
            string reservationFullName = $"{reservation.TouristFirstName} {reservation.TouristLastName}";
            return fullName == reservationFullName;
        }

        private void GenerateVoucher(int userId)
        {
            var newVoucher = new Voucher()
            {
                TouristId = userId,
                Reason = "Tour guide cancellation",
                ExpirationDate = DateTime.Now.AddYears(1),
                IsUsed = false
            };

            voucherService.Save(newVoucher);
        }
    }
}
