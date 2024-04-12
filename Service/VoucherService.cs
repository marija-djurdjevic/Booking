using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class VoucherService
    {

        private VoucherRepository voucherRepository;


        public VoucherService()
        {
            voucherRepository = new VoucherRepository();
        }

        public void Save(Voucher voucher)
        {
            voucherRepository.Save(voucher);
        }


    }
}
