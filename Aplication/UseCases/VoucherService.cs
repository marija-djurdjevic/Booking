using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Aplication.UseCases
{
    public class VoucherService
    {

        private IVoucherRepository voucherRepository;

        public VoucherService(IVoucherRepository voucherRepository)
        {
            this.voucherRepository = voucherRepository;
        }

        public void Save(Voucher voucher)
        {
            voucherRepository.Save(voucher);
        }

        public bool UseVoucher(int voucherId, int touristId)
        {
            Voucher voucher = GetByToueristId(touristId).Find(t => t.Id == voucherId);
            if (voucher != null)
            {
                voucher.IsUsed = true;
                return voucherRepository.Update(voucher);
            }
            return false;
        }


        public List<Voucher> GetVouchersForGuide(int guideId)
        {
            return voucherRepository.GetAll().Where(v => v.GuideId == guideId && v.IsUsed==false).ToList();
        }


        public bool Update(Voucher updatedVoucher)
        {
            return voucherRepository.Update(updatedVoucher);
        }


        public List<Voucher> GetByToueristId(int Id)
        {
            var vouchers = voucherRepository.GetAll();
            return vouchers.FindAll(t =>
            t.TouristId == Id
            && t.ExpirationDate >= System.DateTime.Now
            && !t.IsUsed);
        }
    }
}
