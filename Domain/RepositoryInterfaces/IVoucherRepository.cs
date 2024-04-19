using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IVoucherRepository
    {
        List<Voucher> GetAll();
        void Save(Voucher voucher);
        int NextId();
        void Delete(int id);
        bool Update(Voucher updatedVoucher);
        Voucher GetById(int id);
    }
}
