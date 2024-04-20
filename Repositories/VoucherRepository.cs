using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repositories
{
    public class VoucherRepository : IVoucherRepository
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

        public List<Voucher> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public Voucher GetById(int voucherId)
        {
            vouchers = GetAll();
            return vouchers.FirstOrDefault(t => t.Id == voucherId);
        }

        public void Save(Voucher voucher)
        {
            vouchers = GetAll();
            int nextId = NextId();
            voucher.Id = nextId;
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

        public void Delete(int voucherId)
        {
            vouchers = GetAll();
            Voucher existingVoucher = vouchers.FirstOrDefault(t => t.Id == voucherId);
            if (existingVoucher != null)
            {
                vouchers.Remove(existingVoucher);
                _serializer.ToCSV(FilePath, vouchers);
            }
        }

        public int NextId()
        {
            vouchers = GetAll();
            if (vouchers.Count < 1)
            {
                return 1;
            }
            return vouchers.Max(v => v.Id) + 1;
        }
    }
}
