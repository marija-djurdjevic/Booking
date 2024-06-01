using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.Serializer;
using System.Collections.Generic;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Domain.Models.Enums;
using System;

namespace BookingApp.Aplication.UseCases
{
    public class TouristService
    {
        private readonly ITouristRepository touristRepository;
        private readonly ITouristGuideNotificationRepository touristGuideNotificationRepository;
        private readonly IVoucherRepository voucherRepository;

        public TouristService(ITouristRepository touristRepository, ITouristGuideNotificationRepository touristGuideNotificationRepository, IVoucherRepository voucherRepository)
        {
            this.touristRepository = touristRepository;
            this.touristGuideNotificationRepository = touristGuideNotificationRepository;
            this.voucherRepository = voucherRepository;
        }

        public Tourist GetByUserId(int Id)
        {
            var tourists = touristRepository.GetAll();
            return tourists.Find(t => t.Id == Id);
        }

        public int GetAgeById(int touristId)
        {
            var tourist = GetByUserId(touristId);
            return tourist != null ? tourist.Age : -1;
        }

        public void UpdateVisitedToursNumber(int touristId)
        {
            Tourist tourist = GetByUserId(touristId);
            if (tourist != null)
            {
                tourist.VisitedToursDates.Add(DateOnly.FromDateTime(System.DateTime.Now));

                tourist.VisitedToursDates=tourist.VisitedToursDates.FindAll(t => t >= DateOnly.FromDateTime(System.DateTime.Now.AddYears(-1)));
                Update(tourist);
                GiveVoucher(tourist);
            }
        }

        //method that give voucher to tourist if he has visited 5 tours in a year
        public void GiveVoucher(Tourist tourist)
        {
            if (tourist.VisitedToursDates.Count == 5)
            {
                Voucher voucher = new Voucher
                {
                    TouristId = tourist.Id,
                    Reason = "Tourist winning a voucher",
                    ExpirationDate = System.DateTime.Now.AddMonths(6),
                    IsUsed = false
                };
                voucherRepository.Save(voucher);
                tourist.VisitedToursDates.Clear();
                Update(tourist);

                TouristGuideNotification touristGuideNotification = new TouristGuideNotification
                {
                    TouristId = tourist.Id,
                    Type = NotificationType.VoucherWon,
                    Seen = false,
                    CreationTime = System.DateTime.Now,
                    VoucherMessageText = "Congratulations! You've won a voucher for attending 5 tours. You can redeem it within the next 6 months.",
                };
                touristGuideNotificationRepository.Save(touristGuideNotification);
            }
        }

        public void Update(Tourist tourist)
        {
            touristRepository.Update(tourist);
        }
    }
}
