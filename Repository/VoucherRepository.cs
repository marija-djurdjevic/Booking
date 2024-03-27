using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repository
{
    public class VoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/vouchers.csv";

        private readonly Serializer<Voucher> _serializer;

        private List<Voucher> vouchers;

        public VoucherRepository()
        {
            _serializer = new Serializer<Voucher>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            vouchers = _serializer.FromCSV(FilePath);
        }

        public void Save(Voucher voucher)
        {
            vouchers = GetAll();
            vouchers.Add(voucher);
            _serializer.ToCSV(FilePath, vouchers);
        }

        public bool Update(Voucher updatedVoucher)
        {
            vouchers = GetAll();
            Voucher existingVoucher = vouchers.FirstOrDefault(t => t.Id == updatedVoucher.Id);
            if (existingVoucher != null)
            {
                int index = vouchers.IndexOf(existingVoucher);
                vouchers[index] = updatedVoucher;
                _serializer.ToCSV(FilePath, vouchers);
                return true;
            }
            return false;
        }

        public List<Voucher> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public bool UseVoucher(int voucherId,int touristId)
        {
            Voucher voucher = GetByToueristId(touristId).Find(t => t.Id==voucherId);
            if(voucher != null)
            {
                voucher.IsUsed = true;
                return Update(voucher);
            }
            return false;
        }

        public List<Voucher> GetByToueristId(int Id)
        {
            vouchers = GetAll();
            return vouchers.FindAll(t => 
            t.TouristId == Id 
            && t.ExpirationDate>=System.DateTime.Now
            && !t.IsUsed);
        }
    }
}
