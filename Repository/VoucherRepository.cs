using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Voucher> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<Voucher> GetByToueristId(int Id)
        {
            vouchers = GetAll();
            return vouchers.FindAll(t => t.TouristId == Id && t.ExpirationDate>=System.DateTime.Now);
        }
    }
}
